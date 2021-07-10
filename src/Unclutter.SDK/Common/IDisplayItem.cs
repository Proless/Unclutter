using Unclutter.SDK.Images;

namespace Unclutter.SDK.Common
{
    public interface IDisplayItem
    {
        string Label { get; }
        string Hint { get; }
        ImageReference Icon { get; }
    }
}
