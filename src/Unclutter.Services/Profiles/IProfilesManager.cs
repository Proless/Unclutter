using System.Collections.Generic;
using Unclutter.SDK.IModels;
using Unclutter.Services.Data;

namespace Unclutter.Services.Profiles
{
    public interface IProfilesManager : IDataRepository<IUserProfile, long>
    {
        IEnumerable<IUserProfile> EnumerateProfiles();
        void Save(IEnumerable<IUserProfile> profiles);
    }
}
