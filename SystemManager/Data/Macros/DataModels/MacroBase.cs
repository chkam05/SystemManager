using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemManager.ViewModels.Base;

namespace SystemManager.Data.Macros.DataModels
{
    public abstract class MacroBase : BaseViewModel
    {

        //  VARIABLES

        private MacroType _macroType;


        //  GETTERS & SETTERS

        public MacroType MacroType
        {
            get => _macroType;
            protected set => UpdateProperty(ref _macroType, value);
        }

    }
}
