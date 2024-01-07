using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SystemManager.Data.Processes.Converters
{
    public class AutoRefreshIconKindConverter : IValueConverter
    {

        //  METHODS

        //  --------------------------------------------------------------------------------
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = value as bool?;

            if (state.HasValue)
                return state.Value ? PackIconKind.CheckboxOutline : PackIconKind.CheckBoxOutlineBlank;

            return PackIconKind.Play;
        }

        //  --------------------------------------------------------------------------------
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
