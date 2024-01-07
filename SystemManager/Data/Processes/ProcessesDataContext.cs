using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using SystemController.ProcessesManagement;
using SystemController.ProcessesManagement.Data;
using SystemManager.Data.Processes.Events;
using SystemManager.ViewModels.Base;
using SystemManager.ViewModels.Processes;

namespace SystemManager.Data.Processes
{
    public class ProcessesDataContext : BaseViewModel, IDisposable
    {

        //  DELEGATES

        public delegate void ProcessesLoaderFinishedEventHandler(object? sender, ProcessesLoaderFinishedEventArgs e);


        //  EVENTS

        public ProcessesLoaderFinishedEventHandler? ProcessesLoaded;


        //  VARIABLES

        private BackgroundWorker? _loaderBackgroundWorker;
        private ObservableCollection<ProcessInfoViewModel> _processes;

        public ProcessManager ProcessManager;


        //  GETTERS & SETTERS

        public ObservableCollection<ProcessInfoViewModel> Processes
        {
            get => _processes;
            set
            {
                _processes = value;
                _processes.CollectionChanged += OnProcessesCollectionChanged;
                OnPropertyChanged(nameof(Processes));
                OnPropertyChanged(nameof(ProcessesCount));
            }
        }

        public int ProcessesCount
        {
            get => _processes.Count;
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> ProcessesDataContext class constructor. </summary>
        public ProcessesDataContext()
        {
            _processes = new ObservableCollection<ProcessInfoViewModel>();

            ProcessManager = new ProcessManager();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Performs tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
        public void Dispose()
        {
            if (_loaderBackgroundWorker != null && _loaderBackgroundWorker.IsBusy)
                _loaderBackgroundWorker.CancelAsync();
        }

        #endregion CLASS METHODS

        #region PROCESSES MANAGEMENT METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Load processes. </summary>
        public void LoadProcesses()
        {
            if (_loaderBackgroundWorker != null && _loaderBackgroundWorker.IsBusy)
                return;

            _loaderBackgroundWorker = new BackgroundWorker();
            _loaderBackgroundWorker.WorkerReportsProgress = true;
            _loaderBackgroundWorker.WorkerSupportsCancellation = true;

            _loaderBackgroundWorker.DoWork += LoadProcessesWork;
            _loaderBackgroundWorker.ProgressChanged += LoadProcessesWorkProgressChanged;
            _loaderBackgroundWorker.RunWorkerCompleted += LoadProcessesWorkCompleted;

            Processes.Clear();

            _loaderBackgroundWorker.RunWorkerAsync();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Stop loading processes. </summary>
        public void StopLoadingProcesses()
        {
            if (_loaderBackgroundWorker != null && _loaderBackgroundWorker.IsBusy)
                _loaderBackgroundWorker.CancelAsync();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Load processes background worker work method. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Do Work Event Arguments. </param>
        private void LoadProcessesWork(object? sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;

            Exception? exception = null;
            List<ProcessInfo> processesInfo = new List<ProcessInfo>();
            int processIndex = 0;
            int progress = 0;

            try
            {
                processesInfo = ProcessManager.GetProcesses(out List<Exception> exceptions);
            }
            catch (Exception exc)
            {
                e.Result = new object?[]
                {
                    exc,
                    processIndex
                };
                return;
            }

            var processesInfoCount = processesInfo.Count;


            if (e.Cancel || (worker?.CancellationPending ?? false))
            {
                e.Result = new object?[]
                {
                    exception,
                    processIndex
                };
                return;
            }

            foreach (var processInfo in processesInfo)
            {
                if (e.Cancel || (worker?.CancellationPending ?? false))
                    break;

                progress = (processIndex * 100) / processesInfoCount;
                worker?.ReportProgress(progress, new ProcessInfoViewModel(processInfo));
                processIndex++;
            }

            e.Result = new object?[]
            {
                exception,
                processIndex
            };
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Load processes background worker work progress changed method. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Progress Changed Event Arguments. </param>
        private void LoadProcessesWorkProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            if (e.UserState is ProcessInfoViewModel processInfoViewModel && processInfoViewModel != null)
                Processes.Add(processInfoViewModel);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Load processes background worker work completed method. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Run Worker Completed Event Arguments. </param>
        private void LoadProcessesWorkCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result is object?[] result)
            {
                Exception? exception = result[0] as Exception;
                int? processIndex = result[1] as int?;

                if (exception != null)
                {
                    ProcessesLoaded?.Invoke(this, new ProcessesLoaderFinishedEventArgs(exception, processIndex ?? -1));
                    Processes.Clear();
                }
                else if (e.Cancelled)
                {
                    ProcessesLoaded?.Invoke(this, new ProcessesLoaderFinishedEventArgs(stopped: true));
                    Processes.Clear();
                }
                else
                {
                    ProcessesLoaded?.Invoke(this, new ProcessesLoaderFinishedEventArgs(lastProcessIndex: processIndex ?? -1));
                }
            }

            OnPropertyChanged(nameof(Processes));
            OnPropertyChanged(nameof(ProcessesCount));
        }

        #endregion PROCESSES MANAGEMENT METHODS

        #region PROPERITES CHANGED METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after Processes collection change. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Notify Collection Changed Event Arguments. </param>
        private void OnProcessesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Processes));
            OnPropertyChanged(nameof(ProcessesCount));
        }

        #endregion PROPERITES CHANGED METHODS

    }
}
