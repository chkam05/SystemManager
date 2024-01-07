using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemController.Data;

namespace SystemController.MouseKeyboard.Events
{
    public class MouseMoveEventArgs : EventArgs
    {

        //  VARIABLES

        public POINT CurrentPosition { get; private set; }
        public POINT PreviousPosition { get; private set; }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> MouseMoveEventArgs class constructor. </summary>
        /// <param name="currentPosition"> Current cursor position. </param>
        /// <param name="previousPosition"> Previous cursor position. </param>
        public MouseMoveEventArgs(POINT currentPosition, POINT previousPosition)
        {
            CurrentPosition = currentPosition;
            PreviousPosition = previousPosition;
        }

        #endregion CLASS METHODS

    }
}
