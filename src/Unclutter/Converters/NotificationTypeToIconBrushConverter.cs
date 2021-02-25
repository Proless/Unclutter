using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Unclutter.SDK.Notifications;

namespace Unclutter.Converters
{
    public class NotificationTypeToIconBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is NotificationType type)) return value;

            return type switch
            {
                NotificationType.Information => Brushes.Aqua,
                NotificationType.Warning => Brushes.GreenYellow,
                NotificationType.Error => Brushes.DarkRed,
                _ => Brushes.White
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
