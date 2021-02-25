using System;
using Newtonsoft.Json;
using Unclutter.SDK.IModels;

namespace Unclutter.Services.Profiles
{
    public class UserDetails : IUserDetails
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Uri ProfileUrl { get; set; }
        public bool IsSupporter { get; set; }
        public bool IsPremium { get; set; }
        [JsonIgnore]
        public object ImageSource { get; set; }
    }
}
