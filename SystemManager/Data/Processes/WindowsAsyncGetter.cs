using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemController.ProcessesManagement.Data;
using SystemController.ProcessesManagement;
using SystemManager.Data.Processes.Data;
using SystemManager.Data.Processes.Events;
using System.Diagnostics;
using SystemManager.ViewModels.Processes;
using System.Threading;

namespace SystemManager.Data.Processes
{
    public class WindowsAsyncGetter : IDisposable
    {

        //  DELEGATES

        public delegate void UpdateEventHandler(object? sender, WindowsGetterUpdateEventArgs e);
        public delegate void UpdateFinishedEventHandler(object? sender, WindowsGetterUpdateFinishedEventArgs e);


        //  EVENTS

        public UpdateEventHandler? Update;
        public UpdateFinishedEventHandler? UpdateFinished;


        //  VARIABLES

        private ProcessManager _processManager;
        private ProcessInfo? _processInfo;

        private BackgroundWorker? _bgWorker;
        private List<WindowInfo> _windows;
        private object _windowsLock = new object();


        //  GETTERS & SETTERS

        public List<WindowInfo> Windows
        {
            get
            {
                lock (_windowsLock)
                {
                    return _windows;
                }
            }
            set
            {
                lock (_windowsLock)
                {
                    _windows = value;
                }
            }
        }

