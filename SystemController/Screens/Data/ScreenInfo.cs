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

    }
}
