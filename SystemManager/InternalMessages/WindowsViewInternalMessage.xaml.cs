using chkam05.Tools.ControlsEx;
using chkam05.Tools.ControlsEx.Data;
using chkam05.Tools.ControlsEx.InternalMessages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
using SystemController.MouseKeyboard;
using SystemController.Processes;
using SystemController.Processes.Data;
using SystemManager.Data.Processes;
using SystemManager.ViewModels.Processes;

namespace SystemManager.InternalMessages
{
    public partial class WindowsViewInternalMessage : StandardInternalMessageEx
    {

        //  VARIABLES

        private ProcessesDataContext _processesDataContext;
        private ObservableCollection<WindowInfoViewModel>? _windows;


        //  GETTERS & SETTERS

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


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> WindowsViewInternalMessage class constructor. </summary>
        /// <param name="parentContainer"> InternalMessagesExContainer. </param>
        public WindowsViewInternalMessage(InternalMessagesExContainer parentContainer, List<WindowInfoViewModel> windows,
            ProcessesDataContext processesDataContext)
            : base(parentContainer)
        {
            //  Setup data.
            _processesDataContext = processesDataContext;
            Windows = new ObservableCollection<WindowInfoViewModel>(windows);

            //  Initialize user interface.
            InitializeComponent();

            //  Interface configuration.
            Buttons = new InternalMessageButtons[]
            {
                InternalMessageButtons.CancelButton
            };
        }

        #endregion CLASS METHODS

