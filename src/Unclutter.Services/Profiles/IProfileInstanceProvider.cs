using Unclutter.SDK.IModels;

namespace Unclutter.Services.Profiles
{
    public interface IProfileInstanceProvider
    {
        IUserProfile Current { get; set; }
    }
}
