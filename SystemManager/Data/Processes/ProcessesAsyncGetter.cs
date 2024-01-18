using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemController.ProcessesManagement;
using SystemController.ProcessesManagement.Data;
using SystemManager.Data.Processes.Data;
using SystemManager.Data.Processes.Events;

namespace SystemManager.Data.Processes
{
    public class ProcessesAsyncGetter : IDisposable
    {

        //  DELEGATES

        public delegate void UpdateEventHandler(object? sender, ProcessesGetterUpdateEventArgs e);
        public delegate void UpdateFinishedEventHandler(object? sender, ProgressesGetterUpdateFinishedEventArgs e);


        //  EVENTS

        public UpdateEventHandler? Update;
        public UpdateFinishedEventHandler? UpdateFinished;


        //  VARIABLES

        private ProcessManager _processManager;

        private BackgroundWorker? _bgWorker;
        private List<ProcessInfo> _processes;
        private object _processesLock = new object();


        //  GETTERS & SETTERS

        public List<ProcessInfo> Processes
        {
            get
            {
                lock (_processesLock)
                {
                    return _processes;
                }
            }
            set
            {
                lock (_processes)
                {
                    _processes = value;
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
        /// <summary> ProcessesAsyncGetter class constructor. </summary>
        /// <param name="processManager"> Process manager. </param>
        /// <param name="currentProcesses"> Current processes. </param>
        public ProcessesAsyncGetter(ProcessManager processManager, IEnumerable<ProcessInfo>? currentProcesses = null)
        {
            _processManager = processManager;

            _processes = currentProcesses?.Any() ?? false
                ? currentProcesses.ToList()
                : new List<ProcessInfo>();
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
        /// <param name="autoUpdate"> Auto update mode. </param>
        public void Run(bool autoUpdate = false)
        {
            if (IsRunning)
                return;

            _bgWorker = new BackgroundWorker();
            _bgWorker.WorkerReportsProgress = true;
            _bgWorker.WorkerSupportsCancellation = true;
            _bgWorker.DoWork += Work;
            _bgWorker.ProgressChanged += ProgressChanged;
            _bgWorker.RunWorkerCompleted += WorkCompleted;

            var args = new ProcessAsyncGetterDoWorkArgs(autoUpdate);

            _bgWorker.RunWorkerAsync(args);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Stop processes getter. </summary>
        public void Stop()
        {
            if (_bgWorker?.IsBusy ?? false)
                _bgWorker.CancelAsync();
        }

        #endregion INTERACTION METHODS

        #region WORKER METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Compare processes informations. </summary>
        /// <param name="processes"> Current and new processes tuple. </param>
        /// <returns> Process compare result. </returns>
        private ProcessCompareResult CompareProcesses(Tuple<ProcessInfo?, ProcessInfo?> processes)
        {
            var currentProcessInfo = processes.Item1;
            var newProcessInfo = processes.Item2;

            if (newProcessInfo == null)
                return ProcessCompareResult.Removed;

            else if (currentProcessInfo == null)
                return ProcessCompareResult.New;

            else if (currentProcessInfo.Equals(newProcessInfo))
                return ProcessCompareResult.Equal;

            return ProcessCompareResult.NotEqual;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get new processes. </summary>
        /// <param name="exceptions"> List of exceptions occured while getting processes. </param>
        /// <returns> List of processes. </returns>
        private List<ProcessInfo> GetProcesses(out List<Exception> exceptions)
        {
            try
            {
                return _processManager.GetProcesses(out exceptions);
            }
            catch (Exception exception)
            {
                exceptions = new List<Exception> { exception };
                return new List<ProcessInfo>();
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Group current and new processes by Id. </summary>
        /// <param name="currentProcesses"> List of current processes information. </param>
        /// <param name="newProcesses"> List of new processes information. </param>
        /// <returns></returns>
        private Dictionary<int, Tuple<ProcessInfo?, ProcessInfo?>> GroupProcesses(
            List<ProcessInfo> currentProcesses, List<ProcessInfo> newProcesses)
        {
            return currentProcesses.Concat(newProcesses)
                .GroupBy(process => process.Id)
                .ToDictionary(
                    group => group.Key,
                    group => new Tuple<ProcessInfo?, ProcessInfo?>(
                        group.FirstOrDefault(p => currentProcesses.Contains(p)) ?? null,
                        group.FirstOrDefault(p => newProcesses.Contains(p)) ?? null)
                );
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
            var args = e.Argument as ProcessAsyncGetterDoWorkArgs;
            var autoUpdate = args?.AutoUpdate ?? false;
            var bgWorker = sender as BackgroundWorker;

            do
            {
                List<ProcessInfo> newProcesses = GetProcesses(out List<Exception> exceptions);

                if (IsCancelled(bgWorker, e))
                    return;

                if (exceptions?.Any() ?? false)
                {
                    var progressReport = new ProcessesGetterUpdateEventArgs(
                        false,
                        exceptions: exceptions);

                    bgWorker?.ReportProgress(0, progressReport);
                    continue;
                }

                var grouppedProcesses = GroupProcesses(Processes, newProcesses);

                foreach (var processes in grouppedProcesses)
                {
                    if (IsCancelled(bgWorker, e))
                        return;

                    var comparationResult = CompareProcesses(processes.Value);

                    if (comparationResult != ProcessCompareResult.Equal)
                    {
                        var progressReport = new ProcessesGetterUpdateEventArgs(
                            true,
                            comparationResult,
                            processes.Value.Item1,
                            processes.Value.Item2);

                        bgWorker?.ReportProgress(0, progressReport);
                    }
                }
            }
            while (!IsCancelled(bgWorker, e) && !autoUpdate);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Background worker on progress changed method. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Progress Changed Event Arguments. </param>
        private void ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            if (e.UserState is ProcessesGetterUpdateEventArgs progressReport)
            {
                if (progressReport.ChangeIndication)
                {
                    switch (progressReport.ComparationResult)
                    {
                        case ProcessCompareResult.Equal:
                            return;

                        case ProcessCompareResult.NotEqual:
                            if (progressReport.CurrentProcess != null)
                                Processes.Remove(progressReport.CurrentProcess);

                            if (progressReport.NewProcess != null)
                                Processes.Add(progressReport.NewProcess);
                            break;

                        case ProcessCompareResult.New:
                            if (progressReport.NewProcess != null)
                                Processes.Add(progressReport.NewProcess);
                            break;

                        case ProcessCompareResult.Removed:
                            if (progressReport.CurrentProcess != null)
                                Processes.Remove(progressReport.CurrentProcess);
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
            UpdateFinished?.Invoke(this, new ProgressesGetterUpdateFinishedEventArgs());
        }

        #endregion WORKER METHODS

    }
}
