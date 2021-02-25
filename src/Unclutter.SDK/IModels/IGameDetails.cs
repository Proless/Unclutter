using System;
using System.Collections.Generic;

namespace Unclutter.SDK.IModels
{
    public interface IGameDetails
    {
        long Id { get; }
        string Name { get; }
        string DomainName { get; }
        long Downloads { get; }
        long Mods { get; }
        Uri ForumUrl { get; set; }
        Uri NexusmodsUrl { get; set; }
        string Genre { get; set; }
        long FileCount { get; set; }
        DateTimeOffset? ApprovedDate { get; set; }
        long FileViews { get; set; }
        long Authors { get; set; }
        long FileEndorsements { get; set; }
        IEnumerable<IGameCategory> Categories { get; }
    }
}
