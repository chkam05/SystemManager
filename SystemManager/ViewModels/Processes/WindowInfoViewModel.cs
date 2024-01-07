using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemController.Data;
using SystemController.ProcessesManagement.Data;
using SystemManager.ViewModels.Base;

namespace SystemManager.ViewModels.Processes
{
    public class WindowInfoViewModel : BaseViewModel
    {

        //  VARIABLES

        private ObservableCollection<WindowInfoViewModel> _childWindows;
        private WindowInfoViewModel? _parentWindow;
        private WindowInfo _windowInfo;


        //  GETTERS & SETTERS

        public ObservableCollection<WindowInfoViewModel> ChildWindows
        {
            get => _childWindows;
            set
            {
                _childWindows = value;
                _childWindows.CollectionChanged += OnChildWindowsCollectionChanged;
                OnPropertyChanged(nameof(ChildWindows));
            }
        }

        public WindowInfoViewModel? ParentWindow
        {
            get => _parentWindow;
            set
            {
                _parentWindow = value;
                OnPropertyChanged(nameof(ParentWindow));
            }
        }

        public string? ClassName
        {
            get => _windowInfo.ClassName;
            set
            {
                _windowInfo.ClassName = value;
                OnPropertyChanged(nameof(ClassName));
            }
        }

        public WindowAttributes Attributes
        {
            get => _windowInfo.Attributes;
            set
            {
                _windowInfo.Attributes = value;
                OnPropertyChanged(nameof(Attributes));
            }
        }

        public POINT Position
        {
            get => _windowInfo.Position;
            set
            {
                _windowInfo.Position = value;
                OnPropertyChanged(nameof(Position));
            }
        }

        public WindowRole Role
        {
            get => _windowInfo.Role;
            set
            {
                _windowInfo.Role = value;
                OnPropertyChanged(nameof(Role));
            }
        }

        public SIZE Size
        {
            get => _windowInfo.Size;
            set
            {
                _windowInfo.Size = value;
                OnPropertyChanged(nameof(Size));
            }
        }

        public WindowState State
        {
            get => _windowInfo.State;
            set
            {
                _windowInfo.State = value;
                OnPropertyChanged(nameof(State));
            }
        }

        public string? Title
        {
            get => _windowInfo.Title;
            set
            {
                _windowInfo.Title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public int Transparency
        {
            get => _windowInfo.Transparency;
            set
            {
                _windowInfo.Transparency = value;
                OnPropertyChanged(nameof(Transparency));
            }
        }

        public bool Visible
        {
            get => _windowInfo.Visible;
            set
            {
                _windowInfo.Visible = value;
                OnPropertyChanged(nameof(Visible));
            }
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> WindowInfoViewModel class constructor. </summary>
        /// <param name="windowInfo"> Window information. </param>
        public WindowInfoViewModel(WindowInfo windowInfo)
        {
            _childWindows = new ObservableCollection<WindowInfoViewModel>();
            _windowInfo = windowInfo;

            if (windowInfo.ChildWindows?.Any() ?? false)
                ChildWindows = new ObservableCollection<WindowInfoViewModel>(
                    windowInfo.ChildWindows.Select(w => new WindowInfoViewModel(w)));

            ParentWindow = windowInfo.ParentWindow != null
                ? new WindowInfoViewModel(windowInfo.ParentWindow)
                : null;

            OnPropertyChanged(nameof(ClassName));
            OnPropertyChanged(nameof(Attributes));
            OnPropertyChanged(nameof(Position));
            OnPropertyChanged(nameof(Role));
            OnPropertyChanged(nameof(Size));
            OnPropertyChanged(nameof(State));
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Transparency));
            OnPropertyChanged(nameof(Visible));
        }

        #endregion CLASS METHODS

        #region PROPERITES CHANGED METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after ChildWindows collection changed. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Notify Collection Changed Event Arguments. </param>
        private void OnChildWindowsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ChildWindows));
        }

        #endregion PROPERITES CHANGED METHODS

    }
}
