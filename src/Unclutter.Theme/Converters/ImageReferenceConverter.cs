using System;
using System.Globalization;
using System.Windows.Data;
using Unclutter.SDK.Images;

namespace Unclutter.Theme.Converters
{
    public class ImageReferenceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ImageReference imageReference)
            {
                var imgOptions = new ImageOptions();
                if (parameter is string options)
                {
                    if (options.Contains(' '))
                    {
                        var values = options.Split(' ');
                        if (values.Length == 2 && int.TryParse(values[0], out var width) && int.TryParse(values[1], out var height))
                        {
                            imgOptions = new ImageOptions { Height = height, Width = width };
                        }
                    }
                    else
                    {
                        if (int.TryParse(options, out var dimension))
                        {
                            imgOptions = new ImageOptions { Height = dimension, Width = dimension };
                        }
                    }
                }

                return imageReference.GetImageObject(imgOptions);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
