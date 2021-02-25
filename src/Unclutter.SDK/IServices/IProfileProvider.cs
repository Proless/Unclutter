using System;
using Unclutter.SDK.Common;
using Unclutter.SDK.IModels;

namespace Unclutter.SDK.IServices
{
    public interface IProfileProvider
    {
        IUserProfile Current { get; }
        event Action<ProfileChangedEventArgs> Changed;
    }
}
