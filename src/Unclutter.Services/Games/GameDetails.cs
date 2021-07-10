using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Unclutter.API.Converters;
using Unclutter.SDK.Images;
using Unclutter.SDK.Models;

namespace Unclutter.Services.Games
{
    public class GameDetails : IGameDetails
    {
        public GameDetails() { }

        public GameDetails(IGameDetails gameDetails)
        {
            Id = gameDetails.Id;
            Downloads = gameDetails.Downloads;
            Mods = gameDetails.Mods;
            ForumUrl = gameDetails.ForumUrl;
            NexusModsUrl = gameDetails.NexusModsUrl;
            Genre = gameDetails.Genre;
            FileCount = gameDetails.FileCount;
            ApprovedDate = gameDetails.ApprovedDate;
            FileViews = gameDetails.FileViews;
            Authors = gameDetails.Authors;
            FileEndorsements = gameDetails.FileEndorsements;
            Name = gameDetails.Name;
            DomainName = gameDetails.DomainName;
            Categories = gameDetails.Categories.Select(c => new GameCategory(c)).ToList();
            Image = gameDetails.Image;
        }


        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("downloads")]
        public long Downloads { get; set; }

        [JsonPropertyName("mods")]
        public long Mods { get; set; }

        [JsonPropertyName("forum_url")]
        public Uri ForumUrl { get; set; }

        [JsonPropertyName("nexusmods_url")]
        public Uri NexusModsUrl { get; set; }

        [JsonPropertyName("genre")]
        public string Genre { get; set; }

        [JsonPropertyName("file_count")]
        public long FileCount { get; set; }

        [JsonPropertyName("approved_date")]
        [JsonConverter(typeof(GameApprovedDateConverter))]
        public DateTimeOffset? ApprovedDate { get; set; }

        [JsonPropertyName("file_views")]
        public long FileViews { get; set; }

        [JsonPropertyName("authors")]
        public long Authors { get; set; }

        [JsonPropertyName("file_endorsements")]
        public long FileEndorsements { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("domain_name")]
        public string DomainName { get; set; }

        [JsonPropertyName("categories")]
        public List<GameCategory> Categories { get; set; }

        [JsonIgnore]
        IEnumerable<IGameCategory> IGameDetails.Categories => Categories;

        [JsonIgnore]
        public ImageReference Image { get; set; }
    }
}
