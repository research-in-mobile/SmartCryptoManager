using System;
using System.Globalization;
using Xamarin.Forms;

namespace SmartCryptoManager.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public Color TrueObject { set; get; }

        public Color FalseObject { set; get; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? TrueObject : FalseObject;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Color)value).Equals(TrueObject);
        }

    }
}