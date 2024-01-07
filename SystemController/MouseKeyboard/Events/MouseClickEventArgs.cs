using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemController.MouseKeyboard.Data;

namespace SystemController.MouseKeyboard.Events
{
    public class MouseClickEventArgs : EventArgs
    {

        //  VARIABLES

        public MouseButton Button { get; private set; }
        public int HeldButtons { get; private set; }
        public KeyState State { get; private set; }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> MouseClickEventArgs class constructor. </summary>
        /// <param name="mouseButton"> Mouse button. </param>
        /// <param name="keyState"> Key state. </param>
        /// <param name="heldButtons"> Currentrly held buttons. </param>
        public MouseClickEventArgs(MouseButton mouseButton, KeyState keyState, int heldButtons)
        {
            Button = mouseButton;
            State = keyState;
            HeldButtons = heldButtons;
        }

        #endregion CLASS METHODS

    }
}
