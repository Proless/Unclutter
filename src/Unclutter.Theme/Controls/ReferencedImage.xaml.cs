using System.Windows;
using System.Windows.Controls;
using Unclutter.SDK.Images;

namespace Unclutter.Theme.Controls
{
    /// <summary>
    /// Interaction logic for ReferencedImage.xaml
    /// </summary>
    public partial class ReferencedImage : UserControl
    {
        public ReferencedImage()
        {
            InitializeComponent();
            Root.DataContext = this;
        }

        /* Properties */
        public ImageReference ImageReference
        {
            get => (ImageReference)GetValue(ImageReferenceProperty);
            set => SetValue(ImageReferenceProperty, value);
        }
        public int ImageWidth
        {
            get => (int)GetValue(ImageWidthProperty);
            set => SetValue(ImageWidthProperty, value);
        }
        public int ImageHeight
        {
            get => (int)GetValue(ImageHeightProperty);
            set => SetValue(ImageHeightProperty, value);
        }

        public static readonly DependencyProperty ImageReferenceProperty =
            DependencyProperty.Register(nameof(ImageReference),
                typeof(ImageReference),
                typeof(ReferencedImage),
                new FrameworkPropertyMetadata(null, OnImageReferenceChanged));

        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register(nameof(ImageWidth),
                typeof(int),
                typeof(ReferencedImage),
                new FrameworkPropertyMetadata(0, OnImageWidthHeightChanged));

        public static readonly DependencyProperty ImageHeightProperty =
            DependencyProperty.Register(nameof(ImageHeight),
                typeof(int),
                typeof(ReferencedImage),
                new FrameworkPropertyMetadata(0, OnImageWidthHeightChanged));

        /* Static */
        private static void OnImageReferenceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var referencedImage = (ReferencedImage)d;
            referencedImage.LoadImage();
        }

        private static void OnImageWidthHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var referencedImage = (ReferencedImage)d;
            referencedImage.LoadImage();
        }

        /* Helpers */
        protected void LoadImage()
        {
            if (ImageReference == null || ImageReference.IsDefault)
            {
                ImageObjectHost.Content = null;
                ImageSourceHost.Source = null;
                ImageSourceHost.Visibility = Visibility.Collapsed;
                ImageObjectHost.Visibility = Visibility.Collapsed;
                Visibility = Visibility.Collapsed;
                return;
            }

            var options = new ImageOptions { Width = ImageWidth, Height = ImageHeight };
            if (ImageReference.HasImageSource)
            {
                ImageSourceHost.Visibility = Visibility.Visible;
                ImageObjectHost.Visibility = Visibility.Collapsed;
                Visibility = Visibility.Visible;
                var source = options.IsDefaultSize ? ImageReference.GetImageSource() : ImageReference.GetImageSource(options);
                ImageSourceHost.Source = source;
            }
            else
            {
                ImageSourceHost.Visibility = Visibility.Collapsed;
                ImageObjectHost.Visibility = Visibility.Visible;
                Visibility = Visibility.Visible;
                var content = options.IsDefaultSize ? ImageReference.GetImageObject() : ImageReference.GetImageObject(options);
                ImageObjectHost.Content = content;
            }
        }
    }
}
