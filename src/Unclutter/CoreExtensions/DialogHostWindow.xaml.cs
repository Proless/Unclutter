using Prism.Services.Dialogs;
using System.Windows;

namespace Unclutter.CoreExtensions
{
    /// <summary>
    /// Interaction logic for DialogHostWindow.xaml
    /// </summary>
    public partial class DialogHostWindow : Window, IDialogWindow
    {
        public IDialogResult Result { get; set; }
        public DialogHostWindow()
        {
            InitializeComponent();
            Loaded += (_, _) => Activate();
        }
    }
}
