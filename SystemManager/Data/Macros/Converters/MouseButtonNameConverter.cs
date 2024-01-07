using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using SystemController.MouseKeyboard.Data;

namespace SystemManager.Data.Macros.Converters
{
    public class MouseButtonNameConverter : IValueConverter
    {

        //  CONST

        private static readonly string _defaultValue = string.Empty;

        private static readonly Dictionary<MouseButton, string> _mapping = new Dictionary<MouseButton, string>()
        {
            { MouseButton.LeftButton, "LMB" },
            { MouseButton.MiddleButton, "MMB" },
            { MouseButton.RightButton, "RMB" },
        };


        //  METHODS

        //  --------------------------------------------------------------------------------
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var mouseButton = value as MouseButton?;

            if (mouseButton.HasValue)
                return _mapping.TryGetValue(mouseButton.Value, out string? result)
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
