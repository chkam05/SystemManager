using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SystemManager.ViewModels.Base;

namespace SystemManager.Data.Screenshots
{
    public class ScreenshotDataContext : BaseViewModel
    {

        //  VARIABLES

        private ImageSource _capturedScreen;


        //  GETTERS & SETTERS

        public ImageSource CapturedScreen
        {
            get => _capturedScreen;
            set => UpdateProperty(ref _capturedScreen, value);
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> ScreenshotDataContext class constructor. </summary>
        public ScreenshotDataContext()
        {
            //
        }

        #endregion CLASS METHODS

        #region UPDATE METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Set image source. </summary>
        public void SetImageSource(BitmapImage bitmapImage)
        {
            CapturedScreen = bitmapImage;
        }

        #endregion UPDATE METHODS

    }
}
