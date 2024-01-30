using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemController.Data;

namespace SystemController.Screens.Data
{
    public class ScreenInfo
    {

        //  VARIABLES

        public int BitsPerPixel { get; set; }
        public string? DeviceName { get; set; }
        public string? DevicePath { get; set; }
        public string? DriverVersion { get; set; }
        public int? Frequency { get; set; }
        public string? InternalDeviceName { get; set; }
        public bool IsMainScreen { get; set; }
        public int? Orientation { get; set; }
        public POINT? Position { get; set; }
        public SIZE? Size { get; set; }
        public string? SpecVersion { get; set; }
        public POINT VirtualPosition { get; set; }
        public SIZE VirtualSize { get; set; }
        public POINT VirtualWorkPosition { get; set; }
        public SIZE VirtualWorkSize { get; set; }
        public POINT? WorkPosition { get; set; }
        public SIZE? WorkSize { get; set; }
        public float? XScale { get; set; }
        public float? YScale { get; set; }


        //  GETTERS & SETTERS

        public Rectangle OryginalRect
        {
            get => new Rectangle(
                Position?.X ?? VirtualPosition.X,
                Position?.Y ?? VirtualPosition.Y,
                Size?.Width ?? VirtualSize.Width,
                Size?.Height ?? VirtualSize.Height);
        }

        public Rectangle OryginalWorkRect
        {
            get => new Rectangle(
                WorkPosition?.X ?? VirtualWorkPosition.X,
                WorkPosition?.Y ?? VirtualWorkPosition.Y,
                WorkSize?.Width ?? VirtualWorkSize.Width,
                WorkSize?.Height ?? VirtualWorkSize.Height);
        }

        public Rectangle VirtualRect
        {
            get => new Rectangle(
                VirtualPosition.X,
                VirtualPosition.Y,
                VirtualSize.Width,
                VirtualSize.Height);
        }

        public Rectangle VirtualWorkRect
        {
            get => new Rectangle(
                VirtualWorkPosition.X,
                VirtualWorkPosition.Y,
                VirtualWorkSize.Width,
                VirtualWorkSize.Height);
        }


        //  METHODS

        #region MAP METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> Check if point is in virtual area. </summary>
        /// <param name="virtualPoint"> Virtual point. </param>
        /// <returns> True - point is in virtual area. </returns>
        public bool IsInVirtualRange(Point virtualPoint)
        {
            return IsInVirtualRange(virtualPoint.X, virtualPoint.Y);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Check if point is in virtual area. </summary>
        /// <param name="x"> X point. </param>
        /// <param name="y"> Y point. </param>
        /// <returns> True - point is in virtual area. </returns>
        public bool IsInVirtualRange(double x, double y)
        {
            return IsInVirtualRange(Convert.ToInt32(x), Convert.ToInt32(y));
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Check if point is in virtual area. </summary>
        /// <param name="x"> X point. </param>
        /// <param name="y"> Y point. </param>
        /// <returns> True - point is in virtual area. </returns>
        public bool IsInVirtualRange(int x, int y)
        {
            if (x < VirtualPosition.X || y < VirtualPosition.Y)
                return false;

            if (x > VirtualSize.Width || y > VirtualSize.Height)
                return false;

            return true;
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Map virtual area to oryignal area. </summary>
        /// <param name="virtualRect"> Virtual area react. </param>
        /// <returns> Area mapped to oryginal area. </returns>
        public Rectangle MapToOryginalSize(Rectangle virtualRect)
        {
            return MapToOryginalSize(virtualRect.X, virtualRect.Y, virtualRect.Width, virtualRect.Height);
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Map virtual area to oryignal area. </summary>
        /// <param name="x"> X point. </param>
        /// <param name="y"> Y point. </param>
        /// <param name="width"> Width. </param>
        /// <param name="height"> Height. </param>
        /// <returns> Area mapped to oryginal area. </returns>
        public Rectangle MapToOryginalSize(double x, double y, double width, double height)
        {
            return MapToOryginalSize(
                Convert.ToInt32(x),
                Convert.ToInt32(y),
                Convert.ToInt32(width),
                Convert.ToInt32(height));
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Map virtual area to oryignal area. </summary>
        /// <param name="x"> X point. </param>
        /// <param name="y"> Y point. </param>
        /// <param name="width"> Width. </param>
        /// <param name="height"> Height. </param>
        /// <returns> Area mapped to oryginal area. </returns>
        public Rectangle MapToOryginalSize(int x, int y, int width, int height)
        {
            return new Rectangle(ScaleX(x), ScaleY(y), ScaleX(width), ScaleY(height));
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Scale X point or width to oryginal. </summary>
        /// <param name="x"> X point or width. </param>
        /// <returns> Oryginal X point or width. </returns>
        private int ScaleX(int x)
        {
            return Convert.ToInt32((100 * x) / (XScale ?? 1));
        }

        //  --------------------------------------------------------------------------------
        /// <summary> Scale Y point or height to oryginal. </summary>
        /// <param name="x"> Y point or height. </param>
        /// <returns> Oryginal Y point or height. </returns>
        private int ScaleY(int y)
        {
            return Convert.ToInt32((100 * y) / (YScale ?? 1));
        }

        #endregion MAP METHODS

    }
}
