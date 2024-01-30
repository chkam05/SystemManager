using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemController.Processes.Data;

namespace SystemManager.Data.Processes.Data
{
    public class WindowsAsyncGetterDoWorkArgs
    {

        //  VARIABLES

        public bool AutoUpdate { get; set; }
        public ProcessInfo ProcessInfo { get; set; }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> WindowsAsyncGetterDoWorkArgs class constructor. </summary>
        /// <param name="autoUpdate"> Auto update param. </param>
        /// <param name="processInfo"> Process info. </param>
        public WindowsAsyncGetterDoWorkArgs(bool autoUpdate, ProcessInfo processInfo)
        {
            AutoUpdate = autoUpdate;
            ProcessInfo = processInfo;
        }

        #endregion CLASS METHODS

    }
}
