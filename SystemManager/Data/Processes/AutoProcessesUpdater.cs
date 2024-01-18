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
using SystemManager.ViewModels.Processes;

namespace SystemManager.Data.Processes
{
    public class AutoProcessesUpdater : IDisposable
    {

        //  DELEGATES

        public delegate void UpdateEventHandler(object? sender, AutoProcessUpdateEventArgs e);
        public delegate void UpdateFinishedEventHandler(object? sender, AutoProcessUpdateFinishedEventArgs e);


        //  EVENTS

        public UpdateEventHandler? Update;
        public UpdateFinishedEventHandler? UpdateFinished;


        //  VARIABLES

        private BackgroundWorker? _bgUpdater;
        private List<ProcessInfo> _processes;
        private ProcessManager _processManager;
        private object _processLock = new object();


        //  GETTERS & SETTERS

        public List<ProcessInfo> Processes
        {
            get
            {
                lock (_processLock)
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
            get => _bgUpdater != null && _bgUpdater.IsBusy;
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> AutoProcessesUpdater class constructor. </summary>
        /// <param name="currentProcesses"> Current processes collection. </param>
        public AutoProcessesUpdater(IEnumerable<ProcessInfo> currentProcesses, ProcessManager processManager)
        {
            _processes = currentProcesses.ToList();
            _processManager = processManager;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Performs tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
        public void Dispose()
        {
            if (IsRunning)
                _bgUpdater?.CancelAsync();
        }

        #endregion CLASS METHODS

        #region INTERACTION METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Remove process from list. </summary>
        /// <param name="processInfo"> Process information. </param>
        public void RemoveProcess(ProcessInfo processInfo)
        {
            StopAutoUpdater();

            if (Processes.Contains(processInfo))
                Processes.Remove(processInfo);

            StartAutoUpdater();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Start auto updater. </summary>
        public void StartAutoUpdater()
        {
            if (IsRunning)
                return;

            _bgUpdater = new BackgroundWorker();
            _bgUpdater.WorkerReportsProgress = true;
            _bgUpdater.WorkerSupportsCancellation = true;
            _bgUpdater.DoWork += UpdaterWork;
            _bgUpdater.ProgressChanged += UpdaterProgressChanged;
            _bgUpdater.RunWorkerCompleted += UpdaterWorkCompleted;

            _bgUpdater.RunWorkerAsync();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Stop auto updater. </summary>
        public void StopAutoUpdater()
        {
            if (IsRunning)
                _bgUpdater?.CancelAsync();
        }

        #endregion INTERACTION METHODS

        #region UPDATER METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Updater background worker work method. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Do Work Event Arguments. </param>
        private void UpdaterWork(object? sender, DoWorkEventArgs e)
        {
            var bgWorker = sender as BackgroundWorker;

            List<ProcessInfo> newProcesses = new List<ProcessInfo>();

            while (!IsCancelled(bgWorker, e))
            {
                try
                {
                    //  Get new processes.

                    newProcesses = _processManager.GetProcesses(out List<Exception> exceptions);

                    if (IsCancelled(bgWorker, e))
                            return;

                    if (exceptions.Any())
                    {
                        var progressReport = new AutoProcessUpdateEventArgs(
                            false,
                            exceptions: exceptions);

                        bgWorker?.ReportProgress(0, progressReport);
                        newProcesses.Clear();
                        continue;
                    }
                }
                catch (Exception exception)
                {
                    var progressReport = new AutoProcessUpdateEventArgs(
                        false,
                        exceptions: new List<Exception> { exception });

                    bgWorker?.ReportProgress(0, progressReport);
                    newProcesses.Clear();
                    continue;
                }

                var grouppedProcesses = GroupProcesses(Processes, newProcesses);

                //  Check each process.

                foreach (var processes in grouppedProcesses)
                {
                    if (IsCancelled(bgWorker, e))
                        return;

                    var comparationResult = CompareProcesses(processes.Value);

                    if (comparationResult != ProcessCompareResult.Equal)
                    {
                        var progressReport = new AutoProcessUpdateEventArgs(true,
                            comparationResult,
                            processes.Value.Item1,
                            processes.Value.Item2);

                        bgWorker?.ReportProgress(0, progressReport);
                    }
                }
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after updater background worker progress changed. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Progress Changed Event Arguments. </param>
        private void UpdaterProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            if (e.UserState is AutoProcessUpdateEventArgs progressReport)
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
        /// <summary> Method invoked after background worker finished work. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Run Worker Completed Event Arguments. </param>
        private void UpdaterWorkCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            UpdateFinished?.Invoke(this, new AutoProcessUpdateFinishedEventArgs());
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Check if background worker has been canceled. </summary>
        /// <param name="bgWorker"> Background worker. </param>
        /// <param name="e"> Do Work Event Arguments. </param>
        /// <returns> True - background worker has been canceled; False - otherwise. </returns>
        private bool IsCancelled(BackgroundWorker? bgWorker, DoWorkEventArgs e)
        {
            if (bgWorker?.CancellationPending ?? false)
                return true;

            if (e.Cancel)
                return true;

            return false;
        }

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
        /// <summary> Group current and new processes by Id. </summary>
        /// <param name="currentProcesses"> List of current processes information. </param>
        /// <param name="newProcesses"> List of new processes information. </param>
        /// <returns></returns>
        private Dictionary<int, Tuple<ProcessInfo?, ProcessInfo?>> GroupProcesses(
            List<ProcessInfo> currentProcesses, List<ProcessInfo> newProcesses)
        {
            return currentProcesses
                .GroupBy(process => process.Id)
                .ToDictionary(
                    group => group.Key,
                    group => new Tuple<ProcessInfo?, ProcessInfo?>(
                        group.FirstOrDefault(p => currentProcesses.Contains(p)),
                        group.FirstOrDefault(p => newProcesses.Contains(p)))
                );
        }

        #endregion UPDATER METHODS

    }
}
