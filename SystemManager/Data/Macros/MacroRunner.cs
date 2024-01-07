using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SystemController.MouseKeyboard;
using SystemController.MouseKeyboard.Data;
using SystemManager.Data.Macros.DataModels;
using SystemManager.Data.Macros.Events;
using SystemManager.Data.Macros.Serialization;
using SystemManager.ViewModels.Base;

namespace SystemManager.Data.Macros
{
    public class MacroRunner : BaseViewModel, IDisposable
    {

        //  CONST

        private static readonly JsonSerializerSettings SERIALIZER_SETTINGS = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
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
                var macroItems = JsonConvert.DeserializeObject<List<MacroBase>>(fileContent, new MacroConverter());

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

            RunnerStart?.Invoke(this, new MacroRunnerStartEventArgs());

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
            var mouseKeyboardController = new MouseKeyboardController();
            var worker = sender as BackgroundWorker;

            Exception? exception = null;
            MacroBase? currentMacroItem = null;
            int macoItemIndex = -1;

            if (MacroItems.Any())
            {
                foreach (var macroItem in MacroItems)
                {
                    if (e.Cancel || (worker?.CancellationPending ?? false))
                    {
                        e.Result = new object?[]
                        {
                            exception,
                            currentMacroItem,
                            -1
                        };
                        return;
                    }

                    currentMacroItem = macroItem;
                    macoItemIndex++;

                    try
                    {
                        if (macroItem is MacroDelay macroDelay)
                        {
                            int delay = LongToInt(macroDelay.DelayMiliseconds, false);
                            Thread.Sleep(delay);
                        }
                        else if (macroItem is MacroKeyDown macroKeyDown)
                        {
                            mouseKeyboardController.SimulateKeyPress(
                                macroKeyDown.KeyCode,
                                KeyState.Press);
                        }
                        else if (macroItem is MacroKeyUp macroKeyUp)
                        {
                            mouseKeyboardController.SimulateKeyPress(
                                macroKeyUp.KeyCode,
                                KeyState.Release);
                        }
                        else if (macroItem is MacroKeyClick macroKeyClick)
                        {
                            mouseKeyboardController.SimulateKeyPress(
                                macroKeyClick.KeyCode,
                                KeyState.PressRelease);
                        }
                        else if (macroItem is MacroKeyCombination macroKeyCombination)
                        {
                            mouseKeyboardController.SimulateKeyCombination(
                                macroKeyCombination.KeyCodes.ToArray());
                        }
                        else if (macroItem is MacroMouseDown macroMouseDown)
                        {
                            mouseKeyboardController.SimulateMouseClick(
                                macroMouseDown.MouseButton,
                                KeyState.Press);
                        }
                        else if (macroItem is MacroMouseUp macroMouseUp)
                        {
                            mouseKeyboardController.SimulateMouseClick(
                                macroMouseUp.MouseButton,
                                KeyState.Release);
                        }
                        else if (macroItem is MacroMouseClick macroMouseClick)
                        {
                            mouseKeyboardController.SimulateMouseClick(
                                macroMouseClick.MouseButton,
                                KeyState.PressRelease);
                        }
                        else if (macroItem is MacroMouseMove macroMouseMove)
                        {
                            mouseKeyboardController.SimulateMouseMove(
                                Convert.ToInt32(macroMouseMove.X),
                                Convert.ToInt32(macroMouseMove.Y));
                        }
                        else if (macroItem is MacroMouseScrollHorizontal macroMouseScrollHorizontal)
                        {
                            mouseKeyboardController.SimulateHorizontalMouseScroll(
                                Convert.ToInt32(macroMouseScrollHorizontal.ScrollShift));
                        }
                        else if (macroItem is MacroMouseScrollVertical macroMouseScrollVertical)
                        {
                            mouseKeyboardController.SimulateVerticalMouseScroll(
                                Convert.ToInt32(macroMouseScrollVertical.ScrollShift));
                        }
                    }
                    catch (Exception exc)
                    {
                        e.Result = new object?[]
                        {
                            exc,
                            currentMacroItem,
                            macoItemIndex
                        };
                        return;
                    }
                }
            }

            e.Result = new object?[]
            {
                exception,
                currentMacroItem,
                macoItemIndex
            };
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after runner worker finished work. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Run Worker Completed Event Arguments. </param>
        private void RunnerWorkCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result is object?[] result)
            {
                var exception = (Exception?)result[0];
                var macroItem = (MacroBase?)result[1];
                var macroItemIndex = (int?)result[2];

                if (e.Cancelled)
                    RunnerFinished?.Invoke(this, new MacroRunnerFinishedEventArgs(
                        macroItem: macroItem,
                        macroItemIndex: macroItemIndex ?? -1,
                        stopped: true));

                else if (exception != null)
                {
                    RunnerFinished?.Invoke(this, new MacroRunnerFinishedEventArgs(
                        exception: exception,
                        macroItem: macroItem,
                        macroItemIndex: macroItemIndex ?? -1));
                }

                else
                {
                    RunnerFinished?.Invoke(this, new MacroRunnerFinishedEventArgs(
                        macroItem: macroItem,
                        macroItemIndex: macroItemIndex ?? -1));
                }
            }

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

        #region UTILITY METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Convert long to int. </summary>
        /// <param name="value"> Long value. </param>
        /// <param name="allowNegative"> Allow negative numbers. </param>
        /// <returns> Converted long to int value. </returns>
        private int LongToInt(long value, bool allowNegative = true)
        {
            if (value > int.MaxValue)
                return int.MaxValue;

            if (!allowNegative && value < 0)
                return 0;

            if (value < int.MinValue)
                return int.MinValue;

            return Convert.ToInt32(value);
        }

        #endregion UTILITY METHODS

    }
}
