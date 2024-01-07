using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemController.MouseKeyboard.Data;

namespace SystemManager.Data.Macros.DataModels
{
    public class MacroKeyUp : MacroBase
    {

        //  VARIABLES

        private byte _keyCode;


        //  GETTERS & SETTERS

        public byte KeyCode
        {
            get => _keyCode;
            set => UpdateProperty(ref _keyCode, value);
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> MacroKeyUp class constructor. </summary>
        /// <param name="keyCode"> Key code. </param>
        public MacroKeyUp(byte keyCode = Keys.VK_RETURN)
        {
            MacroType = MacroType.KeyUp;
            KeyCode = keyCode;
        }

        #endregion CLASS METHODS

    }
}
