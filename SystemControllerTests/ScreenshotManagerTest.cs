using NUnit.Framework.Internal;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using SystemController.Screens;
using SystemController.Screens.Data;

namespace SystemControllerTests
{
    [TestFixture]
    public class ScreenshotManagerTest
    {

        //  TEST METHODS

        //  --------------------------------------------------------------------------------
        [Test(Description = "CaptureAllScreens from ScreenshotManager Test")]
        [Order(0)]
        public void CaptureAllScreensTest()
        {
            var bitmap = ScreenshotManager.CaptureAllScreens();
            TestBitmap(bitmap);
        }

        //  --------------------------------------------------------------------------------
        [Test(Description = "CaptureAllScreensAsBitmapImage from ScreenshotManager Test")]
        [Order(1)]
        public void CaptureAllScreensAsBitmapImageTest()
        {
            var bitmapImage = ScreenshotManager.CaptureAllScreensAsBitmapImage();
            TestBitmapImage(bitmapImage);
        }

        //  --------------------------------------------------------------------------------
        [Test(Description = "CaptureAllScreensToClipboard from ScreenshotManager Test")]
        [Order(2)]
        [Apartment(ApartmentState.STA)]
        public void CaptureAllScreensToClipboardTest()
        {
            ScreenshotManager.CaptureAllScreensToClipboard();
            TestClipboard();
        }

        //  --------------------------------------------------------------------------------
        [Test(Description = "CaptureAllScreensToFile from ScreenshotManager Test")]
        [Order(3)]
        public void CaptureAllScreensToFileTest()
        {
            string filePath = Path.Combine(GetApplicationDirectory(), "testImage.png");

            ScreenshotManager.CaptureAllScreensToFile(filePath);
            TestFile(filePath);
        }

        //  --------------------------------------------------------------------------------
        [Test(Description = "CaptureArea from ScreenshotManager Test")]
        [Order(4)]
        public void CaptureAreaTest()
        {
            var rect = PrepareTestArea(100);
            var bitmap = ScreenshotManager.CaptureArea(rect.X, rect.Y, rect.Width, rect.Height);
            TestBitmap(bitmap);
        }

        //  --------------------------------------------------------------------------------
        [Test(Description = "CaptureAreaAsBitmapImage from ScreenshotManager Test")]
        [Order(5)]
        public void CaptureAreaAsBitmapImageTest()
        {
            var rect = PrepareTestArea(100);
            var bitmapImage = ScreenshotManager.CaptureAreaAsBitmapImage(rect.X, rect.Y, rect.Width, rect.Height);
            TestBitmapImage(bitmapImage);
        }

        //  --------------------------------------------------------------------------------
        [Test(Description = "CaptureAreaToClipboard from ScreenshotManager Test")]
        [Order(6)]
        [Apartment(ApartmentState.STA)]
        public void CaptureAreaToClipboardTest()
        {
            var rect = PrepareTestArea(100);
            ScreenshotManager.CaptureAreaToClipboard(rect.X, rect.Y, rect.Width, rect.Height);
            TestClipboard();
        }

        //  --------------------------------------------------------------------------------
        [Test(Description = "CaptureAreaToFile from ScreenshotManager Test")]
        [Order(7)]
        public void CaptureAreaToFileTest()
        {
            string filePath = Path.Combine(GetApplicationDirectory(), "testImage.png");
            var rect = PrepareTestArea(100);

            ScreenshotManager.CaptureAreaToFile(rect.X, rect.Y, rect.Width, rect.Height, filePath);
            TestFile(filePath);
        }

        //  --------------------------------------------------------------------------------
        [Test(Description = "CaptureMainScreen from ScreenshotManager Test")]
        [Order(8)]
        public void CaptureMainScreenTest()
        {
            var bitmap = ScreenshotManager.CaptureMainScreen();
            TestBitmap(bitmap);
        }

        //  --------------------------------------------------------------------------------
        [Test(Description = "CaptureMainScreenAsBitmapImage from ScreenshotManager Test")]
        [Order(9)]
        public void CaptureMainScreenAsBitmapImageTest()
        {
            var bitmapImage = ScreenshotManager.CaptureMainScreenAsBitmapImage();
            TestBitmapImage(bitmapImage);
        }

