using System.Text.Json.Serialization;
using Unclutter.SDK.IModels;
using Unclutter.Services.Games;

namespace Unclutter.Services.Profiles
{
    public class UserProfile : IUserProfile
    {
        public long Id { get; set; }

        public long GameId { get; set; }

        public long UserId { get; set; }

        public string Name { get; set; }

        public string DownloadsDirectory { get; set; }

        public GameDetails Game { get; set; }

        public UserDetails User { get; set; }

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
