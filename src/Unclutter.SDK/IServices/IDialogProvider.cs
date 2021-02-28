using Unclutter.SDK.Dialogs;

namespace Unclutter.SDK.IServices
{
    public interface IDialogProvider
    {
        IDialogConfig Information(string title, string message);
        IDialogConfig Warning(string title, string message);
        IDialogConfig Error(string title, string message);
        IDialogConfig Question(string title, string message);
        ITaskDialogConfig Task(string title, string message);
    }
}
