using chkam05.Tools.ControlsEx.InternalMessages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Reflection;
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
using SystemController.Screens.Data;
using SystemManager.Data.Configuration;
using SystemManager.Data.Screens;
using SystemManager.ViewModels.Screens;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace SystemManager.Pages
{
    public partial class ScreensPage : Page
    {

        //  CONST

        private const double SCALING = 0.10d;


        //  VARIABLES

        private ScreensDataContext _dataContext;
        private InternalMessagesExContainer _imContainer;
        private List<Border> _screensBorders;


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> ScreensPage class constructor. </summary>
        /// <param name="imContainer"> InternalMessageExContainer. </param>
        public ScreensPage(InternalMessagesExContainer imContainer)
        {
            //  Setup components.
            _dataContext = new ScreensDataContext();
            _dataContext.PropertyChanged += DataContextPropertyUpdate;
            _imContainer = imContainer;
            _screensBorders = new List<Border>();

            //  Initialize user interface.
            InitializeComponent();

            //  Set data context.
            DataContext = _dataContext;
        }

        #endregion CLASS METHODS

        #region COMPONENTS MANAGEMENT METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Create screens representation. </summary>
        private void CreateVisualScreensRepresentation()
        {
            if (_screensBorders.Any())
                ClearVisualScreensRepresentation();

            foreach (var screenInfoViewModel in _dataContext.ScreensCollection)
            {
                var screenBorder = CreateScreenBorder(screenInfoViewModel.ScreenInfo);
                var rect = screenInfoViewModel.ScreenInfo.OryginalRect;

                _screensBorders.Add(screenBorder);
                _screensCanvas.Children.Add(screenBorder);

                Canvas.SetLeft(screenBorder, rect.X * SCALING);
                Canvas.SetTop(screenBorder, rect.Y * SCALING);
            }

            _screensScrollVeiwer.ScrollToHorizontalOffset(_screensCanvas.ActualWidth - _screensScrollVeiwer.ViewportWidth / 2);
            _screensScrollVeiwer.ScrollToVerticalOffset(_screensCanvas.ActualHeight - _screensScrollVeiwer.ViewportHeight / 2);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Create border that represents screen. </summary>
        /// <param name="screenInfo"> Screen info. </param>
        /// <returns> Border that represents screen. </returns>
        private Border CreateScreenBorder(ScreenInfo screenInfo)
        {
            var appearance = AppearanceConfig.Instance;
            var accentColor = appearance.AccentColor;
            var backgroundColor = Color.FromArgb(128, accentColor.R, accentColor.G, accentColor.B);
            var background = new SolidColorBrush(backgroundColor);
            var rect = screenInfo.OryginalRect;

            var border = new Border
            {
                Background = background,
                BorderBrush = appearance.AccentColorBrush,
                BorderThickness = new Thickness(1),
                Child = CreateScreenText(screenInfo),
                CornerRadius = new CornerRadius(8),
                Cursor = Cursors.Hand,
                Height = rect.Height * SCALING,
                Width = rect.Width * SCALING,
            };

            border.MouseEnter += ScreenBorderMouseEnter;
            border.MouseLeave += ScreenBorderMouseLeave;
            border.PreviewMouseDown += ScreenBorderPreviewMoudeDown;

            return border;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after cursor entered screen border. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Mouse Evnet Arguments. </param>
        private void ScreenBorderMouseEnter(object? sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                var appearance = AppearanceConfig.Instance;
                var accentColor = appearance.AccentColor;
                var mouseOverColor = (appearance.AccentMouseOverBrush as SolidColorBrush)?.Color;

                if (mouseOverColor.HasValue)
                {
                    var backgroundColor = Color.FromArgb(192, accentColor.R, accentColor.G, accentColor.B);
                    var background = new SolidColorBrush(backgroundColor);

                    border.Background = background;
                }
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after cursor leave screen border. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Mouse Evnet Arguments. </param>
        private void ScreenBorderMouseLeave(object? sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                var appearance = AppearanceConfig.Instance;
                var accentColor = appearance.AccentColor;
                var backgroundColor = Color.FromArgb(128, accentColor.R, accentColor.G, accentColor.B);
                var background = new SolidColorBrush(backgroundColor);

                border.Background = background;
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after pressing mouse button inside screen border. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Mouse Button Event Arguments. </param>
        private void ScreenBorderPreviewMoudeDown(object? sender, MouseButtonEventArgs e)
        {
            if (sender is Border border)
            {
                var textBlock = border.Child as TextBlock;

                if (textBlock != null)
                    _dataContext.SelectScreen(textBlock.Text);
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Create text block with screen name. </summary>
        /// <param name="screenInfo"> Screen info. </param>
        /// <returns> TextBlock with screen name. </returns>
        private TextBlock CreateScreenText(ScreenInfo screenInfo)
        {
            return new TextBlock()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                Text = screenInfo.DeviceName,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Clear screens representation. </summary>
        private void ClearVisualScreensRepresentation()
        {
            foreach (var screenBorder in _screensBorders)
            {
                if (_screensCanvas.Children.Contains(screenBorder))
                    _screensCanvas.Children.Remove(screenBorder);
            }

            _screensBorders.Clear();
        }

        #endregion COMPONENTS MANAGEMENT METHODS

        #region HEADER BUTTONS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking Refresh button. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void RefreshButtonExClick(object sender, RoutedEventArgs e)
        {
            _dataContext.ReloadScreens();
        }

        #endregion HEADER BUTTONS METHODS

        #region PAGE METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after loading page. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CreateVisualScreensRepresentation();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after unloading page. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            //
        }

        #endregion PAGE METHODS

        #region PROPERITES CHANGED METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after updating property in DataContext. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Property Changed Event Arguments. </param>
        private void DataContextPropertyUpdate(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ScreensDataContext.ScreensCollection))
            {
                CreateVisualScreensRepresentation();
            }
        }

        #endregion PROPERITES CHANGED METHODS

    }
}
