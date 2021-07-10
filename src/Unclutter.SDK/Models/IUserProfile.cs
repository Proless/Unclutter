using System.Collections.Generic;
using Unclutter.SDK.Plugins;

namespace Unclutter.SDK.Models
{
    public interface IUserProfile
    {
        int Id { get; }
        string Name { get; }
        string DownloadsDirectory { get; }
        IGameDetails Game { get; }
        IUserDetails User { get; }
        IEnumerable<ProfileDetail> Details { get; }
        bool IsValid();
    }
}
