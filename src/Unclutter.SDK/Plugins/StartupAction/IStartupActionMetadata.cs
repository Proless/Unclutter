using Unclutter.SDK.Metadata;

namespace Unclutter.SDK.Plugins.StartupAction
{
    public interface IStartupActionMetadata : IGroupItem
    {
        string Label { get; }
        string HintLabel { get; }
        string IconId { get; }
    }
}
