using System.Windows.Forms;
using SystemController.Screens;
using SystemController.Screens.Data;

namespace SystemControllerTests
{
    [TestFixture]
    public class Tests
    {

        //  TEST METHODS

        //  --------------------------------------------------------------------------------
        [Test(Description = "GetAllScreens from ScreenManager Test")]
        [Order(0)]
        public void GetAllScreensTest()
        {
            var allScreens = ScreenManager.GetAllScreens();

            Assert.IsNotNull(allScreens);
            Assert.IsTrue(allScreens.Any());
            Assert.IsTrue(allScreens.Any(s => s.IsMainScreen));

            foreach (var screenInfo in allScreens)
                TestScreenInfo(screenInfo);
        }

        //  --------------------------------------------------------------------------------
        [Test(Description = "GetMainScreen from ScreenManager Test")]
        [Order(1)]
        public void GetMainScreenTest()
        {
            var mainScreen = ScreenManager.GetMainScreen();

            TestScreenInfo(mainScreen);
        }

        //  --------------------------------------------------------------------------------
        [Test(Description = "GetScreenCount from ScreenManager Test")]
        [Order(2)]
        public void GetScreenCountTest()
        {
            var allScreens = ScreenManager.GetAllScreens();
            int screenNumber = ScreenManager.GetScreenCount();

            Assert.IsTrue(screenNumber > 0);
            Assert.IsTrue(screenNumber == (allScreens?.Count() ?? -1));
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Test single screen info. </summary>
        /// <param name="screenInfo"> Screen info. </param>
        /// <param name="isMainScreen"> Should be main screen. </param>
        private void TestScreenInfo(ScreenInfo screenInfo, bool isMainScreen = false)
        {
            Assert.IsNotNull(screenInfo);

            Assert.IsTrue(screenInfo.BitsPerPixel > 0);
            Assert.IsTrue(!string.IsNullOrEmpty(screenInfo.DeviceName));
            Assert.IsTrue(!string.IsNullOrEmpty(screenInfo.DevicePath));
            Assert.IsTrue(!string.IsNullOrEmpty(screenInfo.DriverVersion));
            Assert.IsTrue(!string.IsNullOrEmpty(screenInfo.InternalDeviceName));
            Assert.IsTrue(!string.IsNullOrEmpty(screenInfo.SpecVersion));

            Assert.IsNotNull(screenInfo.Frequency);
            Assert.IsNotNull(screenInfo.Orientation);
            Assert.IsNotNull(screenInfo.XScale);
            Assert.IsNotNull(screenInfo.YScale);

            Assert.IsNotNull(screenInfo.Position);
            Assert.IsNotNull(screenInfo.Size);
            Assert.IsNotNull(screenInfo.VirtualPosition);
            Assert.IsNotNull(screenInfo.VirtualSize);
            Assert.IsNotNull(screenInfo.VirtualWorkPosition);
            Assert.IsNotNull(screenInfo.VirtualWorkSize);
            Assert.IsNotNull(screenInfo.WorkPosition);
            Assert.IsNotNull(screenInfo.WorkSize);

            if (isMainScreen)
                Assert.IsTrue(screenInfo.IsMainScreen);
        }

    }
}