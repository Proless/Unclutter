using System;
using System.Globalization;
using System.Windows.Data;

namespace Unclutter.Theme.Converters
{
    public class NumberHumanizerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                int iNum => Humanize(iNum),
                double dNum => Humanize(dNum),
                long lNum => Humanize(lNum),
                _ => value
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        private string Humanize(double number)
        {
            var mag = (int)(Math.Floor(Math.Log10(number)) / 3);
            var divisor = Math.Pow(10, mag * 3);

            var shortNumber = number / divisor;

            var suffix = mag switch
            {
                0 => string.Empty,
                1 => "k",
                2 => "m",
                3 => "b",
                _ => string.Empty
            };
            return shortNumber.ToString("N1") + suffix;
        }
    }
}
