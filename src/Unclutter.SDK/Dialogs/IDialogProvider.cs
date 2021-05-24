namespace Unclutter.SDK.Dialogs
{
    public interface IDialogProvider
    {
        IMessageDialogConfig Message(string title, string message);
        ITaskDialogConfig Task(string title, string message);
    }
}
