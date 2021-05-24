using System.Drawing;
using System.Windows;

namespace Unclutter.Theme.Controls
{
    public partial class CustomWindow : Window
    {
        public double TitleBarHeight
        {
            get => (double)GetValue(TitleBarHeightProperty);
            set => SetValue(TitleBarHeightProperty, value);
        }

        public Brush TitleBarBackground
        {
            get => (Brush)GetValue(TitleBarBackgroundProperty);
            set => SetValue(TitleBarBackgroundProperty, value);
        }

        public Brush TitleBarForeground
        {
            get => (Brush)GetValue(TitleBarForegroundProperty);
            set => SetValue(TitleBarForegroundProperty, value);
        }

        public HorizontalAlignment TitleAlignment
        {
            get => (HorizontalAlignment)GetValue(TitleAlignmentProperty);
            set => SetValue(TitleAlignmentProperty, value);
        }

        public DataTemplate IconTemplate
        {
            get => (DataTemplate)GetValue(IconTemplateProperty);
            set => SetValue(IconTemplateProperty, value);
        }

        public static readonly DependencyProperty IconTemplateProperty =
            DependencyProperty.Register(nameof(IconTemplate), typeof(DataTemplate), typeof(CustomWindow));

        public static readonly DependencyProperty TitleBarHeightProperty =
            DependencyProperty.Register(nameof(TitleBarHeight), typeof(double), typeof(CustomWindow));

        public static readonly DependencyProperty TitleAlignmentProperty =
           DependencyProperty.Register(nameof(TitleAlignment), typeof(HorizontalAlignment), typeof(CustomWindow));

        public static readonly DependencyProperty TitleBarBackgroundProperty =
            DependencyProperty.Register(nameof(TitleBarBackground), typeof(Brush), typeof(CustomWindow));

        public static readonly DependencyProperty TitleBarForegroundProperty =
            DependencyProperty.Register(nameof(TitleBarForeground), typeof(Brush), typeof(CustomWindow));

    }
}
