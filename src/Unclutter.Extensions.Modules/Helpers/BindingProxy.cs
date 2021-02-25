using System.Windows;

namespace Unclutter.Extensions.Modules.Helpers
{
    public class BindingProxy : Freezable
    {
        /* Fields */ 
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));

        /* Properties */
        public object Data
        {
            get => GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        /* Methods */
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }
    }
}
