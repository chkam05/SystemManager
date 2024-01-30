using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemController.Processes.Data;
using SystemManager.Data.Processes.Data;

namespace SystemManager.Data.Processes.Events
{
    public class WindowsGetterUpdateEventArgs : EventArgs
    {

        //  VARIABLES

        public bool ChangeIndication { get; private set; }
        public ProcessCompareResult ComparationResult { get; private set; }
        public WindowInfo? CurrentWindow { get; private set; }
        public WindowInfo? NewWindow { get; private set; }
        public List<Exception>? Exceptions { get; private set; }


        //  GETTERS & SETTERS

        public bool HasErrors
        {
            get => Exceptions != null && Exceptions.Any();
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> WindowsGetterUpdateEventArgs class constructor. </summary>
        public WindowsGetterUpdateEventArgs(bool changed,
            ProcessCompareResult comparationResult = ProcessCompareResult.Equal,
            WindowInfo? currentWindow = null,
            WindowInfo? newWindow = null,
            List<Exception>? exceptions = null)
        {
            ChangeIndication = changed;
            ComparationResult = comparationResult;
            CurrentWindow = currentWindow;
            NewWindow = newWindow;
            Exceptions = exceptions;
        }

        #endregion CLASS METHODS

    }
}
