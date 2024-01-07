using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SystemManager.Data.Macros.Converters
{
    public class MacroTypeIconKindConverter : IValueConverter
    {

        //  CONST

        private static readonly PackIconKind _defaultValue = PackIconKind.QuestionMark;

        private static readonly Dictionary<MacroType, PackIconKind?> _mapping = new Dictionary<MacroType, PackIconKind?>()
        {
            { MacroType.Delay, PackIconKind.TimerOutline },
            { MacroType.KeyDown, PackIconKind.KeyboardOutline },
            { MacroType.KeyUp, PackIconKind.KeyboardOutline },
            { MacroType.KeyClick, PackIconKind.KeyboardOutline },
            { MacroType.KeyCombination, PackIconKind.KeyboardOutline },
            { MacroType.MouseDown, PackIconKind.MouseMoveDown },
            { MacroType.MouseUp, PackIconKind.MouseMoveUp },
            { MacroType.MouseClick, PackIconKind.Mouse },
            { MacroType.MouseMove, PackIconKind.MouseVariant },
            { MacroType.MouseScrollHorizontal, PackIconKind.MouseMoveVertical },
            { MacroType.MouseScrollVertical, PackIconKind.MouseMoveVertical },
        };


        //  METHODS

        //  --------------------------------------------------------------------------------
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var macroType = value as MacroType?;

            if (macroType.HasValue)
                return _mapping.TryGetValue(macroType.Value, out PackIconKind? result) && result.HasValue
                    ? result.Value : _defaultValue;

            return _defaultValue;
        }

        //  --------------------------------------------------------------------------------
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
