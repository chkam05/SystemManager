using chkam05.Tools.ControlsEx;
using chkam05.Tools.ControlsEx.InternalMessages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SystemManager.Data.Macros;
using SystemController.MouseKeyboard.Data;
using MouseButton = SystemController.MouseKeyboard.Data.MouseButton;
using SystemManager.Data.Macros.DataModels;
using SystemManager.Utilities;
using SystemManager.InternalMessages;
using chkam05.Tools.ControlsEx.Data;
using System.Collections.ObjectModel;
using chkam05.Tools.ControlsEx.Events;
using static chkam05.Tools.ControlsEx.Events.Delegates;
using SystemManager.Data.Macros.Events;
using SystemManager.Controls;
using System.Reflection;

namespace SystemManager.Pages
{
    public partial class MacrosPage : Page
    {

        //  VARIABLES

        private InternalMessagesExContainer _imContainer;
        private ListViewItemExMoveHelper<MacroBase> _itemsMoveHelper;
        private MacroRunner _macroRunner;


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> MacrosPage class constructor. </summary>
        /// <param name="imContainer"> InternalMessageExContainer. </param>
        public MacrosPage(InternalMessagesExContainer imContainer)
        {
            //  Setup components.
            _imContainer = imContainer;
            _macroRunner = new MacroRunner();
            _macroRunner.RunnerStart += OnRunnerStart;
            _macroRunner.RunnerFinished += OnRunnerEnd;

            //  Initialize user interface.
            InitializeComponent();

            //  Setup context.
            DataContext = _macroRunner;

            //  Setup tools.
            _itemsMoveHelper = new ListViewItemExMoveHelper<MacroBase>(_macroItemsListView);
        }

        #endregion CLASS METHODS

