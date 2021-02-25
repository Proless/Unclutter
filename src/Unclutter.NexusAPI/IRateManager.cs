using System;
using Unclutter.NexusAPI.DataModels;
using Unclutter.NexusAPI.Exceptions;

namespace Unclutter.NexusAPI
{
    /// <summary>
    /// Manages the API Limits.
    /// </summary>
    public interface IRateManager
    {
        /// <summary>
        /// The current reached limits of the API.
        /// </summary>
        APILimits APILimits { get; }

        /// <summary>
        /// Gets or sets the max daily requests before throwing a <see cref="LimitsExceededException"/>
        /// <br/> The Limit defined by the API is 2500 Daily. Default is 2500.
        /// </summary>
        int CustomDailyLimit { get; set; }

        /// <summary>
        /// Gets or sets the max hourly requests before throwing a <see cref="LimitsExceededException"/>
        /// <br/> The Limit defined by the API is 100 Hourly. Default is 100.
        /// </summary>
        int CustomHourlyLimit { get; set; }

        /// <summary>
        /// Indicates that a Daily limit has been exceeded.
        /// </summary>
        event EventHandler<LimitType> DailyLimitsExceeded;

        /// <summary>
        /// Indicates that a Hourly limit has been exceeded.
        /// </summary>
        event EventHandler<LimitType> HourlyLimitsExceeded;

        /// <summary>
        /// Occurs when the limits are updated.
        /// </summary>
        event Action<APILimits> APILimitsChanged;


        public bool DailyLimitExceeded();
        public bool HourlyLimitExceeded();
        public bool CustomHourlyLimitExceeded();
        public bool CustomDailyLimitExceeded();

    }
}