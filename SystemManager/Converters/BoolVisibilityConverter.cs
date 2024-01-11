using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SystemManager.Converters
{
    public class BoolVisibilityConverter : IValueConverter
    {

        //  METHODS

        //  --------------------------------------------------------------------------------
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var hideOption = parameter is string option && option.ToLower() == "hide";
            var visible = value as bool?;

            if (!visible.HasValue)
                return hideOption ? Visibility.Hidden : Visibility.Collapsed;

            return visible.Value
                ? Visibility.Visible
                : hideOption
                    ? Visibility.Hidden
                    : Visibility.Collapsed;
        }

        //  --------------------------------------------------------------------------------
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visible = value as Visibility?;

            if (!visible.HasValue)
                return false;

            switch (visible.Value)
            {
                case Visibility.Visible:
                    return true;

                case Visibility.Collapsed:
                    return false;

                case Visibility.Hidden:
                    return false;
            }

            return false;
        }

    }
}
