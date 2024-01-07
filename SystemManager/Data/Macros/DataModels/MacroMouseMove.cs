using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemManager.Data.Macros.DataModels
{
    public class MacroMouseMove : MacroBase
    {

        //  VARIABLES

        private double _x;
        private double _y;


        //  GETTERS & SETTERS

        public double X
        {
            get => _x;
            set => UpdateProperty(ref _x, value);
        }

        public double Y
        {
            get => _y;
            set => UpdateProperty(ref _y, value);
        }


        //  METHODS

        #region CLASS METHODS

        //  --------------------------------------------------------------------------------
        /// <summary> MacroMouseMove class constructor. </summary>
        /// <param name="x"> Mouse destination position in X axis. </param>
        /// <param name="y"> Mouse destination position in Y axis. </param>
        public MacroMouseMove(double x = 0.0, double y = 0.0)
        {
            MacroType = MacroType.MouseMove;
            X = x;
            Y = y;
        }

        #endregion CLASS METHODS

    }
}
