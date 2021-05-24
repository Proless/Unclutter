using Unclutter.SDK.IModels;

namespace Unclutter.SDK.Events
{
    public class ProfileChangedEvent
    {
        public IUserProfile OldProfile { get; }
        public IUserProfile NewProfile { get; }

        public ProfileChangedEvent(IUserProfile newProfile, IUserProfile oldProfile)
        {
            NewProfile = newProfile;
            OldProfile = oldProfile;
        }
    }
}
