using MahApps.Metro.Controls;
using Prism.Services.Dialogs;

namespace Unclutter
{
    /// <summary>
    /// Interaction logic for MetroDialogWindow.xaml
    /// </summary>
    public partial class MetroDialogWindow : MetroWindow, IDialogWindow
    {
        public IDialogResult Result { get; set; }
        public MetroDialogWindow()
        {
            InitializeComponent();
        }
    }
}
