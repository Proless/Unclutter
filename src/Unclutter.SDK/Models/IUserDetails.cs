using System;
using Unclutter.SDK.Images;

namespace Unclutter.SDK.Models
{
    public interface IUserDetails
    {
        int Id { get; }
        string Key { get; }
        string Name { get; }
        string Email { get; }
        Uri ProfileUri { get; }
        bool IsSupporter { get; }
        bool IsPremium { get; }
        ImageReference Image { get; }
    }
}