        #region BUTTONS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking BringToFrontWindow ButtonEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void BringToFrontWindowButtonExClick(object sender, RoutedEventArgs e)
        {
            if (e.Source is FrameworkElement frameworkElement)
            {
                if (frameworkElement.DataContext is WindowInfoViewModel windowInfoViewModel)
                    FocusWindow(windowInfoViewModel);
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking ChildWindows ButtonEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void ChildWindowsButtonExClick(object sender, RoutedEventArgs e)
        {
            if (e.Source is FrameworkElement frameworkElement)
            {
                if (frameworkElement.DataContext is WindowInfoViewModel windowInfoViewModel)
                    ShowChildWindows(windowInfoViewModel);
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking CloseWindow ButtonEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void CloseWindowButtonExClick(object sender, RoutedEventArgs e)
        {
            if (e.Source is FrameworkElement frameworkElement)
            {
                if (frameworkElement.DataContext is WindowInfoViewModel windowInfoViewModel)
                    CloseWindow(windowInfoViewModel);
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking MaximizeWindow ButtonEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void MaximizeWindowButtonExClick(object sender, RoutedEventArgs e)
        {
            if (e.Source is FrameworkElement frameworkElement)
            {
                if (frameworkElement.DataContext is WindowInfoViewModel windowInfoViewModel)
                    MaximizeWindow(windowInfoViewModel);
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking MinimizeWindow ButtonEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void MinimizeWindowButtonExClick(object sender, RoutedEventArgs e)
        {
            if (e.Source is FrameworkElement frameworkElement)
            {
                if (frameworkElement.DataContext is WindowInfoViewModel windowInfoViewModel)
                    MinimizeWindow(windowInfoViewModel);
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking ParentWindow ButtonEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void ParentWindowButtonExClick(object sender, RoutedEventArgs e)
        {
            if (e.Source is FrameworkElement frameworkElement)
            {
                if (frameworkElement.DataContext is WindowInfoViewModel windowInfoViewModel)
                    ShowParentWindows(windowInfoViewModel);
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking RestoreWindow ButtonEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void RestoreWindowButtonExClick(object sender, RoutedEventArgs e)
        {
            if (e.Source is FrameworkElement frameworkElement)
            {
                if (frameworkElement.DataContext is WindowInfoViewModel windowInfoViewModel)
                {
                    
                }
            }
        }

        #endregion BUTTONS METHODS

        #region PROPERITES CHANGED METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Triggers propertyName to be updated in the UI by PressedKeys collection changed. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Notify Collection Changed Event Arguments. </param>
        public virtual void OnWindowsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Windows));
        }

        #endregion PROPERITES CHANGED METHODS

        #region TEMPLATE METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> When overridden in a derived class,cis invoked whenever 
        /// application code or internal processes call ApplyTemplate. </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var cancelButton = GetButtonEx("cancelButton");

            if (cancelButton != null)
                cancelButton.Content = "Close";
        }

        #endregion TEMPLATE METHODS

        #region WINDOWS ITEMS MANAGEMENT METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after changing selection of WindowItem. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Selection Changed Event Arguments. </param>
        private void WindowsListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListViewEx listViewEx && listViewEx.SelectedItem != null)
            {
                listViewEx.SelectedItem = null;
            }
        }

        #endregion WINDOWS ITEMS MANAGEMENT METHODS

        #region WINDOWS MANAGEMETN METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Close window. </summary>
        /// <param name="windowInfoViewModel"> Window information view model. </param>
        private void CloseWindow(WindowInfoViewModel windowInfoViewModel)
        {
            var windowInfo = windowInfoViewModel.WindowInfo;
            var actionResult = _processesDataContext.ProcessManager.CloseWindow(windowInfo);
            UpdateWindowsList(actionResult, windowInfoViewModel);

            Windows.Remove(windowInfoViewModel);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Focus window (Bring window to front). </summary>
        /// <param name="windowInfoViewModel"> Window information view model. </param>
        private void FocusWindow(WindowInfoViewModel windowInfoViewModel)
        {
            var windowInfo = windowInfoViewModel.WindowInfo;
            var actionResult = _processesDataContext.ProcessManager.FocusWindow(windowInfo);
            UpdateWindowsList(actionResult, windowInfoViewModel);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Show child window. </summary>
        /// <param name="windowInfoViewModel"> Window information view model. </param>
        private void ShowChildWindows(WindowInfoViewModel windowInfoViewModel)
        {
            if (windowInfoViewModel.HasChildWindows)
            {
                var imWindows = new WindowsViewInternalMessage(
                    _parentContainer,
                    windowInfoViewModel.ChildWindows.ToList(),
                    _processesDataContext);

                _parentContainer.ShowMessage(imWindows);
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Show parent window. </summary>
        /// <param name="windowInfoViewModel"> Window information view model. </param>
        private void ShowParentWindows(WindowInfoViewModel windowInfoViewModel)
        {
            if (windowInfoViewModel.ParentWindow != null)
            {
                var imWindows = new WindowsViewInternalMessage(
                    _parentContainer,
                    new List<WindowInfoViewModel> { windowInfoViewModel.ParentWindow },
                    _processesDataContext);

                _parentContainer.ShowMessage(imWindows);
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Maximize window. </summary>
        /// <param name="windowInfoViewModel"> Window information view model. </param>
        private void MaximizeWindow(WindowInfoViewModel windowInfoViewModel)
        {
            var windowInfo = windowInfoViewModel.WindowInfo;
            var actionResult = _processesDataContext.ProcessManager.MaximizeWindow(windowInfo);
            UpdateWindowsList(actionResult, windowInfoViewModel);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Minimize window. </summary>
        /// <param name="windowInfoViewModel"> Window information view model. </param>
        private void MinimizeWindow(WindowInfoViewModel windowInfoViewModel)
        {
            var windowInfo = windowInfoViewModel.WindowInfo;
            var actionResult = _processesDataContext.ProcessManager.MinimizeWindow(windowInfo);
            UpdateWindowsList(actionResult, windowInfoViewModel);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Restore window. </summary>
        /// <param name="windowInfoViewModel"> Window information view model. </param>
        private void RestoreWindow(WindowInfoViewModel windowInfoViewModel)
        {
            var windowInfo = windowInfoViewModel.WindowInfo;
            var actionResult = _processesDataContext.ProcessManager.RestoreWindow(windowInfo);
            UpdateWindowsList(actionResult, windowInfoViewModel);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Update windows list based on action response. </summary>
        /// <param name="windowActionResult"> Window action result. </param>
        /// <param name="windowInfoViewModel"> Window information view model. </param>
        private void UpdateWindowsList(WindowActionResult windowActionResult, WindowInfoViewModel windowInfoViewModel)
        {
            switch (windowActionResult)
            {
                case WindowActionResult.Success:
                    return;

                case WindowActionResult.WindowNotExist:
                case WindowActionResult.ProcessNotExist:
                case WindowActionResult.UnknownError:
                    Windows.Remove(windowInfoViewModel);
                    return;
            }
        }

        #endregion WINDOWS MANAGEMETN METHODS

    }
}
