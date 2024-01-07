using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SystemManager.Data.Macros.Converters
{
    public class MacroKeyCombinationNameConverter : IValueConverter
    {

        //  METHODS

        //  --------------------------------------------------------------------------------
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<byte> keyCodesList)
            {
                if (keyCodesList?.Any() ?? false)
                    return string.Join(" + ", keyCodesList.Select(k => MacroKeyNameConverter.GetKeyName(k)));
            }
            else if (value is byte[] keyCodesArray)
            {
                if (keyCodesArray?.Any() ?? false)
                    return string.Join(" + ", keyCodesArray.Select(k => MacroKeyNameConverter.GetKeyName(k)));
            }

            return string.Empty;
        }

        //  --------------------------------------------------------------------------------
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
