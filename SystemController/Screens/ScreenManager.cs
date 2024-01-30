using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using SystemController.Data;
using SystemController.Screens.Data;

namespace SystemController.Screens
{
    public static class ScreenManager
    {

        //  CONST

        const int ENUM_CURRENT_SETTINGS = -1;


        //  IMPORT

        [DllImport("user32.dll")]
        static extern bool EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);


        //  METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Get screens info array from all displays. </summary>
        /// <returns> Array of screens info. </returns>
        public static ScreenInfo[] GetAllScreens()
        {
            return Screen.AllScreens
                .Select(s => ScreenToInfo(s))
                .ToArray();
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get main display screen info. </summary>
        /// <returns> Main display screen info. </returns>
        public static ScreenInfo GetMainScreen()
        {
            var firstScreen = Screen.AllScreens.First(s => s.Primary);
            return ScreenToInfo(firstScreen);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get the number of screens. </summary>
        /// <returns> Number of screens. </returns>
        public static int GetScreenCount()
        {
            return Screen.AllScreens.Count();
        }

        #region UTILITY METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Get device mode. </summary>
        /// <param name="deviceName"> Device name. </param>
        /// <returns> Device mode object. </returns>
        private static DEVMODE? GetDevMode(string deviceName)
        {
            try
            {
                var dm = new DEVMODE();
                dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
                EnumDisplaySettings(deviceName, ENUM_CURRENT_SETTINGS, ref dm);

                return dm;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Get scale. </summary>
        /// <param name="oryginal"> Original dimension value. </param>
        /// <param name="scaled"> Scaled dimension value. </param>
        /// <returns> Scale. </returns>
        private static float GetScale(int oryginal, int scaled)
        {
            return (100f * scaled) / (float)oryginal;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Scale </summary>
        /// <param name="value"> Value to scale. </param>
        /// <param name="scale"> Scale. </param>
        /// <returns> Scaled value. </returns>
        private static int Scale(int value, float scale)
        {
            return Convert.ToInt32(value * (scale / 100));
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Convert screen object to screen info. </summary>
        /// <param name="screen"> Screen object. </param>
        /// <returns> Screen info. </returns>
        private static ScreenInfo ScreenToInfo(Screen screen)
        {
            var devMode = GetDevMode(screen.DeviceName);
            var deviceNameParts = screen.DeviceName.Split("\\", StringSplitOptions.RemoveEmptyEntries);

            var screenInfo = new ScreenInfo()
            {
                BitsPerPixel = devMode?.dmBitsPerPel ?? screen.BitsPerPixel,
                DeviceName = string.Join(string.Empty, deviceNameParts),
                DevicePath = screen.DeviceName,
                DriverVersion = null,
                Frequency = null,
                InternalDeviceName = null,
                IsMainScreen = screen.Primary,
                Orientation = null,
                Position = null,
                Size = null,
                SpecVersion = null,
                VirtualPosition = new POINT { X = screen.Bounds.X, Y = screen.Bounds.Y },
                VirtualSize = new SIZE { Width = screen.Bounds.Width, Height = screen.Bounds.Height },
                VirtualWorkPosition = new POINT { X = screen.WorkingArea.X, Y = screen.WorkingArea.Y },
                VirtualWorkSize = new SIZE { Width = screen.WorkingArea.Width, Height = screen.WorkingArea.Height },
                WorkPosition = null,
                WorkSize = null,
            };

            if (devMode.HasValue)
            {
                var xScale = GetScale(devMode.Value.dmPelsWidth, screen.Bounds.Width);
                var yScale = GetScale(devMode.Value.dmPelsHeight, screen.Bounds.Height);

                screenInfo.Position = new POINT()
                {
                    X = devMode.Value.dmPositionX,
                    Y = devMode.Value.dmPositionY,
                };

                screenInfo.Size = new SIZE()
                {
                    Width = devMode.Value.dmPelsWidth,
                    Height = devMode.Value.dmPelsHeight,
                };

                screenInfo.XScale = xScale;
                screenInfo.YScale = yScale;

                screenInfo.WorkPosition = new POINT()
                {
                    X = Scale(screen.WorkingArea.X, xScale),
                    Y = Scale(screen.WorkingArea.Y, yScale)
                };

                screenInfo.WorkSize = new SIZE()
                {
                    Width = Scale(screen.WorkingArea.Width, xScale),
                    Height = Scale(screen.WorkingArea.Height, yScale)
                };

                var driverVersionParts = devMode?.dmDriverVersion.ToString().Split();
                var specVersionParts = devMode?.dmSpecVersion.ToString().Split();

                screenInfo.DriverVersion = driverVersionParts != null ? string.Join(".", driverVersionParts) : null;
                screenInfo.SpecVersion = specVersionParts != null ? string.Join(".", specVersionParts) : null;
                screenInfo.Frequency = devMode?.dmDisplayFrequency;
                screenInfo.InternalDeviceName = devMode?.dmDeviceName;

                if (devMode?.dmDisplayOrientation != null)
                {
                    switch (devMode.Value.dmDisplayOrientation)
                    {
                        case ScreenOrientation.Angle0:
                            screenInfo.Orientation = 0;
                            break;

                        case ScreenOrientation.Angle90:
                            screenInfo.Orientation = 90;
                            break;

                        case ScreenOrientation.Angle180:
                            screenInfo.Orientation = 180;
                            break;

                        case ScreenOrientation.Angle270:
                            screenInfo.Orientation = 270;
                            break;
                    }
                }
            }

            return screenInfo;
        }

        #endregion UTILITY METHODS

    }
}
