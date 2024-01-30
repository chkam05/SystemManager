using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using SystemController.Screens;
using SystemManager.Data.Configuration;
using SystemManager.Windows.Events;

namespace SystemManager.Windows
{
    public partial class ScreenCaptureWindow : Window
    {

        //  DELEGATES

        public delegate void ScreenAreaCaptured(object? sender, ScreenAreaCapturedEventArgs e);


        //  EVENTS

        public ScreenAreaCaptured? OnScreenAreaCaptured;


        //  VARIABLES

        private Border? _captureBorder = null;
        private Point _cursorPosition = new Point();
        private bool _isCapturing = false;


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> ScreenCaptureWindow class constructor. </summary>
        public ScreenCaptureWindow()
        {
            //  Initialize user interface.
            InitializeComponent();
        }

        #endregion CLASS METHODS

        #region COMPONENT METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Create capture border. </summary>
        /// <param name="startPosition"> Capture border start position. </param>
        private void CreateCaptureBorder(Point startPosition)
        {
            if (_captureBorder != null)
            {
                UpdateCaptureBorderSize(startPosition);
                return;
            }

            double width = _captureGrid.ActualWidth - startPosition.X;
            double height = _captureGrid.ActualHeight - startPosition.Y;

            Color accentColor = AppearanceConfig.Instance.AccentColor;
            Color backgroundColor = Color.FromArgb(64, accentColor.R, accentColor.G, accentColor.B);
            SolidColorBrush background = new SolidColorBrush(backgroundColor);

            _captureBorder = new Border
            {
                Background = background,
                BorderBrush = AppearanceConfig.Instance.AccentColorBrush,
                BorderThickness = new Thickness(1),
                Margin = new Thickness(startPosition.X, startPosition.Y, width, height)
            };

            _captureGrid.Children.Add(_captureBorder);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Update capture border size. </summary>
        /// <param name="newSize"> New capture border size. </param>
        private void UpdateCaptureBorderSize(Point newSize)
        {
            if (_captureBorder == null)
                return;

            double width = _captureGrid.ActualWidth - newSize.X;
            double height = _captureGrid.ActualHeight - newSize.Y;

            _captureBorder.Margin = new Thickness(
                _captureBorder.Margin.Left, _captureBorder.Margin.Top, width, height);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Remove capture border. </summary>
        private void RemoveCaptureBorder()
        {
            if (_captureBorder != null)
            {
                if (_captureGrid.Children.Contains(_captureBorder))
                    _captureGrid.Children.Remove(_captureBorder);

                _captureBorder = null;
            }
        }

        #endregion COMPONENT METHODS

        #region INTERACTION METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after pressing mosue button. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Mouse Button Event Arguments. </param>
        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                _isCapturing = true;

                var cursorPosition = e.GetPosition(_captureGrid);

                CreateCaptureBorder(cursorPosition);
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after moving cursor. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Mouse Button Event Arguments. </param>
        private void Grid_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_isCapturing)
            {
                var newCurosrPosition = e.GetPosition(_captureGrid);

                if (IsMouseMoved(newCurosrPosition))
                    UpdateCaptureBorderSize(newCurosrPosition);
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after releasing mosue button. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Mouse Button Event Arguments. </param>
        private void Grid_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Released && _captureBorder != null)
            {
                _isCapturing = false;

                var screenAreaCapturedEventArgs = new ScreenAreaCapturedEventArgs(
                    _captureBorder.Margin.Left,
                    _captureBorder.Margin.Top,
                    _captureBorder.ActualWidth,
                    _captureBorder.ActualHeight);

                Visibility = Visibility.Hidden;

                OnScreenAreaCaptured?.Invoke(this, screenAreaCapturedEventArgs);

                RemoveCaptureBorder();

                Close();
            }
        }

        #endregion INTERACTION METHODS

        #region MOUSE MANAGEMENT METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Check if cursor has been moved. </summary>
        /// <param name="newCursorPosition"> New cursor position. </param>
        /// <returns></returns>
        private bool IsMouseMoved(Point newCursorPosition)
        {
            var isMoved = ((int)_cursorPosition.X != (int)newCursorPosition.X)
                || ((int)_cursorPosition.Y != (int)newCursorPosition.Y);

            if (isMoved)
                _cursorPosition = newCursorPosition;

            return isMoved;
        }

        #endregion MOUSE MANAGEMENT METHODS

        #region SETUP METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Resize window. </summary>
        /// <param name="rect"> New dimensions rect. </param>
        private void Resize(System.Drawing.Rectangle rect)
        {
            Left = rect.X;
            Top = rect.Y;
            Width = rect.Width;
            Height = rect.Height;
        }

        #endregion SETUP METHODS

        #region WINDOW METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after window loaded. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Cancel Event Arguments. </param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //  Get screens informations.
            var screensInfo = ScreenManager.GetAllScreens();
            var area = screensInfo.Select(s => s.OryginalRect).Aggregate(System.Drawing.Rectangle.Union);

            //  Resize window.
            Resize(area);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked when closing the window. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Cancel Event Arguments. </param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //
        }

        #endregion WINDOW METHODS

    }
}
