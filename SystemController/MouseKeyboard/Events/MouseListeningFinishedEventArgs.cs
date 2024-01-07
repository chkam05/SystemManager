using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemController.MouseKeyboard.Data;

namespace SystemController.MouseKeyboard.Events
{
    public class MouseListeningFinishedEventArgs : EventArgs
    {

        //  VARIABLES

        public int HeldButtons { get; private set; }
        public POINT LastCursorPosition { get; private set; }
        public List<Exception> ThrownExceptions { get; private set; }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> MouseListeningFinishedEventArgs class constructor. </summary>
        /// <param name="thrownExceptions"> List of thrown exceptions. </param>
        /// <param name="heldButtons"> Currentrly held buttons. </param>
        /// <param name="cursorPosition"> Last cursor position. </param>
        public MouseListeningFinishedEventArgs(List<Exception> thrownExceptions, int heldButtons, POINT cursorPosition)
        {
            ThrownExceptions = thrownExceptions;
            HeldButtons = heldButtons;
            LastCursorPosition = cursorPosition;
        }

        #endregion CLASS METHODS

    }
}
