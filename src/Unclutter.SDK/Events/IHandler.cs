using System.Threading;
using System.Threading.Tasks;

namespace Unclutter.SDK.Events
{
    public interface IHandler
    {
        bool AutoSubscribe { get; }
    }

    public interface IHandler<in TParam> : IHandler
    {
        Task HandleAsync(TParam @event, CancellationToken cancellationToken);
    }
}
