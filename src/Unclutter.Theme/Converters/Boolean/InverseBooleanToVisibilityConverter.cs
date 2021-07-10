using System.Windows;

namespace Unclutter.Theme.Converters.Boolean
{
    public sealed class InverseBooleanToVisibilityConverter : BooleanConverter<Visibility>
    {
        public InverseBooleanToVisibilityConverter() : base(Visibility.Collapsed, Visibility.Visible) { }
    }
}