        //  --------------------------------------------------------------------------------
        [Test(Description = "CaptureMainScreenToClipboard from ScreenshotManager Test")]
        [Order(10)]
        [Apartment(ApartmentState.STA)]
        public void CaptureMainScreenToClipboardTest()
        {
            ScreenshotManager.CaptureMainScreenToClipboard();
            TestClipboard();
        }

        //  --------------------------------------------------------------------------------
        [Test(Description = "CaptureMainScreenToFile from ScreenshotManager Test")]
        [Order(11)]
        public void CaptureMainScreenToFileTest()
        {
            string filePath = Path.Combine(GetApplicationDirectory(), "testImage.png");

            ScreenshotManager.CaptureMainScreenToFile(filePath);
            TestFile(filePath);
        }

        //  --------------------------------------------------------------------------------
        [Test(Description = "CaptureScreen from ScreenshotManager Test")]
        [Order(12)]
        public void CaptureScreenTest()
        {
            var screen = ScreenManager.GetAllScreens().First();
            var bitmap = ScreenshotManager.CaptureScreen(screen);
            TestBitmap(bitmap);
        }

        //  --------------------------------------------------------------------------------
        [Test(Description = "CaptureScreenAsBitmapImage from ScreenshotManager Test")]
        [Order(13)]
        public void CaptureScreenAsBitmapImageTest()
        {
            var screen = ScreenManager.GetAllScreens().First();
            var bitmapImage = ScreenshotManager.CaptureScreenAsBitmapImage(screen);
            TestBitmapImage(bitmapImage);
        }

        //  --------------------------------------------------------------------------------
        [Test(Description = "CaptureScreenToClipboard from ScreenshotManager Test")]
        [Order(14)]
        [Apartment(ApartmentState.STA)]
        public void CaptureScreenToClipboardTest()
        {
            var screen = ScreenManager.GetAllScreens().First();

            ScreenshotManager.CaptureScreenToClipboard(screen);
            TestClipboard();
        }

        //  --------------------------------------------------------------------------------
        [Test(Description = "CaptureScreenToFile from ScreenshotManager Test")]
        [Order(15)]
        public void CaptureScreenToFileTest()
        {
            string filePath = Path.Combine(GetApplicationDirectory(), "testImage.png");
            var screen = ScreenManager.GetAllScreens().First();

            ScreenshotManager.CaptureScreenToFile(screen, filePath);
            TestFile(filePath);
        }

        #region UTILITY METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Get application directory. </summary>
        /// <returns> Application directory path. </returns>
        private string GetApplicationDirectory()
        {
            return Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly()?.Location)
                ?? AppDomain.CurrentDomain.BaseDirectory;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Prepare test rect area. </summary>
        /// <param name="shift"> Shift. </param>
        /// <returns> Test rect area. </returns>
        private Rectangle PrepareTestArea(int shift = 100)
        {
            Rectangle screensRect = ScreenManager.GetAllScreens()
                .Select(s => s.OryginalRect)
                .Aggregate(Rectangle.Union);

            return new Rectangle(
                screensRect.X + shift,
                screensRect.Y + shift,
                screensRect.Width - shift * 2,
                screensRect.Height - shift * 2);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Test bitmap. </summary>
        /// <param name="bitmap"> Bitmap. </param>
        private void TestBitmap(Bitmap bitmap)
        {
            Assert.IsNotNull(bitmap);
            Assert.IsTrue(bitmap.Height > 0);
            Assert.IsTrue(bitmap.Width > 0);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Test bitmap image. </summary>
        /// <param name="bitmapImage"> Bitmap image. </param>
        private void TestBitmapImage(ImageSource bitmapImage)
        {
            Assert.IsNotNull(bitmapImage);
            Assert.IsTrue(bitmapImage.Height > 0);
            Assert.IsTrue(bitmapImage.Width > 0);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Test clipboard. </summary>
        private void TestClipboard()
        {
            var bitmapImage = Clipboard.GetImage();
            TestBitmapImage(bitmapImage);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Test file. </summary>
        /// <param name="filePath"> File path. </param>
        private void TestFile(string filePath)
        {
            Assert.IsTrue(File.Exists(filePath));

            Bitmap bitmap = new Bitmap(filePath);

            Assert.IsNotNull(bitmap);
            Assert.IsTrue(bitmap.Height > 0);
            Assert.IsTrue(bitmap.Width > 0);

            if (bitmap != null)
                bitmap.Dispose();

            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        #endregion UTILITY METHODS

    }
}
