using chkam05.Tools.ControlsEx.InternalMessages;
using chkam05.Tools.ControlsEx.WindowsEx;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SystemManager.Controls;
using SystemManager.Pages;
using SystemManager.Utilities;
using SystemManager.ViewModels.MainMenu;

namespace SystemManager.Windows
{
    public partial class MainWindow : WindowEx, INotifyPropertyChanged
    {

        //  VARIABLES

        private ObservableCollection<MainMenuItemViewModel> _mainMenuItems;


        //  GETTERS & SETTERS

        public ObservableCollection<MainMenuItemViewModel> MainMenuItems
        {
            get => _mainMenuItems;
            set
            {
                UpdateProperty(ref _mainMenuItems, value);
                _mainMenuItems.CollectionChanged += (s, e) => OnPropertyChanged(nameof(MainMenuItems));
            }
        }

        public InternalMessagesExContainer IMContainer
        {
            get => _internalMessagesExContainer;
        }

        public PagesControl PagesControl
        {
            get => _pagesControl;
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> MainWindow class constructor. </summary>
        public MainWindow()
        {
            //  Initialize data.
            SetupMainMenu();

            //  Initialize user interface.
            InitializeComponent();
        }

        #endregion CLASS METHODS

        #region MAIN MENU INTERACTION METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Close application main menu item. </summary>
        private void CloseApplicationMainMenuItemSelect()
        {
            Application.Current.Shutdown();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Open macros page main menu item. </summary>
        private void OpenMacrosPageMainMenuItemSelect()
        {
            PagesControl.LoadPageOrGetLast(new MacrosPage(IMContainer));
        }

        #endregion MAIN MENU INTERACTION METHODS

        #region PROPERITES CHANGED METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Triggers the given propertyName to be updated in the UI, and set ref field to new value. </summary>
        /// <typeparam name="T"> Field and value type. </typeparam>
        /// <param name="field"> Field. </param>
        /// <param name="newValue"> New value to set in field. </param>
        /// <param name="propertyName"> Field property name. </param>
        public virtual void UpdateProperty<T>(ref T field, T newValue, [CallerMemberName] string? propertyName = null)
        {
            field = newValue;
            OnPropertyChanged(propertyName);
        }

        #endregion PROPERITES CHANGED METHODS

        #region SETUP METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Setup main menu items. </summary>
        private void SetupMainMenu()
        {
            var mainMenuItems = new List<MainMenuItemViewModel>()
            {
                new MainMenuItemViewModel("Macros", "Scripts for automated management", PackIconKind.Code, OpenMacrosPageMainMenuItemSelect),
                new MainMenuItemViewModel("Close", "Shut down application", PackIconKind.Power, CloseApplicationMainMenuItemSelect)
            };

            MainMenuItems = new ObservableCollection<MainMenuItemViewModel>(mainMenuItems);
        }

        #endregion SETUP METHODS

        #region WINDOW METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after loading window. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            //  Load Macros Page
            OpenMacrosPageMainMenuItemSelect();

            //  Load Welcome Message
            var title = $"Welcome in SystemManager";
            var message = $"SystemManager v1.0 for Windows{Environment.NewLine}by Kamil Karpiński";
            var imWelcome = InternalMessageEx.CreateInfoMessage(IMContainer, title, message);

            InternalMessageExHelper.SetInternalMessageAppearance(imWelcome);

            IMContainer.ShowMessage(imWelcome);
        }

        #endregion WINDOW METHODS

    }
}
