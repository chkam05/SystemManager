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

        public WindowInfo WindowInfo { get; private set; }


        //  GETTERS & SETTERS

        public ObservableCollection<WindowInfoViewModel> ChildWindows
        {
            get => _childWindows;
            set
            {
                _childWindows = value;
                _childWindows.CollectionChanged += OnChildWindowsCollectionChanged;
                OnPropertyChanged(nameof(ChildWindows));
                OnPropertyChanged(nameof(HasChildWindows));
            }
        }

        public WindowInfoViewModel? ParentWindow
        {
            get => _parentWindow;
            set
            {
                _parentWindow = value;
                OnPropertyChanged(nameof(ParentWindow));
                OnPropertyChanged(nameof(HasParentWindow));
            }
        }

        public bool HasChildWindows
        {
            get => _childWindows?.Any() ?? false;
        }

        public bool HasParentWindow
        {
            get => _parentWindow != null;
        }

        public string? ClassName
        {
            get => WindowInfo.ClassName;
            set
            {
                WindowInfo.ClassName = value;
                OnPropertyChanged(nameof(ClassName));
            }
        }

        public WindowAttributes Attributes
        {
            get => WindowInfo.Attributes;
            set
            {
                WindowInfo.Attributes = value;
                OnPropertyChanged(nameof(Attributes));
            }
        }

        public int PositionX
        {
            get => WindowInfo.Position.X;
            set
            {
                WindowInfo.Position = new POINT()
                {
                    X = value,
                    Y = WindowInfo.Position.Y
                };
                OnPropertyChanged(nameof(PositionX));
            }
        }

        public int PositionY
        {
            get => WindowInfo.Position.Y;
            set
            {
                WindowInfo.Position = new POINT()
                {
                    X = WindowInfo.Position.X,
                    Y = value
                };
                OnPropertyChanged(nameof(PositionY));
            }
        }

        public int Height
        {
            get => WindowInfo.Size.Height;
            set
            {
                WindowInfo.Size = new SIZE()
                {
                    Height = value,
                    Width = WindowInfo.Size.Width,
                };
                OnPropertyChanged(nameof(Height));
            }
        }

        public int Width
        {
            get => WindowInfo.Size.Width;
            set
            {
                WindowInfo.Size = new SIZE()
                {
                    Height = WindowInfo.Size.Width,
                    Width = value
                };
                OnPropertyChanged(nameof(Width));
            }
        }

        public WindowRole Role
        {
            get => WindowInfo.Role;
            set
            {
                WindowInfo.Role = value;
                OnPropertyChanged(nameof(Role));
            }
        }

        public SIZE Size
        {
            get => WindowInfo.Size;
            set
            {
                WindowInfo.Size = value;
                OnPropertyChanged(nameof(Size));
            }
        }

        public WindowState State
        {
            get => WindowInfo.State;
            set
            {
                WindowInfo.State = value;
                OnPropertyChanged(nameof(State));
            }
        }

        public string? Title
        {
            get => WindowInfo.Title;
            set
            {
                WindowInfo.Title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public int Transparency
        {
            get => WindowInfo.Transparency;
            set
            {
                WindowInfo.Transparency = value;
                OnPropertyChanged(nameof(Transparency));
            }
        }

        public bool Visible
        {
            get => WindowInfo.Visible;
            set
            {
                WindowInfo.Visible = value;
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
            WindowInfo = windowInfo;

            if (windowInfo.ChildWindows?.Any() ?? false)
                ChildWindows = new ObservableCollection<WindowInfoViewModel>(
                    windowInfo.ChildWindows.Select(w => new WindowInfoViewModel(w)));

            ParentWindow = windowInfo.ParentWindow != null
                ? new WindowInfoViewModel(windowInfo.ParentWindow)
                : null;

            OnPropertyChanged(nameof(ChildWindows));
            OnPropertyChanged(nameof(HasChildWindows));
            OnPropertyChanged(nameof(ParentWindow));
            OnPropertyChanged(nameof(HasParentWindow));

            OnPropertyChanged(nameof(ClassName));
            OnPropertyChanged(nameof(Attributes));
            OnPropertyChanged(nameof(PositionX));
            OnPropertyChanged(nameof(PositionY));
            OnPropertyChanged(nameof(Width));
            OnPropertyChanged(nameof(Height));
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
