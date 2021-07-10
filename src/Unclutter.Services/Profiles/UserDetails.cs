using System;
using System.Text.Json.Serialization;
using Unclutter.SDK.Images;
using Unclutter.SDK.Models;

namespace Unclutter.Services.Profiles
{
    public class UserDetails : IUserDetails
    {
        public UserDetails() { }

        public UserDetails(IUserDetails user)
        {
            Id = user.Id;
            Key = user.Key;
            Name = user.Name;
            Email = user.Email;
            ProfileUri = user.ProfileUri;
            IsSupporter = user.IsSupporter;
            IsPremium = user.IsPremium;
            Image = user.Image;
        }

        public int Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Uri ProfileUri { get; set; }
        public bool IsSupporter { get; set; }
        public bool IsPremium { get; set; }
        [JsonIgnore]
        public ImageReference Image { get; set; }
    }
}
