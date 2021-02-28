using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Unclutter.SDK.Dialogs;
using Unclutter.SDK.IServices;

namespace Unclutter.Services.WPF.Dialogs
{
    public class DialogProvider : IDialogProvider
    {
        /* Methods */
        public IDialogConfig Information(string title, string message)
        {
            return InternalCreateDialogConfig(DialogType.Information, title, message);
        }
        public IDialogConfig Warning(string title, string message)
        {
            return InternalCreateDialogConfig(DialogType.Warning, title, message);
        }
        public IDialogConfig Error(string title, string message)
        {
            return InternalCreateDialogConfig(DialogType.Error, title, message);
        }
        public IDialogConfig Question(string title, string message)
        {
            return InternalCreateDialogConfig(DialogType.Question, title, message);
        }
        public ITaskDialogConfig Task(string title, string message)
        {
            var newTaskDialog = new TaskDialog(new TaskDialogWindow())
            {
                Type = DialogType.Task,
                Title = title,
                Text = message
            };

            SetIcon(newTaskDialog);

            var newTaskDialogConfig = new TaskDialogConfig(newTaskDialog);

            return newTaskDialogConfig;
        }

        /* Helpers */
        private IDialogConfig InternalCreateDialogConfig(DialogType type, string title, string message)
        {
            var newDialog = new Dialog(new MessageDialogWindow())
            {
                Type = type,
                Title = title,
                Text = message
            };

            SetIcon(newDialog);

            var newDialogConfig = new DialogConfig(newDialog);

            return newDialogConfig;
        }
        private void SetIcon(BaseDialog dialog)
        {
            var img = new Image
            {
                Source = GetImageSource(dialog.Type)
            };

            dialog.Icon = img;
        }
        private ImageSource GetImageSource(DialogType dialogType)
        {
            var dlgIconId = dialogType switch
            {
                DialogType.Information => "information.png",
                DialogType.Question => "question.png",
                DialogType.Warning => "warning.png",
                DialogType.Error => "error.png",
                DialogType.Task => "task.png",
                _ => "information.png"
            };

            var resourceName = Assembly.GetExecutingAssembly()
                .GetManifestResourceNames()
                .First(r => r.EndsWith(dlgIconId));

            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

            var source = new BitmapImage();
            source.BeginInit();
            source.DecodePixelHeight = 48;
            source.DecodePixelWidth = 48;
            source.StreamSource = stream;
            source.EndInit();
            source.Freeze();

            return source;
        }
    }
}
