using System;
using System.Collections.Generic;
using Unclutter.SDK.Images;

namespace Unclutter.SDK.Models
{
    public interface IGameDetails
    {
        int Id { get; }
        string Name { get; }
        string DomainName { get; }
        long Downloads { get; }
        long Mods { get; }
        Uri ForumUrl { get; set; }
        Uri NexusModsUrl { get; set; }
        string Genre { get; set; }
        long FileCount { get; set; }
        DateTimeOffset? ApprovedDate { get; set; }
        long FileViews { get; set; }
        long Authors { get; set; }
        long FileEndorsements { get; set; }
        IEnumerable<IGameCategory> Categories { get; }
        ImageReference Image { get; }
    }
}
