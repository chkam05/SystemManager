using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemManager.Windows.Events
{
    public class ScreenAreaCapturedEventArgs : EventArgs
    {

        //  VARIABLES

        public double X { get; private set; }
        public double Y { get; private set; }
        public double Width { get; private set; }
        public double Height { get; private set; }


        //  GETTERS & SETTERS

        public Rectangle Rectangle
        {
            get => new Rectangle(
                Convert.ToInt32(X),
                Convert.ToInt32(Y),
                Convert.ToInt32(Width),
                Convert.ToInt32(Height));
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> ScreenAreaCapturedEventArgs class constructor. </summary>
        /// <param name="x"> Position X. </param>
        /// <param name="y"> Position Y. </param>
        /// <param name="width"> Width. </param>
        /// <param name="height"> Height. </param>
        public ScreenAreaCapturedEventArgs(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        #endregion CLASS METHODS

    }
}
