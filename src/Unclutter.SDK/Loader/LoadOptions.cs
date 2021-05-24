using Unclutter.SDK.Common;

namespace Unclutter.SDK.Loader
{
    public class LoadOptions
    {
        public bool AutoLoad { get; set; } = true;
        public ThreadOption LoadThread { get; set; } = ThreadOption.BackgroundThread;
    }
}
