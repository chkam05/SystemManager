using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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

            OnWindowInfoPropertyUpdate();
        }

        #endregion CLASS METHODS

        #region COMPARATION METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Determines whether the specified object is equal to the current object. </summary>
        /// <param name="obj"> Object to compare. </param>
        /// <returns> True - object is equal to the current object; False - otherwise. </returns>
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (obj is WindowInfoViewModel windowInfoViewModel)
                return windowInfoViewModel.GetHashCode() == GetHashCode();

            return false;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Serves as the default hash function. </summary>
        /// <returns> A hash code for the current object. </returns>
        public override int GetHashCode()
        {
            return WindowInfo.GetHashCode();
        }

        #endregion COMPARATION METHODS

        #region PROPERITES CHANGED METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after ChildWindows collection changed. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Notify Collection Changed Event Arguments. </param>
        private void OnChildWindowsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ChildWindows));
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Triggers properies, related with WindowInfo to be updated in the UI. </summary>
        private void OnWindowInfoPropertyUpdate()
        {
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

        #endregion PROPERITES CHANGED METHODS

        #region UPDATE METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Update window information. </summary>
        /// <param name="windowInfo"> Window information. </param>
        /// <param name="allowParentUpdate"> Allow parent window update. </param>
        /// <param name="allowChildUpdate"> allow child window update. </param>
        public void Update(WindowInfo windowInfo, bool allowParentUpdate = true, bool allowChildUpdate = true)
        {
            WindowInfo = windowInfo;

            if (allowParentUpdate)
                UpdateParentWindow(windowInfo.ParentWindow);

            if (allowChildUpdate)
                UpdateChildWindows(windowInfo.ChildWindows);

            OnWindowInfoPropertyUpdate();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Update child windows information. </summary>
        /// <param name="childWindowsInfo"> Child windows information collection. </param>
        private void UpdateChildWindows(IEnumerable<WindowInfo>? childWindowsInfo)
        {
            if (childWindowsInfo == null || !childWindowsInfo.Any())
            {
                ChildWindows = new ObservableCollection<WindowInfoViewModel>();
                return;
            }

            if (!ChildWindows.Any() && (childWindowsInfo?.Any() ?? false))
            {
                ChildWindows = new ObservableCollection<WindowInfoViewModel>(childWindowsInfo.Select(w => new WindowInfoViewModel(w)));
                return;
            }

            var bothWindows = childWindowsInfo?.Where(w => ChildWindows.Any(cw => cw.WindowInfo.Handle == w.Handle)) ?? new List<WindowInfo>();
            var newWindows = childWindowsInfo?.Where(w => !bothWindows.Any(cw => cw.Handle == w.Handle)) ?? new List<WindowInfo>();
            var oldWindows = ChildWindows.Where(cw => !bothWindows.Any(w => w.Handle == cw.WindowInfo.Handle));

            if (oldWindows.Any())
                foreach (var window in oldWindows)
                    ChildWindows.Remove(window);

            if (bothWindows.Any())
                foreach (var window in bothWindows)
                {
                    var currentWindow = ChildWindows.First(w => w.WindowInfo.Handle == window.Handle);
                    currentWindow.Update(window, allowParentUpdate: false);
                }

            if (newWindows.Any())
                foreach (var window in newWindows)
                    ChildWindows.Add(new WindowInfoViewModel(window));
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Update parent window information. </summary>
        /// <param name="parentWindowInfo"> Parent window information. </param>
        private void UpdateParentWindow(WindowInfo? parentWindowInfo)
        {
            if (parentWindowInfo == null)
            {
                ParentWindow = null;
                return;
            }

            if (ParentWindow == null)
            {
                ParentWindow = new WindowInfoViewModel(parentWindowInfo);
                return;
            }

            ParentWindow.Update(parentWindowInfo, allowChildUpdate: false);
        }

        #endregion UPDATE METHODS

    }
}
