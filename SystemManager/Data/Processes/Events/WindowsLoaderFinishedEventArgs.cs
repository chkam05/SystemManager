using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemManager.ViewModels.Processes;

namespace SystemManager.Data.Processes.Events
{
    public class WindowsLoaderFinishedEventArgs : EventArgs
    {

        //  VARIABLES

        public Exception? Exception { get; private set; }
        public List<WindowInfoViewModel> Windows { get; private set; }
        public bool Stopped { get; private set; }


        //  GETTERS & SETTERS

        public bool HasErrors
        {
            get => Exception != null;
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> WindowsLoaderFinishedEventArgs class methods. </summary>
        /// <param name="exception"> Run exception. </param>
        /// <param name="windows"> List of WindowInfoViewModels. </param>
        /// <param name="stopped"> Loading stopped. </param>
        public WindowsLoaderFinishedEventArgs(Exception? exception = null, List<WindowInfoViewModel>? windows = null,
            bool stopped = false)
        {
            Exception = exception;
            Windows = windows ?? new List<WindowInfoViewModel>();
            Stopped = stopped;
        }

        #endregion CLASS METHODS

    }
}