        #region FILES MANAGEMENT METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Create new Macro. </summary>
        private void CreateNewMacro()
        {
            InternalMessageClose<InternalMessageCloseEventArgs> closeHandler = (s, e) =>
            {
                if (e.Result == InternalMessageResult.Yes)
                    _macroRunner.Clear();
            };

            if (_macroRunner.MacroItems.Any())
            {
                var internalMessageClose = CreateLeaveCurrentMacroIM("New Macro", closeHandler);
                _imContainer.ShowMessage(internalMessageClose);
            }
            else
            {
                closeHandler?.Invoke(null, new InternalMessageCloseEventArgs(InternalMessageResult.Yes));
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Open Macro from file. </summary>
        private void OpenMacroFromFile()
        {
            InternalMessageClose<FilesSelectorInternalMessageCloseEventArgs> closeFileHandler = (s, e) =>
            {
                if (e.Result == InternalMessageResult.Ok)
                {
                    try
                    {
                        _macroRunner.OpenMacroItems(e.FilePath);
                    }
                    catch (Exception exc)
                    {
                        var errorIM = CreateOpenMacroErrorIM(e.FilePath, exc);
                        _imContainer.ShowMessage(errorIM);
                    }
                }
            };

            InternalMessageClose<InternalMessageCloseEventArgs> closeHandler = (s, e) =>
            {
                if (e.Result == InternalMessageResult.Yes)
                {
                    var openMacroInternalMessage = CreateOpenMacroIM(closeFileHandler);
                    _imContainer.ShowMessage(openMacroInternalMessage);
                }
            };

            if (_macroRunner.MacroItems.Any())
            {
                var internalMessageClose = CreateLeaveCurrentMacroIM("New Macro", closeHandler);
                _imContainer.ShowMessage(internalMessageClose);
            }
            else
            {
                closeHandler?.Invoke(null, new InternalMessageCloseEventArgs(InternalMessageResult.Yes));
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Save Macro to file. </summary>
        private void SaveMacroToFile()
        {
            if (_macroRunner.CanSaveCurrent)
            {
                try
                {
                    _macroRunner.SaveMacroItems(null, true);
                }
                catch (Exception exc)
                {
                    var errorIM = CreateSaveMacroErrorIM(_macroRunner.CurrentFilePath ?? string.Empty, exc);
                    _imContainer.ShowMessage(errorIM);
                }
            }
            else
            {
                SaveMacroToNewFile();
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Save Macro to new file. </summary>
        private void SaveMacroToNewFile()
        {
            InternalMessageClose<FilesSelectorInternalMessageCloseEventArgs> closeFileHandler = (s, e) =>
            {
                if (e.Result == InternalMessageResult.Ok)
                {
                    try
                    {
                        _macroRunner.SaveMacroItems(e.FilePath, false);
                    }
                    catch (Exception exc)
                    {
                        var errorIM = CreateSaveMacroErrorIM(e.FilePath, exc);
                        _imContainer.ShowMessage(errorIM);
                    }
                }
            };

            var internalMessage = CreateSaveMacroIM(closeFileHandler);
            _imContainer.ShowMessage(internalMessage);
        }


        #endregion FILES MANAGEMENT METHODS

        #region HEADER BUTTON INTERACTION METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking AddMacroItem ButtonEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void AddMacroItemButtonExClick(object sender, RoutedEventArgs e)
        {
            ((ButtonEx)sender).ContextMenu.IsOpen = true;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking File ButtonEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void FileButtonExClick(object sender, RoutedEventArgs e)
        {
            ((ButtonEx)sender).ContextMenu.IsOpen = true;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking RunMacro ButtonEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void RunMacroButtonExClick(object sender, RoutedEventArgs e)
        {
            if (_macroRunner.IsRunning)
                _macroRunner.StopRun();
            else
                _macroRunner.Run();
        }

        #endregion HEADER BUTTON INTERACTION METHODS

        #region HEADER FILE CONTEXT MENU INTERACTION METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking FileNew ContextMenuItemEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void FileNewContextMenuItemExClick(object sender, RoutedEventArgs e)
        {
            CreateNewMacro();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking FileOpen ContextMenuItemEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void FileOpenContextMenuItemExClick(object sender, RoutedEventArgs e)
        {
            OpenMacroFromFile();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking FileSave ContextMenuItemEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void FileSaveContextMenuItemExClick(object sender, RoutedEventArgs e)
        {
            SaveMacroToFile();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking FileSaveAs ContextMenuItemEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void FileSaveAsContextMenuItemExClick(object sender, RoutedEventArgs e)
        {
            SaveMacroToNewFile();
        }

        #endregion HEADER FILE CONTEXT MENU INTERACTION METHODS

        #region HEADER ADD MACROS CONTEXT METNU INTERACTION METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking AddMacroDelay ContextMenuItemEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void AddMacroDelayContextMenuItemExClick(object sender, RoutedEventArgs e)
        {
            _macroRunner.MacroItems.Add(new MacroDelay());
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking AddMacroKeyDown ContextMenuItemEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void AddMacroKeyDownContextMenuItemExClick(object sender, RoutedEventArgs e)
        {
            _macroRunner.MacroItems.Add(new MacroKeyDown());
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking AddMacroKeyUp ContextMenuItemEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void AddMacroKeyUpContextMenuItemExClick(object sender, RoutedEventArgs e)
        {
            _macroRunner.MacroItems.Add(new MacroKeyUp());
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking AddMacroKeyClick ContextMenuItemEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void AddMacroKeyClickContextMenuItemExClick(object sender, RoutedEventArgs e)
        {
            _macroRunner.MacroItems.Add(new MacroKeyClick());
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking AddMacroKeyCombination ContextMenuItemEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void AddMacroKeyCombinationContextMenuItemExClick(object sender, RoutedEventArgs e)
        {
            _macroRunner.MacroItems.Add(new MacroKeyCombination());
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking AddMacroMouseDown ContextMenuItemEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void AddMacroMouseDownContextMenuItemExClick(object sender, RoutedEventArgs e)
        {
            _macroRunner.MacroItems.Add(new MacroMouseDown());
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking AddMacroMouseUp ContextMenuItemEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void AddMacroMouseUpContextMenuItemExClick(object sender, RoutedEventArgs e)
        {
            _macroRunner.MacroItems.Add(new MacroMouseUp());
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking AddMacroMouseClick ContextMenuItemEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void AddMacroMouseClickContextMenuItemExClick(object sender, RoutedEventArgs e)
        {
            _macroRunner.MacroItems.Add(new MacroMouseClick());
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking AddMacroMouseMove ContextMenuItemEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void AddMacroMouseMoveContextMenuItemExClick(object sender, RoutedEventArgs e)
        {
            _macroRunner.MacroItems.Add(new MacroMouseMove());
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking AddMacroScrollHorizontal ContextMenuItemEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void AddMacroMouseScrollHorizontalContextMenuItemExClick(object sender, RoutedEventArgs e)
        {
            _macroRunner.MacroItems.Add(new MacroMouseScrollHorizontal());
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking AddMacroScrollVertical ContextMenuItemEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void AddMacroMouseScrollVerticalContextMenuItemExClick(object sender, RoutedEventArgs e)
        {
            _macroRunner.MacroItems.Add(new MacroMouseScrollVertical());
        }

        #endregion HEADER ADD MACROS CONTEXT METNU INTERACTION METHODS

        #region INTERNAL MESSAGES

        //  --------------------------------------------------------------------------------
        /// <summary> Create leave current macro InternalMessageEx. </summary>
        /// <param name="title"> InternalMessageEx title. </param>
        /// <param name="closeHandler"> Method that will be invoked after closing InternalMessageEx. </param>
        /// <returns> InternalMessageEx. </returns>
        private InternalMessageEx CreateLeaveCurrentMacroIM(string title, InternalMessageClose<InternalMessageCloseEventArgs> closeHandler)
        {
            var message = "Do you want to delete the current Macro?";
            var internalMessage = InternalMessageEx.CreateQuestionMessage(_imContainer, title, message);

            internalMessage.OnClose += closeHandler;

            InternalMessageExHelper.SetInternalMessageAppearance(internalMessage);

            return internalMessage;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Create open macro from file InternalMessageEx. </summary>
        /// <param name="closeHandler"> Method that will be invoked after closing InternalMessageEx. </param>
        /// <returns> InternalMessageEx. </returns>
        private FilesSelectorInternalMessageEx CreateOpenMacroIM(InternalMessageClose<FilesSelectorInternalMessageCloseEventArgs> closeHandler)
        {
            var title = "Open Macro file";
            var internalMessage = FilesSelectorInternalMessageEx.CreateOpenFileInternalMessageEx(_imContainer, title);

            internalMessage.FilesTypes = new ObservableCollection<InternalMessageFileType>()
            {
                new InternalMessageFileType("Macro File", new string[1] { "*.json" })
            };

            internalMessage.OnClose += closeHandler;

            InternalMessageExHelper.SetFilesSelectorInternalMessageAppearance(internalMessage);

            return internalMessage;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Create opening macro from file error InternalMessageEx. </summary>
        /// <param name="filePath"> Macro file path. </param>
        /// <param name="exc"> Exception. </param>
        /// <returns> InternalMessageEx. </returns>
        private InternalMessageEx CreateOpenMacroErrorIM(string filePath, Exception exc)
        {
            var title = "Open Macro file error";
            var message = $"Could not open macro file:{Environment.NewLine}{filePath}{Environment.NewLine}{exc.Message}";

            var internalMessage = InternalMessageEx.CreateErrorMessage(_imContainer, title, message);

            InternalMessageExHelper.SetInternalMessageAppearance(internalMessage);

            return internalMessage;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Create save macro to file InternalMessageEx. </summary>
        /// <param name="closeHandler"> Method that will be invoked after closing InternalMessageEx. </param>
        /// <returns> InternalMessageEx. </returns>
        private FilesSelectorInternalMessageEx CreateSaveMacroIM(InternalMessageClose<FilesSelectorInternalMessageCloseEventArgs> closeHandler)
        {
            var title = "Save Macro file";
            var internalMessage = FilesSelectorInternalMessageEx.CreateSaveFileInternalMessageEx(_imContainer, title);

            internalMessage.FilesTypes = new ObservableCollection<InternalMessageFileType>()
            {
                new InternalMessageFileType("Macro File", new string[1] { "*.json" })
            };

            internalMessage.OnClose += closeHandler;

            InternalMessageExHelper.SetFilesSelectorInternalMessageAppearance(internalMessage);

            return internalMessage;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Create save macro from to error InternalMessageEx. </summary>
        /// <param name="filePath"> Macro file path. </param>
        /// <param name="exc"> Exception. </param>
        /// <returns> InternalMessageEx. </returns>
        private InternalMessageEx CreateSaveMacroErrorIM(string filePath, Exception exc)
        {
            var title = "Save Macro file error";
            var message = $"Could not save macro file:{Environment.NewLine}{filePath}{Environment.NewLine}{exc.Message}";

            var internalMessage = InternalMessageEx.CreateErrorMessage(_imContainer, title, message);

            InternalMessageExHelper.SetInternalMessageAppearance(internalMessage);

            return internalMessage;
        }

        #endregion INTERNAL MESSAGES

        #region MACRO ITEMS MANAGEMENT METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking CaptureKeyMacroItem ButtonEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void CaptureKeyMacroItemButtonExClick(object sender, RoutedEventArgs e)
        {
            KeyReaderInternalMessage? keyReaderIM = null;
            MacroBase? macroItem = null;

            var dataContext = (e.Source as FrameworkElement)?.DataContext;

            if (dataContext is MacroKeyCombination macroKeyCombination)
            {
                keyReaderIM = new KeyReaderInternalMessage(_imContainer, _macroRunner.KeyboardReader, true);
                macroItem = macroKeyCombination;
            }
            else if (dataContext is MacroKeyDown macroKeyDown)
            {
                keyReaderIM = new KeyReaderInternalMessage(_imContainer, _macroRunner.KeyboardReader);
                macroItem = macroKeyDown;
            }
            else if (dataContext is MacroKeyUp macroKeyUp)
            {
                keyReaderIM = new KeyReaderInternalMessage(_imContainer, _macroRunner.KeyboardReader);
                macroItem = macroKeyUp;
            }
            else if (dataContext is MacroKeyClick macroKeyClick)
            {
                keyReaderIM = new KeyReaderInternalMessage(_imContainer, _macroRunner.KeyboardReader);
                macroItem = macroKeyClick;
            }

            if (keyReaderIM != null)
            {
                keyReaderIM.OnClose += (s, e) =>
                {
                    if (e.Result == InternalMessageResult.Ok)
                    {
                        if (macroItem is MacroKeyCombination macroKeyCombination)
                        {
                            if (keyReaderIM.PressedKeys.Any())
                                macroKeyCombination.KeyCodes = new ObservableCollection<byte>(keyReaderIM.PressedKeys);
                        }
                        else if (macroItem is MacroKeyDown macroKeyDown)
                        {
                            if (keyReaderIM.PressedKeys.Any())
                                macroKeyDown.KeyCode = keyReaderIM.PressedKeys.First();
                        }
                        else if (macroItem is MacroKeyUp macroKeyUp)
                        {
                            if (keyReaderIM.PressedKeys.Any())
                                macroKeyUp.KeyCode = keyReaderIM.PressedKeys.First();
                        }
                        else if (macroItem is MacroKeyClick macroKeyClick)
                        {
                            if (keyReaderIM.PressedKeys.Any())
                                macroKeyClick.KeyCode = keyReaderIM.PressedKeys.First();
                        }
                    }
                };

                _imContainer.ShowMessage(keyReaderIM);
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking RemoveMacroItem ButtonEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void RemoveMacroItemButtonExClick(object sender, RoutedEventArgs e)
        {
            var dataContext = (e.Source as FrameworkElement)?.DataContext;

            if (dataContext is MacroBase macroBase)
                _macroRunner.MacroItems.Remove(macroBase);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after dropping dragged MacroItem. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Drag Event Arguments. </param>
        private void MacroItemListViewItemExDrop(object sender, DragEventArgs e)
        {
            _itemsMoveHelper.ItemDrop(sender, e, _macroRunner.MacroItems);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after start dragging MacroItem. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Drag Event Arguments. </param>
        private void MacroItemListViewItemExDragEnter(object sender, DragEventArgs e)
        {
            _itemsMoveHelper.ItemDragEnter(sender, e);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after pressing mouse button on MacroItem. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Mouse Button Event Arguments. </param>
        private void MacroItemHandlerPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement)
            {
                _itemsMoveHelper.ItemPreviewMouseDown(frameworkElement, e);
            }
        }

        //  --------------------------------------------------------------------------------
        // <summary> Method invoked after releasing mouse button on MacroItem. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Mouse Button Event Arguments. </param>
        private void MacroItemHandlerPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement)
            {
                _itemsMoveHelper.ItemPreviewMouseUp(frameworkElement, e);
            }
        }

        //  --------------------------------------------------------------------------------
        // <summary> Method invoked during cursor move over MacroItem. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Mouse Button Event Arguments. </param>
        private void MacroItemHandlerPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement)
            {
                _itemsMoveHelper.MacroItemHandlerPreviewMouseMove(frameworkElement, e);
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after changing selection of MacroItem. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Selection Changed Event Arguments. </param>
        private void MacroItemsListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListViewEx listViewEx && listViewEx.SelectedItem != null)
            {
                listViewEx.SelectedItem = null;
            }
        }

        #endregion MACRO ITEMS MANAGEMENT METHODS

        #region MACRO RUNNER METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after MacroRunner start. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Macro Runner Start Event Arguments. </param>
        private void OnRunnerStart(object? sender, MacroRunnerStartEventArgs e)
        {
            _macroItemsListView.IsEnabled = false;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after MacroRunner stop. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Macro Runner Finished Event Arguments. </param>
        private void OnRunnerEnd(object? sender, MacroRunnerFinishedEventArgs e)
        {
            if (e.Exception != null)
            {
                var macroItemType = e.MacroItem?.GetType()?.Name ?? "unknown";

                var title = "MacroRunner error";
                var message = $"An error occurred while executing macro command {macroItemType} at index {e.MacroItemIndex}:" +
                    $"{Environment.NewLine}{e.Exception.Message}";
                var imError = InternalMessageEx.CreateErrorMessage(_imContainer, title, message);

                InternalMessageExHelper.SetInternalMessageAppearance(imError);

                _imContainer.ShowMessage(imError);
            }

            _macroItemsListView.IsEnabled = true;
        }

        #endregion MACRO RUNNER METHODS

        #region PAGE METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked during unloading page. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion PAGE METHODS

        #region SHORTCUT METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after executing Ctrl+N keyboard shortuct. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Executed Routed Event Arguments.</param>
        internal void OnNewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CreateNewMacro();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after executing Ctrl+O keyboard shortuct. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Executed Routed Event Arguments.</param>
        internal void OnOpenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            OpenMacroFromFile();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after executing Ctrl+S keyboard shortuct. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Executed Routed Event Arguments.</param>
        internal void OnSaveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SaveMacroToFile();
        }

        #endregion SHORTCUT METHODS

    }
}
