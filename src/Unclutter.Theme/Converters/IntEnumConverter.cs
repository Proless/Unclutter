using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Unclutter.Theme.Converters
{
    public class IntEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null || !value.GetType().IsEnum || !targetType.IsEnum)
            {
                return value;
            }

            var intValue = (int)value;
            var targetValue = targetType
                .GetEnumValues()
                .Cast<Enum>()
                .FirstOrDefault(e => System.Convert.ToInt32(e) == intValue);

            return targetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
