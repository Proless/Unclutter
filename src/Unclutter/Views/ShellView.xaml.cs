using MahApps.Metro.Controls;
using System.Windows;
using Unclutter.Modules.Plugins.AppWindowFlyout;
using Unclutter.SDK.Services;

namespace Unclutter.Views
{
    /// <summary>
    /// Interaction logic for ShellViewModel.xaml
    /// </summary>
    public partial class ShellView : MetroWindow
    {
        private readonly IEventAggregator _eventAggregator;

        public ShellView(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            InitializeComponent();
        }

        /* Event handlers */
        private void OnFlyoutOpened(object sender, RoutedEventArgs e)
        {
            if (sender is Flyout flyout && flyout.Tag is IAppWindowFlyout windowFlyout)
            {
                windowFlyout.OnOpened();
            }
        }
        private void OnFlyoutClosed(object sender, RoutedEventArgs e)
        {
            if (sender is Flyout flyout && flyout.Tag is IAppWindowFlyout windowFlyout)
            {
                windowFlyout.OnClosed();
            }
        }
    }
}
