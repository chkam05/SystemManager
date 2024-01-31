using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemController.Screens;
using SystemController.Screens.Data;
using SystemManager.ViewModels.Base;
using SystemManager.ViewModels.Screens;

namespace SystemManager.Data.Screens
{
    public class ScreensDataContext : BaseViewModel
    {

        //  VARIABLES

        private ObservableCollection<ScreenInfoViewModel> _screensCollection = new ObservableCollection<ScreenInfoViewModel>();
        private ScreenInfoViewModel _selectedScreen;


        //  GETTERS & SETTERS

        public ObservableCollection<ScreenInfoViewModel> ScreensCollection
        {
            get => _screensCollection;
            set
            {
                UpdateProperty(ref _screensCollection, value);
                _screensCollection.CollectionChanged += ScreensCollectionChanged;
            }
        }

        public ScreenInfoViewModel SelectedScreen
        {
            get => _selectedScreen;
            set => UpdateProperty(ref _selectedScreen, value);
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> ScreensDataContext class constructor. </summary>
        public ScreensDataContext()
        {
            ReloadScreens();
        }

        #endregion CLASS METHODS

        #region PROPERITES CHANGED METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after Screens collection changed. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Notify Collection Changed Event Arguments. </param>
        private void ScreensCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ScreensCollection));
        }

        #endregion PROPERITES CHANGED METHODS

        #region SCREENS MANAGEMENT METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Reload screens. </summary>
        public void ReloadScreens()
        {
            ScreensCollection = new ObservableCollection<ScreenInfoViewModel>(
                ScreenManager.GetAllScreens().Select(s => new ScreenInfoViewModel(s)));
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Select screen. </summary>
        /// <param name="deviceName"> Screen device name. </param>
        public void SelectScreen(string deviceName)
        {
            var screen = ScreensCollection.FirstOrDefault(s => s.DeviceName == deviceName);

            if (screen != null)
                SelectedScreen = screen;
        }

        #endregion SCREENS MANAGEMENT METHODS

    }
}
