namespace Unclutter.SDK.Events
{
    public interface IHandler
    {
        HandlerOptions HandlerOptions { get; }
    }

    public interface IHandler<in TEvent> : IHandler
    {
        void Handle(TEvent @event);
    }
}
