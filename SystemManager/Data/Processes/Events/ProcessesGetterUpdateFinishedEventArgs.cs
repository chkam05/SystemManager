using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemManager.ViewModels.Processes;

namespace SystemManager.Data.Processes.Events
{
    public class ProcessesGetterUpdateFinishedEventArgs : EventArgs
    {

        //  VARIABLES

        public Exception? Exception { get; private set; }
        public int LastProcessIndex { get; private set; }
        public bool Stopped { get; private set; }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> ProcessesGetterUpdateFinishedEventArgs class constructor. </summary>
        /// <param name="exception"> Thrown exception. </param>
        /// <param name="lastProcessIndex"> Process index. </param>
        /// <param name="stopped"> Is the process loading stopped. </param>
        public ProcessesGetterUpdateFinishedEventArgs(
            Exception? exception = null,
            int lastProcessIndex = -1,
            bool stopped = false)
        {
            Exception = exception;
            LastProcessIndex = lastProcessIndex;
            Stopped = stopped;
        }

        #endregion CLASS METHODS

    }
}
