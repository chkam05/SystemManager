using Newtonsoft.Json.Linq;
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
using SystemManager.Data.Configuration;
using SystemManager.Data.Processes.Data;
using SystemManager.Data.Processes.Events;
using SystemManager.ViewModels.Base;
using SystemManager.ViewModels.Processes;

namespace SystemManager.Data.Processes
{
    public class ProcessesDataContext : BaseViewModel, IDisposable
    {

        //  DELEGATES

        public delegate void ProcessesLoaderFinishedEventHandler(object? sender, ProcessesLoaderFinishedEventArgs e);
        public delegate void WindowsLoaderFinishedEventHandler(object? sender, WindowsLoaderFinishedEventArgs e);


        //  EVENTS

        public ProcessesLoaderFinishedEventHandler? ProcessesLoaded;
        public WindowsLoaderFinishedEventHandler? WindowsLoaded;


        //  VARIABLES

        private BackgroundWorker? _processesLoaderBackgroundWorker;
        private BackgroundWorker? _windowsLoaderBackgroundWorker;
        private ObservableCollection<ProcessInfoViewModel>? _processes;
        private ObservableCollection<ProcessInfoViewModel>? _processesFiltered;

        private AutoProcessesUpdater? _processesAutoUpdater;

        private string _filterText;

        public ProcessManager ProcessManager;


        //  GETTERS & SETTERS

        public ObservableCollection<ProcessInfoViewModel> Processes
        {
            get
            {
                if (_processes == null)
                {
                    _processes = new ObservableCollection<ProcessInfoViewModel>();
                    _processes.CollectionChanged += OnProcessesCollectionChanged;
                }

                return _processes;
            }
            set
            {
                _processes = value;
                _processes.CollectionChanged += OnProcessesCollectionChanged;
                OnPropertyChanged(nameof(Processes));
                OnPropertyChanged(nameof(ProcessesCount));
            }
        }

        public ObservableCollection<ProcessInfoViewModel> ProcessesFiltered
        {
            get
            {
                if (_processesFiltered == null)
                {
                    _processesFiltered = new ObservableCollection<ProcessInfoViewModel>();
                    _processesFiltered.CollectionChanged += OnProcessesFilteredCollectionChanged;
                }

                return _processesFiltered;
            }
            set
            {
                _processesFiltered = value;
                _processesFiltered.CollectionChanged += OnProcessesFilteredCollectionChanged;
                OnPropertyChanged(nameof(ProcessesFiltered));
            }
        }

        public int ProcessesCount
        {
            get => _processes.Count;
        }

        public string FilterText
        {
            get => _filterText;
            set
            {
                UpdateProperty(ref _filterText, value);
                OnPropertyChanged(nameof(IsFilterMode));
                ApplySearchFilter();
            }
        }

        public bool IsFilterMode
        {
            get => !string.IsNullOrEmpty(_filterText) && !string.IsNullOrWhiteSpace(_filterText);
        }

        public bool IsProcessesAutoUpdating
        {
            get => _processesAutoUpdater?.IsRunning ?? false;
        }

        public bool IsProcessesLoading
        {
            get => _processesLoaderBackgroundWorker != null && _processesLoaderBackgroundWorker.IsBusy;
        }

