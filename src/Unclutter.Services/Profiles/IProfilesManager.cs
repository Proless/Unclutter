#nullable enable
using System;
using System.Collections.Generic;
using Unclutter.SDK.Data;
using Unclutter.SDK.Models;

namespace Unclutter.Services.Profiles
{
    public interface IProfilesManager : IDataRepository<IUserProfile, long>
    {
        string ProfilesDirectory { get; }
        IUserProfile? CurrentProfile { get; }
        event Action<ProfileChangedArgs> ProfileChanged;
        IEnumerable<IUserProfile> EnumerateProfiles();
        IEnumerable<IUserDetails> EnumerateUsers();
        void Save(IEnumerable<IUserProfile> profiles);
        void LoadProfile(IUserProfile profile);
        IUserProfile Create(string name, string downloadsDirectory, IGameDetails game, IUserDetails user);
    }

    public class ProfileChangedArgs
    {
        public IUserProfile OldProfile { get; }
        public IUserProfile NewProfile { get; }

        public ProfileChangedArgs(IUserProfile newProfile, IUserProfile oldProfile)
        {
            NewProfile = newProfile;
            OldProfile = oldProfile;
        }
    }
}
