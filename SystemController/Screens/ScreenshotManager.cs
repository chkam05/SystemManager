using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using SystemController.Screens.Data;

namespace SystemController.Screens
{
    public static class ScreenshotManager
    {

        //  METHODS

        #region CAPTURE SCREENSHOT FROM ALL SCREENS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Capture screenshot from all screens. </summary>
        /// <returns> Screenshot bitmap. </returns>
        public static Bitmap CaptureAllScreens()
        {
            Rectangle screensRect = GetAllScreensRect();

            return CaptureScreen(screensRect);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Capture screenshot from all screens as bitmap image. </summary>
        /// <returns> Screenshot bitmap image. </returns>
        public static BitmapImage CaptureAllScreensAsBitmapImage()
        {
            var bitmap = CaptureAllScreens();
            return ConvertBitmapToBitmapImage(bitmap);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Capture screenshot from all screens to clipboard. </summary>
        public static void CaptureAllScreensToClipboard()
        {
            var bitmap = CaptureAllScreens();
            CopyBitmapToClipboard(bitmap);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Capture screenshot from all screens and save to file. </summary>
        /// <param name="filePath"> Save file path. </param>
        /// <param name="imageFormat"> Image format (default PNG). </param>
        public static void CaptureAllScreensToFile(string filePath, ImageFormat? imageFormat = null)
        {
            var bitmap = CaptureAllScreens();
            SaveBitmapToFile(bitmap, filePath, imageFormat);
        }

        #endregion CAPTURE SCREENSHOT FROM ALL SCREENS METHODS

        #region CAPTURE SCREENSHOT FROM AREA METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Capture screenshot from specific area. </summary>
        /// <param name="selectedArea"> Selected area as rect. </param>
        /// <returns> Screenshot bitmap. </returns>
        public static Bitmap CaptureArea(Rectangle selectedArea)
        {
            return CaptureArea(selectedArea.X, selectedArea.Y, selectedArea.Width, selectedArea.Height);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Capture screenshot from specific area. </summary>
        /// <param name="x"> Screen X point. </param>
        /// <param name="y"> Screen Y point. </param>
        /// <param name="width"> Screenshot width. </param>
        /// <param name="height"> Screenshot height. </param>
        /// <returns> Screenshot bitmap. </returns>
        public static Bitmap CaptureArea(int x, int y, int width, int height)
        {
            Rectangle screensRect = GetAllScreensRect();
            Rectangle clampedRect = ClampScreenshotArea(x, y, width, height, screensRect);

            return CaptureScreen(clampedRect);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Capture screenshot from specific area as bitmap image. </summary>
        /// <param name="selectedArea"> Selected area as rect. </param>
        /// <returns> Screenshot bitmap image. </returns>
        public static BitmapImage CaptureAreaAsBitmapImage(Rectangle selectedArea)
        {
            return CaptureAreaAsBitmapImage(selectedArea.X, selectedArea.Y, selectedArea.Width, selectedArea.Height);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Capture screenshot from specific area as bitmap image. </summary>
        /// <param name="x"> Screen X point. </param>
        /// <param name="y"> Screen Y point. </param>
        /// <param name="width"> Screenshot width. </param>
        /// <param name="height"> Screenshot height. </param>
        /// <returns> Screenshot bitmap image. </returns>
        public static BitmapImage CaptureAreaAsBitmapImage(int x, int y, int width, int height)
        {
            var bitmap = CaptureArea(x, y, width, height);
            return ConvertBitmapToBitmapImage(bitmap);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Capture screenshot from specific area to clipboard. </summary>
        /// <param name="selectedArea"> Selected area as rect. </param>
        public static void CaptureAreaToClipboard(Rectangle selectedArea)
        {
            CaptureAreaToClipboard(selectedArea.X, selectedArea.Y, selectedArea.Width, selectedArea.Height);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Capture screenshot from specific area to clipboard. </summary>
        /// <param name="x"> Screen X point. </param>
        /// <param name="y"> Screen Y point. </param>
        /// <param name="width"> Screenshot width. </param>
        /// <param name="height"> Screenshot height. </param>
        public static void CaptureAreaToClipboard(int x, int y, int width, int height)
        {
            var bitmap = CaptureArea(x, y, width, height);
            CopyBitmapToClipboard(bitmap);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Capture screenshot from specific area and save to file. </summary>
        /// <param name="selectedArea"> Selected area as rect. </param>
        /// <param name="filePath"> Save file path. </param>
        /// <param name="imageFormat"> Image format (default PNG). </param>
        public static void CaptureAreaToFile(Rectangle selectedArea, string filePath, ImageFormat? imageFormat = null)
        {
            CaptureAreaToFile(selectedArea.X, selectedArea.Y, selectedArea.Width, selectedArea.Height,
                filePath, imageFormat);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Capture screenshot from specific area and save to file. </summary>
        /// <param name="x"> Screen X point. </param>
        /// <param name="y"> Screen Y point. </param>
        /// <param name="width"> Screenshot width. </param>
        /// <param name="height"> Screenshot height. </param>
        /// <param name="filePath"> Save file path. </param>
        /// <param name="imageFormat"> Image format (default PNG). </param>
        public static void CaptureAreaToFile(int x, int y, int width, int height, string filePath, ImageFormat? imageFormat = null)
        {
            var bitmap = CaptureArea(x, y, width, height);
            SaveBitmapToFile(bitmap, filePath, imageFormat);
        }

        #endregion CAPTURE SCREENSHOT FROM AREA METHODS

        #region CAPTURE SCREENSHOT FROM MAIN SCREEN METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Capture screenshot from main screen. </summary>
        /// <returns> Screenshot bitmap. </returns>
        public static Bitmap CaptureMainScreen()
        {
            ScreenInfo screen = ScreenManager.GetMainScreen();

            return CaptureScreen(screen.OryginalRect);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Capture screenshot from main screen as bitmap image. </summary>
        /// <returns> Screenshot bitmap image. </returns>
        public static BitmapImage CaptureMainScreenAsBitmapImage()
        {
            var bitmap = CaptureMainScreen();
            return ConvertBitmapToBitmapImage(bitmap);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Capture screenshot from main screen to clipboard. </summary>
        public static void CaptureMainScreenToClipboard()
        {
            var bitmap = CaptureMainScreen();
            CopyBitmapToClipboard(bitmap);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Capture screenshot from main screen and save to file. </summary>
        /// <param name="filePath"> Save file path. </param>
        /// <param name="imageFormat"> Image format (default PNG). </param>
        public static void CaptureMainScreenToFile(string filePath, ImageFormat? imageFormat = null)
        {
            var bitmap = CaptureMainScreen();
            SaveBitmapToFile(bitmap, filePath, imageFormat);
        }

        #endregion CAPTURE SCREENSHOT FROM MAIN SCREEN METHODS

        #region CAPTURE SCREENSHOT FROM SINGLE SCREEN METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Capture screenshot from specified screen. </summary>
        /// <param name="screenInfo"> Screen info. </param>
        /// <returns> Screenshot bitmap. </returns>
        public static Bitmap CaptureScreen(ScreenInfo screenInfo)
        {
            if (screenInfo == null)
                throw new ArgumentException($"{nameof(screenInfo)} can not be null.");

            return CaptureScreen(screenInfo.OryginalRect);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Capture screenshot from specified screen as bitmap image. </summary>
        /// <param name="screenInfo"> Screen info. </param>
        /// <returns> Screenshot bitmap image. </returns>
        public static BitmapImage CaptureScreenAsBitmapImage(ScreenInfo screenInfo)
        {
            var bitmap = CaptureScreen(screenInfo);
            return ConvertBitmapToBitmapImage(bitmap);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Capture screenshot from specified screen to clipboard. </summary>
        /// <param name="screenInfo"> Screen info. </param>
        public static void CaptureScreenToClipboard(ScreenInfo screenInfo)
        {
            var bitmap = CaptureScreen(screenInfo);
            CopyBitmapToClipboard(bitmap);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Capture screenshot from specified screen and save to file. </summary>
        /// <param name="screenInfo"> Screen info. </param>
        /// <param name="filePath"> Save file path. </param>
        /// <param name="imageFormat"> Image format (default PNG). </param>
        public static void CaptureScreenToFile(ScreenInfo screenInfo, string filePath, ImageFormat? imageFormat = null)
        {
            var bitmap = CaptureScreen(screenInfo);
            SaveBitmapToFile(bitmap, filePath, imageFormat);
        }

        #endregion CAPTURE SCREENSHOT FROM SINGLE SCREEN METHODS

        #region UTILITY METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Capture screen to bitmap. </summary>
        /// <param name="captureArea"> Capture area as rect. </param>
        /// <returns> Captured screen bitmap. </returns>
        private static Bitmap CaptureScreen(Rectangle captureArea)
        {
            Bitmap screenshot = new Bitmap(captureArea.Width, captureArea.Height);
            Size screenshotSize = new Size(captureArea.Width, captureArea.Height);

            using (Graphics graphics = Graphics.FromImage(screenshot))
            {
                graphics.Clear(Color.Black);
                graphics.CopyFromScreen(captureArea.X, captureArea.Y, 0, 0, screenshotSize);
            }

            return screenshot;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Clamp screenshot area to screens rect. </summary>
        /// <param name="x"> Screen X point. </param>
        /// <param name="y"> Screen Y point. </param>
        /// <param name="width"> Screen width. </param>
        /// <param name="height"> Screen height. </param>
        /// <param name="screensRect"> All screns rect. </param>
        /// <returns> Clamped area rect to screens rect. </returns>
        private static Rectangle ClampScreenshotArea(int x, int y, int width, int height, Rectangle screensRect)
        {
            int newX = Math.Max(screensRect.X, Math.Min(x, screensRect.Width));
            int newY = Math.Max(screensRect.Y, Math.Min(y, screensRect.Height));
            int newWidth = newX + width;
            int newHeight = newY + height;

            if (newWidth > screensRect.Width)
                newWidth = newWidth - (screensRect.Width - newWidth);

            if (newHeight > screensRect.Height)
                newHeight = newHeight - (screensRect.Height - newHeight);

            return new Rectangle(newX, newY, newWidth, newHeight);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Convert bitmap to WPF bitmap. </summary>
        /// <param name="bitmap"> Screenshot bitmap. </param>
        /// <returns> WPF bitmap. </returns>
        private static BitmapImage ConvertBitmapToBitmapImage(Bitmap screenBitmap)
        {
            BitmapImage bitmapImage = new BitmapImage();

            using (var memoryStream = new MemoryStream())
            {
                screenBitmap.Save(memoryStream, ImageFormat.Bmp);

                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new MemoryStream(memoryStream.ToArray());
                bitmapImage.EndInit();
            }

            return bitmapImage;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Copy bitmap to clipboard. </summary>
        /// <param name="screenBitmap"> Screenshot bitmap. </param>
        private static void CopyBitmapToClipboard(Bitmap screenBitmap)
        {
            System.Windows.Forms.Clipboard.SetImage(screenBitmap);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get all screens rect. </summary>
        /// <returns> All screens rect. </returns>
        private static Rectangle GetAllScreensRect()
        {
            return ScreenManager.GetAllScreens().Select(s => s.OryginalRect).Aggregate(Rectangle.Union);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Check if the file path is correct. </summary>
        /// <param name="filePath"> File path. </param>
        /// <returns> True - file path is correct; False - otherwise. </returns>
        private static bool IsPathValid(string filePath)
        {
            try
            {
                string fullPath = Path.GetFullPath(filePath);

                if (string.IsNullOrEmpty(fullPath) || !Path.IsPathRooted(fullPath))
                    return false;

                string? directoryPath = Path.GetDirectoryName(fullPath);

                if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
                    Directory.CreateDirectory(fullPath);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Save bitmap to file. </summary>
        /// <param name="screenBitmap"> Screenshot bitmap. </param>
        /// <param name="filePath"> File path. </param>
        /// <param name="imageFormat"> Image format (default PNG). </param>
        private static void SaveBitmapToFile(Bitmap screenBitmap, string filePath, ImageFormat? imageFormat = null)
        {
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException($"{nameof(filePath)} can not be null or empty.");

            if (!IsPathValid(filePath))
                throw new ArgumentException($"Ivalid file path: {filePath}.");

            screenBitmap.Save(filePath, imageFormat ?? ImageFormat.Png);
        }

        #endregion UTILITY METHODS

    }
}
