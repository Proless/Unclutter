using Unclutter.SDK.Dialogs;
using Unclutter.Services.Dialogs.Message;
using Unclutter.Services.Dialogs.Task;

namespace Unclutter.Services.Dialogs
{
    public class DialogProvider : IDialogProvider
    {
        /* Methods */
        public IMessageDialogConfig Message(string title, string message)
        {
            var config = new MessageDialogConfig();

            config.Title(title);
            config.Message(message);

            return config;
        }
        public ITaskDialogConfig Task(string title, string message)
        {
            var config = new TaskDialogConfig();

            config.Title(title);
            config.Message(message);

            return config;
        }
    }
}
