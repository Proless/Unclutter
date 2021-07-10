using MahApps.Metro.Controls;
using Prism.Services.Dialogs;

namespace Unclutter.CoreExtensions
{
    /// <summary>
    /// Interaction logic for StartupDialogHostWindow.xaml
    /// </summary>
    public partial class StartupDialogHostWindow : MetroWindow, IDialogWindow
    {
        public IDialogResult Result { get; set; }
        object IDialogWindow.Content
        {
            get => ContentHost.Content;
            set => ContentHost.Content = value;
        }
        public StartupDialogHostWindow()
        {
            InitializeComponent();
            Loaded += (_, _) => Activate();
        }
    }
}
