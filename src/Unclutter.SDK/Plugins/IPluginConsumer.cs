using System.ComponentModel.Composition;

namespace Unclutter.SDK.Plugins
{
    public interface IPluginConsumer : IPartImportsSatisfiedNotification
    {
        ImportOptions Options { get; }
    }
}
