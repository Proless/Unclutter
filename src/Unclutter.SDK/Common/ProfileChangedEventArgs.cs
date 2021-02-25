using System;
using Unclutter.SDK.IModels;

namespace Unclutter.SDK.Common
{
    public class ProfileChangedEventArgs : EventArgs
    {
        public IUserProfile OldProfile { get; }
        public IUserProfile NewProfile { get; }

        public ProfileChangedEventArgs(IUserProfile oldProfile, IUserProfile newProfile)
        {
            OldProfile = oldProfile;
            NewProfile = newProfile;
        }
    }
}
