using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SystemManager.Data.Macros.Converters
{
    public class MacroTypeNameConverter : IValueConverter
    {

        //  CONST

        private static readonly string _defaultValue = "Unknown";

        private static readonly Dictionary<MacroType, string> _mapping = new Dictionary<MacroType, string>()
        {
            { MacroType.Delay, "Delay" },
            { MacroType.KeyDown, "Key Down" },
            { MacroType.KeyUp, "Key Up" },
            { MacroType.KeyClick, "Key Click" },
            { MacroType.KeyCombination, "Keys Combination" },
            { MacroType.MouseDown, "Mouse Down" },
            { MacroType.MouseUp, "Mouse Up" },
            { MacroType.MouseClick, "Mouse Click" },
            { MacroType.MouseMove, "Mouse Move" },
            { MacroType.MouseScrollHorizontal, "Mouse Scroll Horizontal" },
            { MacroType.MouseScrollVertical, "Mouse Scroll Vertical" },
        };


        //  METHODS

        //  --------------------------------------------------------------------------------
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var macroType = value as MacroType?;

            if (macroType.HasValue)
                return _mapping.TryGetValue(macroType.Value, out string? result)
                    ? result : _defaultValue;

            return _defaultValue;
        }

        //  --------------------------------------------------------------------------------
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
