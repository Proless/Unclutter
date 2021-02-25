using System;

namespace Unclutter.SDK.IModels
{
	public interface IUserDetails
	{
		long Id { get; }
		string Key { get; }
		string Name { get; }
		string Email { get; }
		Uri ProfileUrl { get; }
		bool IsSupporter { get; }
		bool IsPremium { get; }
	}
}
