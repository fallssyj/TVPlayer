using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TVPlayer.Common.Converters
{
    public class FavoriteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool && (bool)value)
                return Application.Current.FindResource("favorite1");
            else
                return Application.Current.FindResource("favorite");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
