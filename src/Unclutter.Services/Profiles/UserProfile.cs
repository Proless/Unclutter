using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Unclutter.SDK.Models;
using Unclutter.SDK.Plugins;
using Unclutter.Services.Games;

namespace Unclutter.Services.Profiles
{
    public class UserProfile : IUserProfile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int GameId { get; set; }
        public int UserId { get; set; }
        public string DownloadsDirectory { get; set; }
        public GameDetails Game { get; set; }
        public UserDetails User { get; set; }
        [JsonIgnore]
        public IEnumerable<ProfileDetail> Details { get; set; } = Enumerable.Empty<ProfileDetail>();
        [JsonIgnore]
        IGameDetails IUserProfile.Game => Game;
        [JsonIgnore]
        IUserDetails IUserProfile.User => User;
        public bool IsValid()
        {
            return Game != null
                   && User != null
                   && !string.IsNullOrWhiteSpace(DownloadsDirectory);
        }
    }
}
