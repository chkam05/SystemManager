using chkam05.Tools.ControlsEx;
using chkam05.Tools.ControlsEx.Data;
using chkam05.Tools.ControlsEx.InternalMessages;
using System;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SystemController.Screens;
using SystemController.Screens.Data;
using SystemManager.Data.Configuration;
using Color = System.Windows.Media.Color;

namespace SystemManager.InternalMessages
{
    public partial class ScreenSelectInternalMessage : StandardInternalMessageEx
    {

        //  CONST

        private const double SCALING = 0.25d;


        //  VARIABLES

        private ButtonEx? _okButton = null;
        private ScreenInfo[] _screens;

        public ScreenInfo? SelectedScreenInfo { get; private set; }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> ScreenSelectInternalMessage class constructor. </summary>
        /// <param name="parentContainer"> InternalMessagesExContainer. </param>
        public ScreenSelectInternalMessage(InternalMessagesExContainer parentContainer) : base(parentContainer)
        {
            //  Setup data.
            _screens = ScreenManager.GetAllScreens();

            //  Initialize user interface.
            InitializeComponent();

            //  Interface configuration.
            Buttons = new InternalMessageButtons[]
            {
                InternalMessageButtons.OkButton,
                InternalMessageButtons.CancelButton
            };
        }

        #endregion CLASS METHODS

        #region INTERFACE MANAGEMENT METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Update canvas size. </summary>
        private void UpdateCanvasSize()
        {
            double maxX = _canvas.Children.OfType<FrameworkElement>().Max(e => e.Width);
            double maxY = _canvas.Children.OfType<FrameworkElement>().Max(e => e.Height);

            _canvas.MinWidth = maxX;
            _canvas.MinHeight = maxY;
        }

        #endregion INTERFACE MANAGEMENT METHODS

        #region INTERNAL MESSAGE METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after loading InternalMessageEx. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void StandardInternalMessageEx_Loaded(object sender, RoutedEventArgs e)
        {
            ShowScreens();
        }

        #endregion INTERNAL MESSAGE METHODS

        #region SCREENS MANAGEMENT

        //  --------------------------------------------------------------------------------
        /// <summary> Show screens in UI. </summary>
        private void ShowScreens()
        {
            foreach (var screen in _screens)
            {
                var rect = screen.OryginalRect;
                var border = CreateBorder(screen);

                _canvas.Children.Add(border);

                SetBorderPosition(border, rect);
            }

            UpdateCanvasSize();
            CenterScrollViewer();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Center scroll viewer. </summary>
        private void CenterScrollViewer()
        {
            _scorllViewer.ScrollToHorizontalOffset(_canvas.ActualWidth - _scorllViewer.ViewportWidth / 2);
            _scorllViewer.ScrollToVerticalOffset(_canvas.ActualHeight - _scorllViewer.ViewportHeight / 2);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Create screen info Border. </summary>
        /// <param name="screenInfo"> Screen info. </param>
        /// <returns> Border. </returns>
        private Border CreateBorder(ScreenInfo screenInfo)
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
                Child = CreateTextBlock(screenInfo),
                CornerRadius = new CornerRadius(8),
                Cursor = Cursors.Hand,
                Height = rect.Height * SCALING,
                Width = rect.Width * SCALING,
            };

            border.MouseEnter += (sender, e) =>
            {
                var appearance = AppearanceConfig.Instance;
                var mouseOverColor = (appearance.AccentMouseOverBrush as SolidColorBrush)?.Color;

                if (mouseOverColor.HasValue)
                {
                    var backgroundColor = Color.FromArgb(192, accentColor.R, accentColor.G, accentColor.B);
                    var background = new SolidColorBrush(backgroundColor);
                    ((Border)sender).Background = background;
                }
            };

            border.PreviewMouseDown += (sender, e) =>
            {
                var textBlock = ((Border)sender).Child as TextBlock;

                if (textBlock != null)
                {
                    SelectedScreenInfo = _screens.FirstOrDefault(s => s.DeviceName == textBlock.Text);

                    if (_okButton != null)
                        _okButton.IsEnabled = SelectedScreenInfo != null;
                }
            };

            border.MouseLeave += (sender, e) =>
            {
                var appearance = AppearanceConfig.Instance;
                var accentColor = appearance.AccentColor;
                var backgroundColor = Color.FromArgb(128, accentColor.R, accentColor.G, accentColor.B);
                ((Border)sender).Background = background;
            };

            return border;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Create screen info TextBlock. </summary>
        /// <param name="screenInfo"> Screen info. </param>
        /// <returns> TextBlock. </returns>
        private TextBlock CreateTextBlock(ScreenInfo screenInfo)
        {
            return new TextBlock()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                Text = screenInfo.DeviceName,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Set Border position on cavnas. </summary>
        /// <param name="border"> Border. </param>
        /// <param name="rectPosition"> Rect with position. </param>
        private void SetBorderPosition(Border border, Rectangle rectPosition)
        {
            Canvas.SetLeft(border, rectPosition.X * SCALING);
            Canvas.SetTop(border, rectPosition.Y * SCALING);
        }

        #endregion SCREENS MANAGEMENT

        #region TEMPLATE METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> When overridden in a derived class,cis invoked whenever 
        /// application code or internal processes call ApplyTemplate. </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _okButton = GetButtonEx("okButton");

            if (_okButton != null)
            {
                _okButton.Content = "Select";
                _okButton.IsEnabled = false;
            }
        }

        #endregion TEMPLATE METHODS

    }
}
