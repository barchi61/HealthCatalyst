using System;
using System.Globalization;
using System.Windows.Data;

namespace PeopleSearch.Infrastructure.Behaviors
{
    [ValueConversion(typeof(object), typeof(bool))]
    public class NullToBoolValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = value == null ? false : true;
            if (parameter != null)
                return !result;
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

    }
}
