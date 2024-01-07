using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemManager.Data.Macros.DataModels
{
    public class MacroDelay : MacroBase
    {

        //  VARIABLES

        private long _delayMiliseconds;


        //  GETTERS & SETTERS

        public long DelayMiliseconds
        {
            get => _delayMiliseconds;
            set => UpdateProperty(ref _delayMiliseconds, value);
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> MacroDelay class constructor. </summary>
        /// <param name="delayMiliseconds"> Delay in miliseconds. </param>
        public MacroDelay(long delayMiliseconds = 1000)
        {
            MacroType = MacroType.Delay;
            DelayMiliseconds = delayMiliseconds;
        }

        #endregion CLASS METHODS

    }
}
