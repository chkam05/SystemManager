using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemController.MouseKeyboard.Events
{
    public class KeysListeningFinishedEventArgs : EventArgs
    {

        //  VARIABLES

        public int HeldKeys { get; private set; }
        public List<Exception> ThrownExceptions { get; private set; }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> KeyPressListeningFinishedEventArgs class constructor. </summary>
        /// <param name="thrownExceptions"> List of thrown exceptions. </param>
        /// <param name="heldKeys"> Currentrly held keys. </param>
        public KeysListeningFinishedEventArgs(List<Exception> thrownExceptions, int heldKeys)
        {
            ThrownExceptions = thrownExceptions;
            HeldKeys = heldKeys;
        }

        #endregion CLASS METHODS

    }
}
