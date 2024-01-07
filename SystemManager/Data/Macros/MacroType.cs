using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemManager.Data.Macros
{
    public enum MacroType
    {
        Delay = 0,
        KeyDown = 1,
        KeyUp = 2,
        KeyClick = 3,
        KeyCombination = 4,
        MouseDown = 5,
        MouseUp = 6,
        MouseClick = 7,
        MouseMove = 8,
        MouseScrollHorizontal = 9,
        MouseScrollVertical = 10,
    }
}
