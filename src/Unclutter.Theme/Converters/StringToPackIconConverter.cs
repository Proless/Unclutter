using MaterialDesignThemes.Wpf;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Unclutter.Theme.Converters
{
    public class StringToPackIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string iconReference || string.IsNullOrWhiteSpace(iconReference)) return null;

            var dimensions = 25;

            var useParameters = int.TryParse(parameter?.ToString(), out var dimension);

            if (useParameters)
            {
                dimensions = dimension;
            }

            try
            {
                var iconName = iconReference.Replace("-", "");
                var iconKind = Enum.Parse<PackIconKind>(iconName, true);
                return new PackIcon { Kind = iconKind, Height = dimensions, Width = dimensions };
            }
            catch (Exception)
            {
                return new PackIcon { Kind = PackIconKind.None };
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
