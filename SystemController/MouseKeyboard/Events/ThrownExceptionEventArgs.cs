using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemController.MouseKeyboard.Events
{
    public class ThrownExceptionEventArgs : EventArgs
    {

        //  VARIABLES

        public Exception Exception { get; private set; }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> KeyPressExceptionEventArgs class constructor. </summary>
        /// <param name="exception"> Throwed exception. </param>
        public ThrownExceptionEventArgs(Exception exception) : base()
        {
            Exception = exception;
        }

        #endregion CLASS METHODS

    }
}
