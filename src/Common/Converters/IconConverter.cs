using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TVPlayer.Common.Converters
{
    public class IconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string iconKey)
            {
                return Application.Current.FindResource(iconKey);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
