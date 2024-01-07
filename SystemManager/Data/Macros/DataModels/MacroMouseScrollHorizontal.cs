using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemManager.Data.Macros.DataModels
{
    public class MacroMouseScrollHorizontal : MacroBase
    {

        //  VARIABLES

        private long _scrollShift;


        //  GETTERS & SETTERS

        public long ScrollShift
        {
            get => _scrollShift;
            set => UpdateProperty(ref _scrollShift, value);
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> MacroMouseScrollHorizontal class constructor. </summary>
        /// <param name="scrollShift"> Scroll shift. </param>
        public MacroMouseScrollHorizontal(long scrollShift = 0)
        {
            MacroType = MacroType.MouseScrollHorizontal;
            ScrollShift = scrollShift;
        }

        #endregion CLASS METHODS

    }
}
