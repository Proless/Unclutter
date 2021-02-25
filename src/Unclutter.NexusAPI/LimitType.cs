using Unclutter.NexusAPI.Exceptions;

namespace Unclutter.NexusAPI
{
    /// <summary>
    /// Represents the type of the limit that caused the <see cref="LimitsExceededException"/>
    /// </summary>
    public enum LimitType
	{
		/// <summary>
		/// The official API Limits
		/// </summary>
		API,
		/// <summary>
		/// The custom Limits set using the <see cref="IRateManager"/>
		/// </summary>
		Custom
	}
}
