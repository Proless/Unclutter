using System.Threading;

namespace Unclutter.SDK.Progress
{
    public interface IProgressController
    {
        CancellationTokenSource CancellationTokenSource { get; set; }
        void SetCancelable(bool cancelable);
        void SetIndeterminate(bool indeterminate);
        void SetMessage(string text);
        void SetProgress(double progress);
        void SetTitle(string title);
    }
}
