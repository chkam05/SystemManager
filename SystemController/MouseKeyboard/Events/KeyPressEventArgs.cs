using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemController.MouseKeyboard.Data;

namespace SystemController.MouseKeyboard.Events
{
    public class KeyPressEventArgs : EventArgs
    {

        //  VARIABLES

        public byte KeyCode { get; private set; }
        public KeyState KeyState { get; private set; }
        public byte[] HeldKeyCodes { get; private set; }
        public int HeldKeys { get; private set; }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> KeyPressEventArgs class constructor. </summary>
        /// <param name="keyCode"> Key code. </param>
        /// <param name="heldKeyCodes"> Currently held key codes. </param>
        /// <param name="keyState"> Key state. </param>
        /// <param name="heldKeys"> Currently held keys. </param>
        public KeyPressEventArgs(byte keyCode, byte[] heldKeyCodes, KeyState keyState, int heldKeys) : base()
        {
            KeyCode = keyCode;
            HeldKeyCodes = heldKeyCodes;
            KeyState = keyState;
            HeldKeys = heldKeys;
        }

        #endregion CLASS METHODS

    }
}
