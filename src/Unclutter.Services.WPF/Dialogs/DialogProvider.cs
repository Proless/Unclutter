using Prism.Services.Dialogs;
using Unclutter.SDK.Dialogs;
using Unclutter.SDK.IServices;

namespace Unclutter.Services.WPF.Dialogs
{
    public class DialogProvider : IDialogProvider
    {
        /* Fields */
        private readonly IDialogService _dialogService;

        /* Properties */
        public string MessageDialogId { get; set; }
        public string TaskDialogId { get; set; }

        /* Constructor */
        public DialogProvider(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

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
            var newTaskDialog = new TaskDialog
            {
                Type = DialogType.Task,
                Title = title,
                Text = message
            };

            SetIcon(newTaskDialog);

            var newTaskDialogConfig = new TaskDialogConfig(newTaskDialog);
            newTaskDialogConfig.Created += OnDialogCreated;   // TODO: unsubscribe when no longer needed !, memory leak potential ?

            return newTaskDialogConfig;
        }

        /* Helpers */
        private IDialogConfig InternalCreateDialogConfig(DialogType type, string title, string message)
        {
            var newDialog = new Dialog
            {
                Type = type,
                Title = title,
                Text = message
            };

            SetIcon(newDialog);

            var newDialogConfig = new DialogConfig(newDialog);
            newDialogConfig.Created += OnDialogCreated; // TODO: unsubscribe when no longer needed !, memory leak potential ?

            return newDialogConfig;
        }
        private void OnDialogCreated(BaseDialog dialog)
        {
            dialog.RequestOpen += OnRequestOpen;
        }
        private void OnRequestOpen(BaseDialog dialog, OpenMode mode)
        {

        }
        private void SetIcon(BaseDialog baseDialog)
        {
        }
    }
}
