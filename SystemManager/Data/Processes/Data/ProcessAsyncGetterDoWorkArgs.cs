using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemManager.Data.Processes.Data
{
    public class ProcessAsyncGetterDoWorkArgs
    {

        //  VARIABLES

        public bool AutoUpdate { get; set; }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> ProcessAsyncGetterDoWorkArgs class constructor. </summary>
        /// <param name="autoUpdate"> Auto update param. </param>
        public ProcessAsyncGetterDoWorkArgs(bool autoUpdate)
        {
            AutoUpdate = autoUpdate;
        }

        #endregion CLASS METHODS

    }
}
