using System;
using Unclutter.SDK.Common;
using Unclutter.SDK.IModels;
using Unclutter.SDK.IServices;

namespace Unclutter.Services.Profiles
{
    public class ProfileProvider : IProfileProvider, IProfileInstanceProvider
    {
        /* Fields */
        private IUserProfile _current;

        /* Properties */
        public IUserProfile Current
        {
            get => _current;
            set
            {
                if (value == null) throw new NullReferenceException(nameof(Current));

                OnProfileChanged(_current, value);
                _current = value;
            }
        }

        /* Events */
        public event Action<ProfileChangedEventArgs> Changed;

        protected virtual void OnProfileChanged(IUserProfile oldProfile, IUserProfile newProfile)
        {
            if (oldProfile != newProfile)
            {
                Changed?.Invoke(new ProfileChangedEventArgs(oldProfile, newProfile));
            }
        }
    }
}
