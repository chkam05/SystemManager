using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SystemManager.Converters
{
    public class StringIntConverter : IValueConverter
    {

        //  METHODS

        //  --------------------------------------------------------------------------------
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var number = (value as int?) ?? 0;

            return number.ToString();
        }

        //  --------------------------------------------------------------------------------
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strNumber = value as string;
            var defNumber = (parameter as int?) ?? 0;

            return int.TryParse(strNumber, out int number)
                ? number : defNumber;
        }

    }
}
