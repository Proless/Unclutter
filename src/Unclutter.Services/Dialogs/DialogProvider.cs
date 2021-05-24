using Unclutter.SDK.Dialogs;
using Unclutter.Services.Dialogs.Message;
using Unclutter.Services.Dialogs.TaskDlg;

namespace Unclutter.Services.Dialogs
{
    public class DialogProvider : IDialogProvider
    {
        public IMessageDialogConfig Message(string title, string message)
        {
            var dialog = new MessageDialog
            {
                Title = title,
                Text = message
            };

            var config = new MessageDialogConfig(dialog);

            return config;
        }

        public ITaskDialogConfig Task(string title, string message)
        {
            var dialog = new TaskDialog
            {
                Title = title,
                Text = message
            };

            var config = new TaskDialogConfig(dialog);

            return config;
        }
    }
}
