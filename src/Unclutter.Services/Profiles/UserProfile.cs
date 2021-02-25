using Newtonsoft.Json;
using Unclutter.SDK.IModels;
using Unclutter.Services.Converters;
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

        [JsonConverter(typeof(TypeConverter<GameDetails, IGameDetails>))]
        public IGameDetails Game { get; set; }

        [JsonConverter(typeof(TypeConverter<UserDetails, IUserDetails>))]
        public IUserDetails User { get; set; }

        public bool IsValid()
        {
            return Game != null
                && User != null
                && !string.IsNullOrWhiteSpace(DownloadsDirectory);
        }
    }
}
