using Unclutter.SDK.Common;

namespace Unclutter.SDK.Events
{
    public class HandlerOptions
    {
        public bool AutoSubscribeToEvents { get; set; } = true;
        public ThreadOption AutoSubscribeThread { get; set; } = ThreadOption.Default;
    }
}
