using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemManager.Data.Macros.DataModels
{
    public class MacroMouseScrollVertical : MacroBase
    {

        //  VARIABLES

        private int _scrollShift;


        //  GETTERS & SETTERS

        public int ScrollShift
        {
            get => _scrollShift;
            set => UpdateProperty(ref _scrollShift, value);
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> MacroMouseScrollVertical class constructor. </summary>
        /// <param name="scrollShift"> Scroll shift. </param>
        public MacroMouseScrollVertical(int scrollShift = 0)
        {
            MacroType = MacroType.MouseScrollVertical;
            ScrollShift = scrollShift;
        }

        #endregion CLASS METHODS

    }
}
