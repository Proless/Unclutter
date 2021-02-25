using Unclutter.SDK.Dialogs;

namespace Unclutter.SDK.IServices
{
    public interface IDialogProvider
    {
        string MessageDialogId { get; set; }
        string TaskDialogId { get; set; }
        IDialogConfig Information(string title, string message);
        IDialogConfig Warning(string title, string message);
        IDialogConfig Error(string title, string message);
        IDialogConfig Question(string title, string message);
        ITaskDialogConfig Task(string title, string message);
    }
}
