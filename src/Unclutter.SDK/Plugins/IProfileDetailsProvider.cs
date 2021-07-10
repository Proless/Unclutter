using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Unclutter.SDK.Common;
using Unclutter.SDK.Images;
using Unclutter.SDK.Models;

namespace Unclutter.SDK.Plugins
{
    public interface IProfileDetailsProvider
    {
        /// <summary>
        /// Called for each profile to create detail entries.
        /// </summary>
        /// <param name="profile">The profile to display more information about.</param>
        /// <returns>true to display the information, otherwise false, this can be used to determine if the passed <see cref="IUserProfile"/> is relevant or not.</returns>
        bool PopulateDetails(IUserProfile profile);
        IEnumerable<ProfileDetail> Details { get; }
    }

    public class ProfileDetail : IDisplayItem
    {
        public string Label { get; set; }
        public string Hint { get; set; }
        public ImageReference Icon { get; set; }
    }

    [MetadataAttribute, AttributeUsage(AttributeTargets.Class)]
    public sealed class ExportProfileDetailsProviderAttribute : ExportAttribute
    {
        public ExportProfileDetailsProviderAttribute() : base(typeof(IProfileDetailsProvider))
        {
        }
    }
}
