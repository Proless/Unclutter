using System.Threading;
using System.Threading.Tasks;

namespace Unclutter.SDK.Common
{
    public interface IProgressController
    {
        CancellationTokenSource CancellationTokenSource { get; set; }
        void SetCancelable(bool cancelable);
        void SetIndeterminate(bool indeterminate);
        void SetText(string text);
        void SetProgress(double progress);
        void SetTitle(string title);
        Task CloseAsync();
    }
}