        public bool IsRunning
        {
            get => _bgWorker != null && _bgWorker.IsBusy;
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> WindowsAsyncGetter class constructor. </summary>
        /// <param name="processManager"> Process manager. </param>
        /// <param name="currentWindows"> Current windows. </param>
        public WindowsAsyncGetter(ProcessManager processManager, IEnumerable<WindowInfo>? currentWindows = null)
        {
            _processManager = processManager;

            _windows = currentWindows?.Any() ?? false
                ? currentWindows.ToList()
                : new List<WindowInfo>();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Performs tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
        public void Dispose()
        {
            Stop();
        }

        #endregion CLASS METHODS

        #region INTERACTION METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Run processes getter. </summary>
        /// <param name="processInfo"> Process info. </param>
        /// <param name="autoUpdate"> Auto update mode. </param>
        public bool Run(ProcessInfo processInfo, bool autoUpdate = false)
        {
            var isNewProcess = _processInfo == null || _processInfo.Id != processInfo.Id;

            if (isNewProcess)
                _processInfo = processInfo;

            if (IsRunning)
            {
                if (isNewProcess)
                    _bgWorker?.CancelAsync();
                else
                    return false;
            }

            _bgWorker = new BackgroundWorker();
            _bgWorker.WorkerReportsProgress = true;
            _bgWorker.WorkerSupportsCancellation = true;
            _bgWorker.DoWork += Work;
            _bgWorker.ProgressChanged += ProgressChanged;
            _bgWorker.RunWorkerCompleted += WorkCompleted;

            var args = new WindowsAsyncGetterDoWorkArgs(autoUpdate, processInfo);

            _bgWorker.RunWorkerAsync(args);

            return true;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Stop processes getter. </summary>
        public void Stop()
        {
            if (_bgWorker?.IsBusy ?? false)
                _bgWorker.CancelAsync();
        }

        #endregion INTERACTION METHODS

        #region WORK METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Compare windows informations. </summary>
        /// <param name="windows"> Current and new windows tuple. </param>
        /// <returns> Process compare result. </returns>
        private ProcessCompareResult CompareWindows(Tuple<WindowInfo?, WindowInfo?> windows)
        {
            var currentWindowInfo = windows.Item1;
            var newWindowInfo = windows.Item2;

            if (newWindowInfo == null)
                return ProcessCompareResult.Removed;

            else if (currentWindowInfo == null)
                return ProcessCompareResult.New;

            else if (currentWindowInfo.Equals(newWindowInfo))
                return ProcessCompareResult.Equal;

            return ProcessCompareResult.NotEqual;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Group current and new windows. </summary>
        /// <param name="currentWindows"> List of current window information. </param>
        /// <param name="newWindows"> List of new window information. </param>
        /// <returns> Windows info dictionary. </returns>
        private Dictionary<int, Tuple<WindowInfo?, WindowInfo?>> GroupWindows(
            List<WindowInfo> currentWindows, List<WindowInfo> newWindows)
        {
            return currentWindows.Concat(newWindows)
                .GroupBy(window => window.Handle)
                .ToDictionary(
                    group => (int) group.Key,
                    group => new Tuple<WindowInfo?, WindowInfo?>(
                        group.FirstOrDefault(p => currentWindows.Contains(p)) ?? null,
                        group.FirstOrDefault(p => newWindows.Contains(p)) ?? null)
                );
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get new windows. </summary>
        /// <param name="exceptions"> List of exceptions occured while getting windows. </param>
        /// <returns> List of windows. </returns>
        private List<WindowInfo> GetWindows(ProcessInfo processInfo, out List<Exception> exceptions)
        {
            try
            {
                exceptions = new List<Exception>();
                return _processManager.GetWindows(processInfo);
            }
            catch (Exception exception)
            {
                exceptions = new List<Exception> { exception };
                return new List<WindowInfo>();
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Check if work process has been cancelled. </summary>
        /// <param name="bgWorker"> Background worker. </param>
        /// <param name="e"> Do Work Event Arguments. </param>
        /// <returns> True - work cancelled; False - oterwise. </returns>
        private bool IsCancelled(BackgroundWorker? bgWorker, DoWorkEventArgs e)
        {
            if (bgWorker?.CancellationPending ?? false)
                return true;

            if (e.Cancel)
                return true;

            return false;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Background worker work method. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Do Work Event Arguments. </param>
        private void Work(object? sender, DoWorkEventArgs e)
        {
            var args = e.Argument as WindowsAsyncGetterDoWorkArgs;
            var autoUpdate = args?.AutoUpdate ?? false;
            var bgWorker = sender as BackgroundWorker;
            var processInfo = args?.ProcessInfo ?? null;

            if (processInfo == null)
                return;

            do
            {
                var newWindows = GetWindows(processInfo, out List<Exception> exceptions);

                if (IsCancelled(bgWorker, e))
                {
                    e.Result = new WindowsGetterUpdateFinishedEventArgs(stopped: true);
                    return;
                }

                if (exceptions?.Any() ?? false)
                {
                    var progressReport = new WindowsGetterUpdateEventArgs(
                        false,
                        exceptions: exceptions);

                    bgWorker?.ReportProgress(0, progressReport);
                    continue;
                }

                var grouppedWindows = GroupWindows(Windows, newWindows);

                foreach (var windows in grouppedWindows)
                {
                    if (IsCancelled(bgWorker, e))
                    {
                        e.Result = new WindowsGetterUpdateFinishedEventArgs(stopped: true);
                        return;
                    }

                    var comparationResult = CompareWindows(windows.Value);

                    if (comparationResult != ProcessCompareResult.Equal)
                    {
                        var progressReport = new WindowsGetterUpdateEventArgs(
                            true,
                            comparationResult,
                            windows.Value.Item1,
                            windows.Value.Item2);

                        bgWorker?.ReportProgress(0, progressReport);
                    }
                }
            }
            while (!IsCancelled(bgWorker, e) && autoUpdate);

            Thread.Sleep(1000);

            e.Result = new WindowsGetterUpdateFinishedEventArgs(
                windows: Windows.Select(w => new WindowInfoViewModel(w)).ToList());
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Background worker on progress changed method. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Progress Changed Event Arguments. </param>
        private void ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            if (e.UserState is WindowsGetterUpdateEventArgs progressReport)
            {
                if (progressReport.ChangeIndication)
                {
                    switch (progressReport.ComparationResult)
                    {
                        case ProcessCompareResult.Equal:
                            return;

                        case ProcessCompareResult.NotEqual:
                            if (progressReport.CurrentWindow != null)
                                Windows.Remove(progressReport.CurrentWindow);

                            if (progressReport.NewWindow != null)
                                Windows.Add(progressReport.NewWindow);
                            break;

                        case ProcessCompareResult.New:
                            if (progressReport.NewWindow != null)
                                Windows.Add(progressReport.NewWindow);
                            break;

                        case ProcessCompareResult.Removed:
                            if (progressReport.CurrentWindow != null)
                                Windows.Remove(progressReport.CurrentWindow);
                            break;
                    }
                }

                Update?.Invoke(this, progressReport);
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Background worker on finished work method. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Run Worker Completed Event Arguments. </param>
        private void WorkCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                UpdateFinished?.Invoke(this, new WindowsGetterUpdateFinishedEventArgs(stopped: true));

            else if (e.Result is WindowsGetterUpdateFinishedEventArgs result)
                UpdateFinished?.Invoke(this, result);
        }

        #endregion WORK METHODS

    }
}