        public bool IsWindowsLoading
        {
            get => _windowsLoaderBackgroundWorker != null && _windowsLoaderBackgroundWorker.IsBusy;
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> ProcessesDataContext class constructor. </summary>
        public ProcessesDataContext()
        {
            _filterText = string.Empty;

            Processes = new ObservableCollection<ProcessInfoViewModel>();
            ProcessesFiltered = new ObservableCollection<ProcessInfoViewModel>();

            ProcessManager = new ProcessManager();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Performs tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
        public void Dispose()
        {
            if (_processesLoaderBackgroundWorker != null && _processesLoaderBackgroundWorker.IsBusy)
                _processesLoaderBackgroundWorker.CancelAsync();
        }

        #endregion CLASS METHODS

        #region AUTO UPDATER METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Start processes auto updater. </summary>
        public void StartProcessesAutoUpdater()
        {
            if (_processesAutoUpdater != null && _processesAutoUpdater.IsRunning)
                return;

            if (_processesLoaderBackgroundWorker != null && _processesLoaderBackgroundWorker.IsBusy)
                StopLoadingProcesses();

            _processesAutoUpdater = new AutoProcessesUpdater(
                Processes.Select(p => p.ProcessInfo), ProcessManager);

            _processesAutoUpdater.Update += ProcessesAutoUpdaterUpdated;
            _processesAutoUpdater.UpdateFinished += ProcessesAutoUpdaterFinished;
            _processesAutoUpdater.StartAutoUpdater();

            OnPropertyChanged(nameof(IsProcessesAutoUpdating));
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Stop processes auto updater. </summary>
        public void StopProcessesAutoUpdater()
        {
            if (_processesAutoUpdater == null || !_processesAutoUpdater.IsRunning)
                return;

            _processesAutoUpdater.StopAutoUpdater();

            OnPropertyChanged(nameof(IsProcessesAutoUpdating));
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after processes auto updater update process. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Auto Process Update Event Arguments. </param>
        public void ProcessesAutoUpdaterUpdated(object? sender, AutoProcessUpdateEventArgs e)
        {
            if (e.ChangeIndication)
            {
                switch (e.ComparationResult)
                {
                    case ProcessCompareResult.NotEqual:
                        if (e.CurrentProcess != null && e.NewProcess != null)
                        {
                            var currentProcessViewModel = Processes.FirstOrDefault(p => p.Id == e.CurrentProcess.Id);

                            if (currentProcessViewModel != null)
                            {
                                currentProcessViewModel.Update(e.NewProcess);

                                if (IsFilterMode)
                                {
                                    var filteredProcessViewModel = ProcessesFiltered.FirstOrDefault(p => p.Id == e.CurrentProcess.Id);

                                    if (filteredProcessViewModel != null)
                                        filteredProcessViewModel.Update(e.NewProcess);

                                    else if (FilterProcessInfoViewModel(currentProcessViewModel, FilterText))
                                        ProcessesFiltered.Add(currentProcessViewModel);
                                }
                            }
                        }
                        break;

                    case ProcessCompareResult.New:
                        if (e.NewProcess != null)
                        {
                            var newProcessInfoViewModel = new ProcessInfoViewModel(e.NewProcess);

                            Processes.Add(newProcessInfoViewModel);

                            if (FilterProcessInfoViewModel(newProcessInfoViewModel, FilterText))
                                ProcessesFiltered.Add(newProcessInfoViewModel);
                        }
                        break;

                    case ProcessCompareResult.Removed:
                        if (e.CurrentProcess != null)
                        {
                            var currentProcessViewModel = Processes.FirstOrDefault(p => p.Id == e.CurrentProcess.Id);

                            if (currentProcessViewModel != null)
                            {
                                Processes.Remove(currentProcessViewModel);

                                if (ProcessesFiltered.Contains(currentProcessViewModel))
                                    ProcessesFiltered.Remove(currentProcessViewModel);
                            }
                        }
                        break;
                }
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after process auto updater finished work. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Auto Process Update Finished Event Arguments. </param>
        public void ProcessesAutoUpdaterFinished(object? sender, AutoProcessUpdateFinishedEventArgs e)
        {
            OnPropertyChanged(nameof(IsProcessesAutoUpdating));
        }

        #endregion AUTO UPDATER METHODS

        #region FILTERING METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Apply search filter. </summary>
        private void ApplySearchFilter()
        {
            if (!IsFilterMode)
            {
                ProcessesFiltered.Clear();
                return;
            }

            ProcessesFiltered.Clear();
            ProcessesFiltered = new ObservableCollection<ProcessInfoViewModel>(Processes.Where(
                p => FilterProcessInfoViewModel(p, _filterText)));
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Apply search filter on single ProcessInfo view model. </summary>
        /// <param name="processInfoViewModel"> Process info view model. </param>
        /// <param name="searchText"> Query text. </param>
        /// <returns> True - process info item match query; False - otherwise. </returns>
        private bool FilterProcessInfoViewModel(ProcessInfoViewModel processInfoViewModel, string searchText)
        {
            var processInfoOption = ConfigManager.Instance.Config.ProcessInfoOptions;
            bool queryResult = false;

            if (processInfoOption.Name && processInfoViewModel.Name != null)
                queryResult = processInfoViewModel.Name.ToLower().Contains(searchText.ToLower())
                    ? true
                    : queryResult;

            if (processInfoOption.Description && processInfoViewModel.Description != null)
                queryResult = processInfoViewModel.Description.ToLower().Contains(searchText.ToLower())
                    ? true
                    : queryResult;

            if (processInfoOption.CommandLocation && processInfoViewModel.CommandLocation != null)
                queryResult = processInfoViewModel.CommandLocation.ToLower().Contains(searchText.ToLower())
                    ? true
                    : queryResult;

            if (processInfoOption.UserName && processInfoViewModel.UserName != null)
                queryResult = processInfoViewModel.UserName.ToLower().Contains(searchText.ToLower())
                    ? true
                    : queryResult;

            return queryResult;
        }

        #endregion FILTERING METHODS

        #region PROCESSES MANAGEMENT METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Add ProcessInfoViewModel to processes collection. </summary>
        /// <param name="processInfoViewModel"> Process info view model. </param>
        private void AddProcessInfoViewModel(ProcessInfoViewModel processInfoViewModel)
        {
            if (processInfoViewModel == null)
                return;

            Processes.Add(processInfoViewModel);

            if (IsFilterMode && FilterProcessInfoViewModel(processInfoViewModel, _filterText))
                ProcessesFiltered.Add(processInfoViewModel);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Clear processes collections. </summary>
        private void ClearProcesses()
        {
            Processes.Clear();
            ProcessesFiltered.Clear();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Load processes. </summary>
        /// <returns> True - loading started; False - otherwise. </returns>
        public bool LoadProcesses()
        {
            if (_processesLoaderBackgroundWorker != null && _processesLoaderBackgroundWorker.IsBusy)
                return false;

            _processesLoaderBackgroundWorker = new BackgroundWorker();
            _processesLoaderBackgroundWorker.WorkerReportsProgress = true;
            _processesLoaderBackgroundWorker.WorkerSupportsCancellation = true;

            _processesLoaderBackgroundWorker.DoWork += LoadProcessesWork;
            _processesLoaderBackgroundWorker.ProgressChanged += LoadProcessesWorkProgressChanged;
            _processesLoaderBackgroundWorker.RunWorkerCompleted += LoadProcessesWorkCompleted;

            ClearProcesses();

            _processesLoaderBackgroundWorker.RunWorkerAsync();

            OnPropertyChanged(nameof(IsProcessesLoading));

            return IsProcessesLoading;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Stop loading processes. </summary>
        public void StopLoadingProcesses()
        {
            if (_processesLoaderBackgroundWorker != null && _processesLoaderBackgroundWorker.IsBusy)
                _processesLoaderBackgroundWorker.CancelAsync();
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
            int processesInfoCount = 0;
            int processIndex = 0;
            int progress = 0;

            //  Get processes.

            try
            {
                processesInfo = ProcessManager.GetProcesses(out List<Exception> exceptions);
                processesInfoCount = processesInfo.Count;
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

            //  Consider stopping loading.

            if (e.Cancel || (worker?.CancellationPending ?? false))
            {
                e.Result = new object?[]
                {
                    exception,
                    processIndex
                };
                return;
            }

            //  Load each process.

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
            if (e.UserState is ProcessInfoViewModel processInfoViewModel)
                AddProcessInfoViewModel(processInfoViewModel);
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
                    ClearProcesses();
                }
                else if (e.Cancelled)
                {
                    ProcessesLoaded?.Invoke(this, new ProcessesLoaderFinishedEventArgs(stopped: true));
                    ClearProcesses();
                }
                else
                {
                    ProcessesLoaded?.Invoke(this, new ProcessesLoaderFinishedEventArgs(lastProcessIndex: processIndex ?? -1));
                }
            }

            OnPropertyChanged(nameof(ProcessesCount));
            OnPropertyChanged(nameof(IsProcessesLoading));
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

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after ProcessesFiltered collection change. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Notify Collection Changed Event Arguments. </param>
        private void OnProcessesFilteredCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ProcessesFiltered));
        }

        #endregion PROPERITES CHANGED METHODS

        #region WINDOWS MANAGEMENT METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Load windows. </summary>
        /// <param name="processInfo"> Process info. </param>
        /// <returns> True - loading started; False - otherwise. </returns>
        public bool LoadWindows(ProcessInfo processInfo)
        {
            if (processInfo == null)
                return false;

            if (!ProcessManager.IsProcessAlive(processInfo, out _) || !processInfo.HasWindows)
                return false;

            if (_windowsLoaderBackgroundWorker != null && _windowsLoaderBackgroundWorker.IsBusy)
                return false;

            _windowsLoaderBackgroundWorker = new BackgroundWorker();
            _windowsLoaderBackgroundWorker.WorkerSupportsCancellation = true;

            _windowsLoaderBackgroundWorker.DoWork += LoadWindowsWork;
            _windowsLoaderBackgroundWorker.RunWorkerCompleted += LoadWindowsWorkCompleted;

            _windowsLoaderBackgroundWorker.RunWorkerAsync(processInfo);

            OnPropertyChanged(nameof(IsWindowsLoading));

            return IsWindowsLoading;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Stop loading windows. </summary>
        public void StopLoadingWindows()
        {
            if (_windowsLoaderBackgroundWorker != null && _windowsLoaderBackgroundWorker.IsBusy)
                _windowsLoaderBackgroundWorker.CancelAsync();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Load windows background worker work method. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Do Work Event Arguments. </param>
        public void LoadWindowsWork(object? sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;

            Exception? exception = null;
            ProcessInfo? processInfo = e.Argument as ProcessInfo;
            List<WindowInfo>? windows;

            if (processInfo == null)
            {
                e.Result = new object?[]
                {
                    exception,
                    new List<WindowInfoViewModel>()
                };
                return;
            }

            //  Get windows.

            try
            {
                windows = ProcessManager.GetWindows(processInfo);
            }
            catch (Exception exc)
            {
                e.Result = new object?[]
                {
                    exc,
                    new List<WindowInfoViewModel>()
                };
                return;
            }

            //  Consider stopping loading.

            if (e.Cancel || (worker?.CancellationPending ?? false))
            {
                e.Result = new object?[]
                {
                    exception,
                    new List<WindowInfoViewModel>()
                };
                return;
            }

            //  If no windows.

            if (!(windows?.Any() ?? false))
            {
                e.Result = new object?[]
                {
                    exception,
                    new List<WindowInfoViewModel>()
                };
                return;
            }

            //  Load each window.

            e.Result = new object?[]
            {
                exception,
                windows.Select(w => new WindowInfoViewModel(w)).ToList()
            };
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Load windows background worker work completed method. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Run Worker Completed Event Arguments. </param>
        public void LoadWindowsWorkCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result is object?[] result)
            {
                Exception? exception = result[0] as Exception;
                List<WindowInfoViewModel>? windowInfoViewModels = result[1] as List<WindowInfoViewModel>;

                if (exception != null)
                {
                    WindowsLoaded?.Invoke(this, new WindowsLoaderFinishedEventArgs(exception, windowInfoViewModels));
                }
                else if (e.Cancelled)
                {
                    WindowsLoaded?.Invoke(this, new WindowsLoaderFinishedEventArgs(stopped: true));
                }
                else
                {
                    WindowsLoaded?.Invoke(this, new WindowsLoaderFinishedEventArgs(windows: windowInfoViewModels));
                }
            }

            OnPropertyChanged(nameof(IsWindowsLoading));
        }

        #endregion WINDOWS MANAGEMENT METHODS

    }
}
