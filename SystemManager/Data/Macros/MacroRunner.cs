using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemController.MouseKeyboard;
using SystemController.MouseKeyboard.Data;
using SystemManager.Data.Macros.DataModels;
using SystemManager.Data.Macros.Events;
using SystemManager.ViewModels.Base;

namespace SystemManager.Data.Macros
{
    public class MacroRunner : BaseViewModel, IDisposable
    {

        //  CONST

        private static readonly JsonSerializerSettings SERIALIZER_SETTINGS = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
        };


        //  DELEGATES

        public delegate void RunnerStartEventHandler(object? sender, MacroRunnerStartEventArgs e);
        public delegate void RunnerFinishedEventHandler(object? sender, MacroRunnerFinishedEventArgs e);


        //  EVENTS

        public event RunnerStartEventHandler? RunnerStart;
        public event RunnerFinishedEventHandler? RunnerFinished;


        //  VARIABLES

        private string? _currentFilePath;
        private ObservableCollection<MacroBase> _macroItems;
        private bool _isRunning = false;
        private BackgroundWorker? _runnerWorker;

        private byte[] _keyCodes;
        private MouseButton? _mouseButton;
        private int _mousePositionX = 0;
        private int _mousePositionY = 0;
        private int _mouseScrollX = 0;
        private int _mouseScrollY = 0;

        public KeyboardReader KeyboardReader;
        public MouseReader MouseReader;


        //  GETTERS & SETTERS

        public string? CurrentFilePath
        {
            get => _currentFilePath;
            private set => UpdateProperty(ref _currentFilePath, value);
        }

        public bool CanSaveCurrent
        {
            get
            {
                if (string.IsNullOrEmpty(_currentFilePath))
                    return false;

                var fileDirectoryPath = Path.GetDirectoryName(_currentFilePath);

                if (!Directory.Exists(fileDirectoryPath))
                    return false;

                return true;
            }
        }

        public ObservableCollection<MacroBase> MacroItems
        {
            get => _macroItems;
            set
            {
                UpdateProperty(ref _macroItems, value);
                _macroItems.CollectionChanged += OnMacroItemsCollectionChanged;
            }
        }

        public bool IsRunning
        {
            get => _isRunning;
            private set => UpdateProperty(ref _isRunning, value);
        }

        public byte[] KeyCodes
        {
            get => _keyCodes;
            set => UpdateProperty(ref _keyCodes, value);
        }

        public int MousePositionX
        {
            get => _mousePositionX;
            set => UpdateProperty(ref _mousePositionX, value);
        }

        public int MousePositionY
        {
            get => _mousePositionY;
            set => UpdateProperty(ref _mousePositionY, value);
        }

        public MouseButton? MouseButton
        {
            get => _mouseButton;
            set => UpdateProperty(ref _mouseButton, value);
        }

        public int MouseScrollX
        {
            get => _mouseScrollX;
            set => UpdateProperty(ref _mouseScrollX, value);
        }

