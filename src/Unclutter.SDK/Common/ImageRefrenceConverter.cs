using System;
using System.ComponentModel;
using System.Globalization;

namespace Unclutter.SDK.Common
{
    /// <summary>
    /// Converts a <see cref="string"/> to an <see cref="ImageReference"/> or vice versa
    /// </summary>
    public sealed class ImageReferenceConverter : TypeConverter
    {

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
            sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);


        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) =>
            destinationType == typeof(string) || base.CanConvertTo(context, destinationType);

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string s)
            {
                if (ImageReference.TryParse(s, out var imageReference))
                    return imageReference;

            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is ImageReference)
                return ((ImageReference)value).ToString();
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
