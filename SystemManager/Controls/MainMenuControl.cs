using chkam05.Tools.ControlsEx;
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
using SystemManager.ViewModels.MainMenu;

namespace SystemManager.Controls
{
    public class MainMenuControl : Control, INotifyPropertyChanged
    {

        //  EVENTS

        public event PropertyChangedEventHandler? PropertyChanged;


        //  DEPENDENCY PROPERITES

        public static readonly DependencyProperty MenuItemsProperty = DependencyProperty.Register(
            nameof(MenuItems),
            typeof(ObservableCollection<MainMenuItemViewModel>),
            typeof(MainMenuControl),
            new PropertyMetadata(new ObservableCollection<MainMenuItemViewModel>()));

        public static readonly DependencyProperty SelectedMenuItemProperty = DependencyProperty.Register(
            nameof(SelectedMenuItem),
            typeof(MainMenuItemViewModel),
            typeof(MainMenuControl),
            new PropertyMetadata(null));


        //  VARIABLES

        private ListViewEx listViewEx;


        //  GETTERS & SETTERS

        public ObservableCollection<MainMenuItemViewModel> MenuItems
        {
            get => (ObservableCollection<MainMenuItemViewModel>) GetValue(MenuItemsProperty);
            set
            {
                SetValue(MenuItemsProperty, value);
                OnPropertyChanged(nameof(MenuItems));
            }
        }

        public MainMenuItemViewModel? SelectedMenuItem
        {
            get => (MainMenuItemViewModel?) GetValue(SelectedMenuItemProperty);
            set
            {
                SetValue(SelectedMenuItemProperty, value);
                OnPropertyChanged(nameof(SelectedMenuItem));
            }
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Static MainMenuControl class constructor. </summary>
        static MainMenuControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MainMenuControl), new FrameworkPropertyMetadata(typeof(MainMenuControl)));
        }

        #endregion CLASS METHODS

        #region INTERACTION METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after changing selection. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Selection Changed Event Arguments. </param>
        private void OnMenuItemSelected(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListViewEx listViewEx)
            {
                if (listViewEx.SelectedItem is MainMenuItemViewModel mainMenuItem)
                {
                    SelectedMenuItem = mainMenuItem;

                    SelectedMenuItem.Action?.Invoke();

                    listViewEx.SelectedItem = null;
                }
            }
        }

        #endregion INTERACTION METHODS

        #region PROPERITES CHANGED METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Triggers the given propertyName to be updated in the UI. </summary>
        /// <param name="propertyName"> Field property name. </param>
        public virtual void OnPropertyChanged(string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion PROPERITES CHANGED METHODS

        #region TEMPLATE METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after call System.Windows.FrameworkElement.ApplyTemplate. </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_ListViewEx") is ListViewEx listViewEx)
            {
                this.listViewEx = listViewEx;
                this.listViewEx.SelectionChanged += OnMenuItemSelected;
            }
        }

        #endregion TEMPLATE METHODS

    }
}
