using System.Windows;
using System.Windows.Media;
using Unclutter.SDK.Common;
using Unclutter.Services.Loader;

namespace Unclutter.Initialization
{
    /// <summary>
    /// Interaction logic for LoaderScreen.xaml
    /// </summary>
    public partial class LoaderScreen : Window, ILoaderScreen
    {
        /* Properties */
        public Brush DefaultTextForeground { get; }

        /* Constructor */
        public LoaderScreen()
        {
            InitializeComponent();
            Root.DataContext = this;
            MouseLeftButtonDown += (_, _) => DragMove();
            DefaultTextForeground = ProgressText.Foreground.Clone();
        }

        /* Helpers */
        private void SetProgressPercentage(ProgressReport report)
        {
            Dispatcher.Invoke(() =>
            {
                if (report.ProgressPercentage is > 0d and <= 100d)
                {
                    ProgressBar.IsIndeterminate = false;
                    ProgressBar.Value = report.ProgressPercentage;
                }
                else
                {
                    ProgressBar.IsIndeterminate = true;
                    ProgressBar.Value = 0d;
                }
            });
        }
        private void SetProgressText(ProgressReport report)
        {
            Dispatcher.Invoke(() =>
            {
                if (report.HasError || report.Status == OperationStatus.Error)
                {
                    ProgressTextBg.Background = new SolidColorBrush
                    {
                        Color = Colors.DimGray,
                        Opacity = 0.7
                    };
                    ProgressText.Foreground = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    ProgressTextBg.Background = null;
                    ProgressText.Foreground = DefaultTextForeground;
                }
                ProgressText.Text = report.Message;
            });
        }

        #region ILoaderScreen
        public void ReportProgress(ProgressReport report)
        {
            SetProgressText(report);
            SetProgressPercentage(report);
        }
        void ILoaderScreen.Show()
        {
            Dispatcher.Invoke(Show);
        }
        void ILoaderScreen.Close()
        {
            Dispatcher.Invoke(Close);
        }
        void ILoaderScreen.Hide()
        {
            Dispatcher.Invoke(Hide);
        }
        #endregion

        #region DPs
        public string Subtitle
        {
            get => (string)GetValue(SubtitleProperty);
            set => SetValue(SubtitleProperty, value);
        }
        public string Footer
        {
            get => (string)GetValue(FooterProperty);
            set => SetValue(FooterProperty, value);
        }
        public object Logo
        {
            get => GetValue(LogoProperty);
            set => SetValue(LogoProperty, value);
        }

        public static readonly DependencyProperty FooterProperty =
            DependencyProperty.Register(nameof(Footer), typeof(string), typeof(LoaderScreen));

        public static readonly DependencyProperty SubtitleProperty =
            DependencyProperty.Register(nameof(Subtitle), typeof(string), typeof(LoaderScreen));

        public static readonly DependencyProperty LogoProperty =
            DependencyProperty.Register(nameof(Logo), typeof(object), typeof(LoaderScreen));
        #endregion
    }
}
