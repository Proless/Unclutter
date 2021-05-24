using Prism.Services.Dialogs;
using System.Windows;

namespace Unclutter.Initialization
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window, IDialogWindow
    {
        public IDialogResult Result { get; set; }

        object IDialogWindow.Content
        {
            get => ContentHost.Content;
            set => ContentHost.Content = value;
        }

        public DialogWindow()
        {
            InitializeComponent();
            Loaded += (_, _) => Activate();
            CanClose = true;
        }

        public bool CanClose
        {
            get => (bool)GetValue(CanCloseProperty);
            set => SetValue(CanCloseProperty, value);
        }

        public static readonly DependencyProperty CanCloseProperty =
            DependencyProperty.Register(nameof(CanClose), typeof(bool), typeof(DialogWindow));

        /* Event Handlers */
        private void OnCloseButtonClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
