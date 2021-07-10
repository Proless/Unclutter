using System.ComponentModel.Composition;

namespace Unclutter.SDK.Plugins
{
    public interface IPluginConsumer : IPartImportsSatisfiedNotification
    {
        ImportOptions Options { get; }
    }

    public class ImportOptions
    {
        public bool AutoImport { get; set; } = true;
    }
}
