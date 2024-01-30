using chkam05.Tools.ControlsEx.Data;
using chkam05.Tools.ControlsEx.InternalMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using SystemController.Screens;
using SystemManager.Data.Screenshots;
using SystemManager.InternalMessages;
using SystemManager.Windows;

namespace SystemManager.Pages
{
    public partial class ScreenshotPage : Page
    {

        //  VARIABLES

        private ScreenshotDataContext _dataContext;
        private InternalMessagesExContainer _imContainer;


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> ScreenshotPage class constructor. </summary>
        /// <param name="imContainer"> InternalMessageExContainer. </param>
        public ScreenshotPage(InternalMessagesExContainer imContainer)
        {
            //  Setup components.
            _dataContext = new ScreenshotDataContext();
            _imContainer = imContainer;

            //  Initialize user interface.
            InitializeComponent();

            //  Setup data context.
            DataContext = _dataContext;
        }

        #endregion CLASS METHODS

        #region HEADER BUTTONS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking AllScreensCapture button. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void AllScrensCaptureButtonExClick(object sender, RoutedEventArgs e)
        {
            var screenBitmapImage = ScreenshotManager.CaptureAllScreensAsBitmapImage();
            _dataContext.SetImageSource(screenBitmapImage);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking AreaCapture button. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void AreaCaptureButtonExClick(object sender, RoutedEventArgs e)
        {
            var captureWindow = new ScreenCaptureWindow();

            captureWindow.OnScreenAreaCaptured += (s, e) =>
            {
                var selectedRegions = ScreenManager.GetSelectedRegions(e.Rectangle);

                if (selectedRegions.Any())
                {
                    var mappedRegion = selectedRegions.Select(r => r.Key.MapToOryginalSize(r.Value))
                        .Aggregate(System.Drawing.Rectangle.Union);

                    var screenBitmapImage = ScreenshotManager.CaptureAreaAsBitmapImage(e.Rectangle);
                    _dataContext.SetImageSource(screenBitmapImage);
                }
                else
                {
                    var screenBitmapImage = ScreenshotManager.CaptureAreaAsBitmapImage(e.Rectangle);
                    _dataContext.SetImageSource(screenBitmapImage);
                }
            };

            captureWindow.Show();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking MainScreenCapture button. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void MainScreenCaptureButtonExClick(object sender, RoutedEventArgs e)
        {
            var screenBitmapImage = ScreenshotManager.CaptureMainScreenAsBitmapImage();
            _dataContext.SetImageSource(screenBitmapImage);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked after clicking ScreenCapture button. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void ScreenCaptureButtonExClick(object sender, RoutedEventArgs e)
        {
            var screenSelectIM = new ScreenSelectInternalMessage(_imContainer);

            screenSelectIM.OnClose += (s, e) =>
            {
                if (e.Result == InternalMessageResult.Ok)
                {
                    var selectedScreen = (s as ScreenSelectInternalMessage)?.SelectedScreenInfo;

                    if (selectedScreen != null)
                    {
                        var screenBitmapImage = ScreenshotManager.CaptureScreenAsBitmapImage(selectedScreen);
                        _dataContext.SetImageSource(screenBitmapImage);
                    }
                }
            };

            _imContainer.ShowMessage(screenSelectIM);
        }

        #endregion HEADER BUTTONS METHODS

        #region PAGE METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked during loading page. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Method invoked during unloading page. </summary>
        /// <param name="sender"> Object that invoked the method. </param>
        /// <param name="e"> Routed Event Arguments. </param>
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            //
        }

        #endregion PAGE METHODS

    }
}
