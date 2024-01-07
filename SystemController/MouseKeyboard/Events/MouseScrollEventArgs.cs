using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemController.MouseKeyboard.Data;

namespace SystemController.MouseKeyboard.Events
{
    public class MouseScrollEventArgs : EventArgs
    {

        //  VARIABLES

        public int Delta { get; private set; }
        public ScrollOrientation Orientation { get; private set; }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> MouseScrollEventArgs class constructor. </summary>
        /// <param name="delta"> Scroll delta. </param>
        /// <param name="scrollOrientation"> Scroll orientation. </param>
        public MouseScrollEventArgs(int delta, ScrollOrientation scrollOrientation)
        {
            Delta = delta;
            Orientation = scrollOrientation;
        }

        #endregion CLASS METHODS

    }
}
