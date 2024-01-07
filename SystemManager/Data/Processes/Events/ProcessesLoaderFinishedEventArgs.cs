using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemManager.Data.Macros.DataModels;

namespace SystemManager.Data.Processes.Events
{
    public class ProcessesLoaderFinishedEventArgs : EventArgs
    {

        //  VARIABLES

        public Exception? Exception { get; private set; }
        public int LastProcessIndex { get; private set; }
        public bool Stopped { get; private set; }


        //  GETTERS & SETTERS

        public bool HasErrors
        {
            get => Exception != null;
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> ProcessesLoaderFinishedEventArgs class methods. </summary>
        /// <param name="exception"> Run exception. </param>
        /// <param name="lastProcessIndex"> Index of last loaded process. </param>
        /// <param name="stopped"> Loading stopped. </param>
        public ProcessesLoaderFinishedEventArgs(Exception? exception = null, int lastProcessIndex = -1,
            bool stopped = false)
        {
            Exception = exception;
            LastProcessIndex = lastProcessIndex;
            Stopped = stopped;
        }

        #endregion CLASS METHODS

    }
}
