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
    public class ProcessLoadStateIconKindConverter : IValueConverter
    {

        //  METHODS

        //  --------------------------------------------------------------------------------
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var param = parameter as ProcessLoadStateIconKindConverterParam?;
            var state = value as bool?;

            if (state.HasValue)
            {
                if (param.HasValue)
                {
                    switch (param)
                    {
                        case ProcessLoadStateIconKindConverterParam.RefreshButton:
                            return state.Value ? PackIconKind.Stop : PackIconKind.Refresh;

                        case ProcessLoadStateIconKindConverterParam.StatusBarIcon:
                            return state.Value ? PackIconKind.Refresh : PackIconKind.Memory;
                    }
                }
            }

            return PackIconKind.Refresh;
        }

        //  --------------------------------------------------------------------------------
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
