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
using SystemController.Processes;
using SystemController.Processes.Data;
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

        public delegate void ProcessesLoaderFinishedEventHandler(object? sender, ProcessesGetterUpdateFinishedEventArgs e);
        public delegate void WindowsLoaderFinishedEventHandler(object? sender, WindowsGetterUpdateFinishedEventArgs e);


        //  EVENTS

        public ProcessesLoaderFinishedEventHandler? ProcessesLoaded;
        public WindowsLoaderFinishedEventHandler? WindowsLoaded;


        //  VARIABLES

        private ProcessesAsyncGetter _processesAsyncGetter;
        private WindowsAsyncGetter _windowsAsyncGetter;
        private bool _isAutoUpdate = false;

        private ObservableCollection<ProcessInfoViewModel>? _filteredProcesses;
        private ObservableCollection<ProcessInfoViewModel>? _processes;
        private ObservableCollection<WindowInfoViewModel>? _windows;

        private string _filterText = string.Empty;

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
                if (_filteredProcesses == null)
                {
                    _filteredProcesses = new ObservableCollection<ProcessInfoViewModel>();
                    _filteredProcesses.CollectionChanged += OnFilteredProcessesCollectionChanged;
                }

                return _filteredProcesses;
            }
            set
            {
                _filteredProcesses = value;
                _filteredProcesses.CollectionChanged += OnFilteredProcessesCollectionChanged;
                OnPropertyChanged(nameof(ProcessesFiltered));
            }
        }

        public ObservableCollection<WindowInfoViewModel> Windows
        {
            get
            {
                if (_windows == null)
                {
                    _windows = new ObservableCollection<WindowInfoViewModel>();
                    _windows.CollectionChanged += OnWindowsCollectionChanged;
                }

                return _windows;
            }
            set
            {
                _windows = value;
                _windows.CollectionChanged += OnWindowsCollectionChanged;
                OnPropertyChanged(nameof(Windows));
            }
        }

        public string FilterText
        {
            get => _filterText;
            set
            {
                UpdateProperty(ref _filterText, value);
                OnPropertyChanged(nameof(IsFilterMode));
                FilterProcesses();
            }
        }

        public bool IsAutoUpdate
        {
            get => _isAutoUpdate;
            private set
            {
                UpdateProperty(ref _isAutoUpdate, value);
            }
        }

        public bool IsFilterMode
        {
            get => !string.IsNullOrEmpty(_filterText) && !string.IsNullOrWhiteSpace(_filterText);
        }

        public bool IsLoading
        {
            get => _processesAsyncGetter.IsRunning;
        }

        public bool IsWindowsLoading
        {
            get => _windowsAsyncGetter.IsRunning;
        }

        public int ProcessesCount
        {
            get => Processes.Count;
        }



        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> ProcessesDataContext class constructor. </summary>
        public ProcessesDataContext()
        {
            ProcessManager = new ProcessManager();
            Processes = new ObservableCollection<ProcessInfoViewModel>();
            ProcessesFiltered = new ObservableCollection<ProcessInfoViewModel>();

            _processesAsyncGetter = new ProcessesAsyncGetter(
                ProcessManager,
                Processes.Select(p => p.ProcessInfo));

            _processesAsyncGetter.Update += OnProcessUpdate;
            _processesAsyncGetter.UpdateFinished += OnProcessesUpdateFinished;

            _windowsAsyncGetter = new WindowsAsyncGetter(ProcessManager, null);
            _windowsAsyncGetter.Update += OnWindowUpdate;
            _windowsAsyncGetter.UpdateFinished += OnWindowUpdateFinished;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Performs tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
        public void Dispose()
        {
            _processesAsyncGetter.Dispose();
            _windowsAsyncGetter.Dispose();
        }

        #endregion CLASS METHODS

        #region FILTER PROCESSES

        //  --------------------------------------------------------------------------------
        /// <summary> Filter processes. </summary>
        private void FilterProcesses()
        {
            if (!IsFilterMode)
            {
                ProcessesFiltered.Clear();
                return;
            }

            ProcessesFiltered.Clear();
            ProcessesFiltered = new ObservableCollection<ProcessInfoViewModel>(Processes.Where(
                p => FilterSingleProcess(p, _filterText)));
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Filter single process. </summary>
        /// <param name="processInfoViewModel"> Process info view model. </param>
        /// <param name="searchText"> Query text. </param>
        /// <returns> True - process info item match query; False - otherwise. </returns>
        private bool FilterSingleProcess(ProcessInfoViewModel processInfoViewModel, string searchText)
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

        #endregion FILTER PROCESSES

        #region PROCESSES GETTER METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Start loading processes. </summary>
        /// <param name="autoUpdate"> Auto update mode. </param>
        public bool LoadProcesses(bool autoUpdate = false)
        {
            if (_processesAsyncGetter.IsRunning)
                return false;

            if (_processesAsyncGetter.Run(autoUpdate))
            {
                IsAutoUpdate = autoUpdate;
                OnPropertyChanged(nameof(IsLoading));
                return true;
            }

            return false;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Stop loading processes. </summary>
        public void StopLoadingProcesses()
        {
            if (_processesAsyncGetter.IsRunning)
                _processesAsyncGetter.Stop();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after process update. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Processes Getter Update Event Arguments. </param>
        public void OnProcessUpdate(object? sender, ProcessesGetterUpdateEventArgs e)
        {
            if (!e.ChangeIndication)
                return;

            switch (e.ComparationResult)
            {
                case ProcessCompareResult.NotEqual:
                    if (e.CurrentProcess != null && e.NewProcess != null)
                    {
                        var currentProcessInfoViewModel = Processes.FirstOrDefault(p => p.Id == e.CurrentProcess.Id);

                        if (currentProcessInfoViewModel == null)
                            return;

                        currentProcessInfoViewModel.Update(e.NewProcess);

                        if (IsFilterMode)
                        {
                            var filteredProcessViewModel = ProcessesFiltered.FirstOrDefault(p => p.Id == e.CurrentProcess.Id);

                            if (filteredProcessViewModel != null)
                                filteredProcessViewModel.Update(e.NewProcess);

                            else if (FilterSingleProcess(currentProcessInfoViewModel, FilterText))
                                ProcessesFiltered.Add(currentProcessInfoViewModel);
                        }
                    }
                    break;

                case ProcessCompareResult.New:
                    if (e.NewProcess != null)
                    {
                        var newProcessInfoViewModel = new ProcessInfoViewModel(e.NewProcess);

                        Processes.Add(newProcessInfoViewModel);

                        if (FilterSingleProcess(newProcessInfoViewModel, FilterText))
                            ProcessesFiltered.Add(newProcessInfoViewModel);
                    }
                    break;

                case ProcessCompareResult.Removed:
                    if (e.CurrentProcess != null)
                    {
                        var currentProcessInfoViewModel = Processes.FirstOrDefault(p => p.Id == e.CurrentProcess.Id);

                        if (currentProcessInfoViewModel == null)
                            return;

                        Processes.Remove(currentProcessInfoViewModel);

                        if (ProcessesFiltered.Contains(currentProcessInfoViewModel))
                            ProcessesFiltered.Remove(currentProcessInfoViewModel);
                    }
                    break;
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after finished processes update. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Processes Getter Update Finished Event Arguments. </param>
        public void OnProcessesUpdateFinished(object? sender, ProcessesGetterUpdateFinishedEventArgs e)
        {
            IsAutoUpdate = false;
            OnPropertyChanged(nameof(IsLoading));
            ProcessesLoaded?.Invoke(this, e);
        }

        #endregion PROCESSES GETTER METHODS

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
        /// <summary> Method invoked after FilteredProcesses collection change. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Notify Collection Changed Event Arguments. </param>
        private void OnFilteredProcessesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ProcessesFiltered));
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after Windows collection change. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Notify Collection Changed Event Arguments. </param>
        private void OnWindowsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Windows));
        }

        #endregion PROPERITES CHANGED METHODS

        #region WINDOWS GETTER METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Start loading windows. </summary>
        /// <param name="processInfo"> Process info. </param>
        public bool LoadWindows(ProcessInfo processInfo)
        {
            if (_windowsAsyncGetter.IsRunning)
                _windowsAsyncGetter.Stop();

            if (_windowsAsyncGetter.Run(processInfo))
            {
                OnPropertyChanged(nameof(IsWindowsLoading));
                return true;
            }

            return false;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Stop loading windows. </summary>
        public void StopLoadingWindows()
        {
            if (_windowsAsyncGetter.IsRunning)
                _processesAsyncGetter.Stop();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after window update. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Windows Getter Update Event Arguments. </param>
        public void OnWindowUpdate(object? sender, WindowsGetterUpdateEventArgs e)
        {
            if (!e.ChangeIndication)
                return;

            switch (e.ComparationResult)
            {
                case ProcessCompareResult.NotEqual:
                    if (e.CurrentWindow != null && e.NewWindow != null)
                    {
                        var currentWindowInfoViewModel = Windows.FirstOrDefault(w => w.WindowInfo.Handle == e.CurrentWindow.Handle);

                        if (currentWindowInfoViewModel == null)
                            return;

                        currentWindowInfoViewModel.Update(e.NewWindow);
                    }
                    break;

                case ProcessCompareResult.New:
                    if (e.NewWindow != null)
                    {
                        var newWindowInfoViewModel = new WindowInfoViewModel(e.NewWindow);

                        Windows.Add(newWindowInfoViewModel);
                    }
                    break;

                case ProcessCompareResult.Removed:
                    if (e.CurrentWindow != null)
                    {
                        var currentWindowInfoViewModel = Windows.FirstOrDefault(w => w.WindowInfo.Handle == e.CurrentWindow.Handle);

                        if (currentWindowInfoViewModel == null)
                            return;

                        Windows.Remove(currentWindowInfoViewModel);
                    }
                    break;
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after finished windows update. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Windows Getter Update Finished Event Arguments. </param>
        public void OnWindowUpdateFinished(object? sender, WindowsGetterUpdateFinishedEventArgs e)
        {
            OnPropertyChanged(nameof(IsWindowsLoading));
            WindowsLoaded?.Invoke(this, e);
        }

        #endregion WINDOWS GETTER METHODS

    }
}
