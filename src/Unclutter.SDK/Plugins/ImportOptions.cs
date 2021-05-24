using Unclutter.SDK.Common;

namespace Unclutter.SDK.Plugins
{
    public class ImportOptions
    {
        public bool AutoImport { get; set; } = true;
        public ThreadOption ImportThread { get; set; } = ThreadOption.BackgroundThread;
    }

}
