using System.Threading;
using System.Threading.Tasks;
using Unclutter.SDK.Common;

namespace Unclutter.Services.Dialogs.TaskDlg
{
    internal class TaskDialogProgressController : IProgressController
    {
        /* Fields */
        private readonly TaskDialog _taskDialog;

        /* Constructor */
        public TaskDialogProgressController(TaskDialog taskDialog)
        {
            _taskDialog = taskDialog;
        }

        /* Properties */
        public CancellationTokenSource CancellationTokenSource { get; set; }

        /* Methods */
        public void SetCancelable(bool cancelable)
        {
            _taskDialog.IsCancelable = cancelable;
        }

        public void SetIndeterminate(bool indeterminate)
        {
            _taskDialog.IsIndeterminate = indeterminate;
        }

        public void SetText(string text)
        {
            _taskDialog.Text = text;
        }

        public void SetProgress(double progress)
        {
            if (progress >= 0d || progress <= 100d)
            {
                SetIndeterminate(false);
                _taskDialog.ProgressValue = progress;
            }
        }

        public void SetTitle(string title)
        {
            _taskDialog.Title = title;
        }

        public Task CloseAsync()
        {
            _taskDialog.Close();
            return System.Threading.Tasks.Task.CompletedTask;
        }
    }
}