using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemManager.Data.Macros.DataModels;

namespace SystemManager.Data.Macros.Events
{
    public class MacroRunnerFinishedEventArgs : EventArgs
    {

        //  VARIABLES

        public Exception? Exception { get; private set; }
        public MacroBase? MacroItem { get; private set; }
        public int MacroItemIndex { get; private set; }
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
        public MacroRunnerFinishedEventArgs(Exception? exception = null, MacroBase? macroItem = null,
            int macroItemIndex = -1, bool stopped = false)
        {
            Exception = exception;
            MacroItem = macroItem;
            MacroItemIndex = macroItemIndex;
            Stopped = stopped;
        }

        #endregion CLASS METHODS

    }
}
