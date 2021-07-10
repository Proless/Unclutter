using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Unclutter.Theme.Converters.Boolean
{
    public class BooleanConverter<T> : IValueConverter
    {
        public T TrueValue { get; protected set; }
        public T FalseValue { get; protected set; }

        public BooleanConverter(T trueValue, T falseValue)
        {
            TrueValue = trueValue;
            FalseValue = falseValue;
        }
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool boolean ? boolean ? TrueValue : FalseValue : value;
        }
        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is T tValue && EqualityComparer<T>.Default.Equals(tValue, TrueValue);
        }
    }
}
