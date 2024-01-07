﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemController.MouseKeyboard.Data;

namespace SystemManager.Data.Macros.DataModels
{
    public class MacroMouseUp : MacroBase
    {

        //  VARIABLES

        private MouseButton _mouseButton;


        //  GETTERS & SETTERS

        public MouseButton MouseButton
        {
            get => _mouseButton;
            set => UpdateProperty(ref _mouseButton, value);
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> MacroMouseUp class constructor. </summary>
        /// <param name="mouseButton"> Mouse button. </param>
        public MacroMouseUp(MouseButton mouseButton = MouseButton.RightButton)
        {
            MacroType = MacroType.MouseDown;
            MouseButton = mouseButton;
        }

        #endregion CLASS METHODS

    }
}