        public int MouseScrollY
        {
            get => _mouseScrollY;
            set => UpdateProperty(ref _mouseScrollY, value);
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> MacroRunner class constructor. </summary>
        public MacroRunner()
        {
            _macroItems = new ObservableCollection<MacroBase>();
            KeyboardReader = new KeyboardReader();
            MouseReader = new MouseReader();

            SetupKeyboardReader();
            SetupMouseReader();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Performs tasks associated with freeing, releasing, or resetting unmanaged resources. </summary>
        public void Dispose()
        {
            if (MouseReader.IsListening)
                MouseReader.StopListening();
        }

        #endregion CLASS METHODS

        #region FILES METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Clear MacroItems. </summary>
        public void Clear()
        {
            _currentFilePath = null;

            MacroItems.Clear();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Open MacroItems from file. </summary>
        /// <param name="filePath"> File path. </param>
        public void OpenMacroItems(string filePath)
        {
            if (File.Exists(filePath))
            {
                var fileContent = File.ReadAllText(filePath);
                var macroItems = JsonConvert.DeserializeObject<List<MacroBase>>(fileContent);

                CurrentFilePath = filePath;

                MacroItems = (macroItems?.Any() ?? false)
                    ? new ObservableCollection<MacroBase>(macroItems)
                    : new ObservableCollection<MacroBase>();
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Save MacroItems to file. </summary>
        /// <param name="filePath"> File path. </param>
        /// <param name="overwrite"> Overwrite previous loaded file. </param>
        public void SaveMacroItems(string? filePath, bool overwrite = false)
        {
            var saveFilePath = CanSaveCurrent ? _currentFilePath : filePath;

            if (!string.IsNullOrEmpty(saveFilePath))
            {
                var fileDirectoryPath = Path.GetDirectoryName(saveFilePath);

                if (Directory.Exists(fileDirectoryPath))
                {
                    var fileContent = JsonConvert.SerializeObject(MacroItems, SERIALIZER_SETTINGS);
                    File.WriteAllText(saveFilePath, fileContent);
                }
            }
        }

        #endregion FILES METHODS

        #region PROPERITES CHANGED METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after changing KeyCodes collection. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Notify Collection Changed Event Arguments. </param>
        protected void OnMacroItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(MacroItems));
        }

        #endregion PROPERITES CHANGED METHODS

        #region RUN METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Start run. </summary>
        public void Run()
        {
            if (_runnerWorker != null && _runnerWorker.IsBusy)
                return;

            _runnerWorker = new BackgroundWorker()
            {
                WorkerSupportsCancellation = true
            };

            _runnerWorker.DoWork += RunnerWork;
            _runnerWorker.RunWorkerCompleted += RunnerWorkCompleted;

            IsRunning = true;
            _runnerWorker.RunWorkerAsync();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Stop run. </summary>
        public void StopRun()
        {
            if (_runnerWorker != null && _runnerWorker.IsBusy)
                _runnerWorker.CancelAsync();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Runner worker async method. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Do Work Event Arguments. </param>
        private void RunnerWork(object? sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;

            if (MacroItems.Any())
            {
                foreach (var macroItem in MacroItems)
                {
                    if (e.Cancel || (worker?.CancellationPending ?? false))
                        return;

                    try
                    {
                        if (macroItem is MacroDelay macroDelay)
                        {

                        }
                        else if (macroItem is MacroKeyDown macroKeyDown)
                        {

                        }
                        else if (macroItem is MacroKeyUp macroKeyUp)
                        {

                        }
                        else if (macroItem is MacroKeyClick macroKeyClick)
                        {

                        }
                        else if (macroItem is MacroKeyCombination macroKeyCombination)
                        {

                        }
                        else if (macroItem is MacroMouseDown macroMouseDown)
                        {

                        }
                        else if (macroItem is MacroMouseUp macroMouseUp)
                        {

                        }
                        else if (macroItem is MacroMouseClick macroMouseClick)
                        {

                        }
                        else if (macroItem is MacroMouseMove macroMouseMove)
                        {

                        }
                        else if (macroItem is MacroMouseScrollHorizontal macroMouseScrollHorizontal)
                        {

                        }
                        else if (macroItem is MacroMouseScrollVertical macroMouseScrollVertical)
                        {

                        }
                    }
                    catch (Exception exc)
                    {
                        e.Result = exc;
                        return;
                    }
                }
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after runner worker finished work. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Run Worker Completed Event Arguments. </param>
        private void RunnerWorkCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                RunnerFinished?.Invoke(this, new MacroRunnerFinishedEventArgs(null, true));

            if (e.Result is Exception exc)
                RunnerFinished?.Invoke(this, new MacroRunnerFinishedEventArgs(exc));

            RunnerFinished?.Invoke(this, new MacroRunnerFinishedEventArgs(null));

            IsRunning = false;
        }

        #endregion RUN METHODS

        #region SETUP METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Setup KeyboardReader. </summary>
        private void SetupKeyboardReader()
        {
            KeyboardReader.OnKeyPress += (s, e) =>
            {
                if (e.KeyState == KeyState.Press)
                    KeyCodes = e.HeldKeyCodes;
            };

            KeyboardReader.StartListening();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Setup MouseReader. </summary>
        private void SetupMouseReader()
        {
            MouseReader.OnMouseClick += (s, e) =>
            {
                if (e.State == KeyState.Press)
                    MouseButton = e.Button;
            };

            MouseReader.OnMouseMove += (s, e) =>
            {
                MousePositionX = e.CurrentPosition.X;
                MousePositionY = e.CurrentPosition.Y;
            };

            MouseReader.OnMouseScroll += (s, e) =>
            {
                switch (e.Orientation)
                {
                    case ScrollOrientation.Vertical:
                        MouseScrollY = e.Delta;
                        break;

                    case ScrollOrientation.Horizontal:
                        MouseScrollX = e.Delta;
                        break;
                }
            };

            MouseReader.StartListening();
        }

        #endregion SETUP METHODS

    }
}
