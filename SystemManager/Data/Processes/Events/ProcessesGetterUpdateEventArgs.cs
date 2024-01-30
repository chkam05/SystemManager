using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemController.Processes.Data;
using SystemManager.Data.Processes.Data;

namespace SystemManager.Data.Processes.Events
{
    public class ProcessesGetterUpdateEventArgs : EventArgs
    {

        //  VARIABLES

        public bool ChangeIndication { get; private set; }
        public ProcessCompareResult ComparationResult { get; private set; }
        public ProcessInfo? CurrentProcess { get; private set; }
        public ProcessInfo? NewProcess { get; private set; }
        public List<Exception>? Exceptions { get; private set; }


        //  GETTERS & SETTERS

        public bool HasErrors
        {
            get => Exceptions != null && Exceptions.Any();
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> ProcessesGetterUpdateEventArgs class constructor. </summary>
        /// <param name="changed"> Change indication. </param>
        /// <param name="comparationResult"> Comparation result. </param>
        /// <param name="currentProcess"> Current process. </param>
        /// <param name="newProcess"> New process. </param>
        /// <param name="exceptions"> List of exceptions. </param>
        public ProcessesGetterUpdateEventArgs(bool changed,
            ProcessCompareResult comparationResult = ProcessCompareResult.Equal,
            ProcessInfo? currentProcess = null,
            ProcessInfo? newProcess = null,
            List<Exception>? exceptions = null)
        {
            ChangeIndication = changed;
            ComparationResult = comparationResult;
            CurrentProcess = currentProcess;
            NewProcess = newProcess;
            Exceptions = exceptions;
        }

        #endregion CLASS METHODS

    }
}
