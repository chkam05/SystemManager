using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemController.ProcessesManagement.Data;
using SystemManager.Data.Processes.Data;
using SystemManager.ViewModels.Processes;

namespace SystemManager.Data.Processes.Events
{
    public class WindowsGetterUpdateFinishedEventArgs
    {

        //  VARIABLES

        public Exception? Exception { get; private set; }
        public List<WindowInfoViewModel> Windows { get; private set; }
        public bool Stopped { get; private set; }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> WindowsGetterUpdateFinishedEventArgs class constructor. </summary>
        /// <param name="exception"> Thrown exception. </param>
        /// <param name="windows"> List of windows info view models. </param>
        /// <param name="stopped"> Is the process loading stopped. </param>
        public WindowsGetterUpdateFinishedEventArgs(
            Exception? exception = null,
            List<WindowInfoViewModel>? windows = null,
            bool stopped = false)
        {
            Exception = exception;
            Windows = windows ?? new List<WindowInfoViewModel>();
            Stopped = stopped;
        }

        #endregion CLASS METHODS

    }
}
