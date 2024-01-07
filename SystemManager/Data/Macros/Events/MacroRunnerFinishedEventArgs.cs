using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemManager.Data.Macros.Events
{
    public class MacroRunnerFinishedEventArgs : EventArgs
    {

        //  VARIABLES

        public Exception? Exception { get; private set; }
        public bool Stopped { get; private set; }


        //  GETTERS & SETTERS

        public bool HasErrors
        {
            get => Exception != null;
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> MacroRunnerFinishedEventArgs class methods. </summary>
        /// <param name="exception"> Run exception. </param>
        public MacroRunnerFinishedEventArgs(Exception? exception = null, bool stopped = false)
        {
            Exception = exception;
            Stopped = stopped;
        }

        #endregion CLASS METHODS

    }
}
