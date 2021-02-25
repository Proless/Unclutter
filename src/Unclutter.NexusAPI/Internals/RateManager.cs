using System;
using Unclutter.NexusAPI.DataModels;

namespace Unclutter.NexusAPI.Internals
{
    internal class RateManager : IRateManager
    {
        /* Fields */
        private int _customDailyLimit;
        private int _customHourlyLimit;
        private APILimits _apiLimits;

        /* Properties */
        public APILimits APILimits
        {
            get => _apiLimits;
            set
            {
                _apiLimits = value ?? _apiLimits;
                APILimitsChanged?.Invoke(_apiLimits);
                CheckLimitsAndRaiseIfExceeded();
            }
        }

        public int CustomDailyLimit
        {
            get => _customDailyLimit;
            set
            {
                if (value > 2500 || value < 0)
                {
                    _customDailyLimit = 2500;
                }
                else
                {
                    _customDailyLimit = value;
                }

            }
        }
        public int CustomHourlyLimit
        {
            get => _customHourlyLimit;
            set
            {
                if (value > 100 || value < 0)
                {
                    _customHourlyLimit = 100;
                }
                else
                {
                    _customHourlyLimit = value;
                }
            }
        }

        /* Events */
        public event EventHandler<LimitType> DailyLimitsExceeded;
        public event EventHandler<LimitType> HourlyLimitsExceeded;
        public event Action<APILimits> APILimitsChanged;

        /* Constructor */
        internal RateManager()
        {
            APILimits = new APILimits();
            CustomDailyLimit = 2500;
            CustomHourlyLimit = 100;
        }

        /* Methods */
        public bool DailyLimitExceeded()
        {
            return APILimits.DailyRemaining <= 0;
        }
        public bool HourlyLimitExceeded()
        {
            return APILimits.HourlyRemaining <= 0;
        }
        public bool CustomHourlyLimitExceeded()
        {
            var customLimit = APILimits.HourlyLimit - APILimits.HourlyRemaining >= CustomHourlyLimit;
            return customLimit;
        }
        public bool CustomDailyLimitExceeded()
        {
            var customLimit = APILimits.DailyLimit - APILimits.DailyRemaining >= CustomDailyLimit;
            return customLimit;
        }

        /* Helpers */
        private void CheckLimitsAndRaiseIfExceeded()
        {
            if (HourlyLimitExceeded())
            {
                HourlyLimitsExceeded?.Invoke(this, LimitType.API);
                return;
            }

            if (CustomHourlyLimitExceeded())
            {
                HourlyLimitsExceeded?.Invoke(this, LimitType.Custom);
                return;
            }

            if (DailyLimitExceeded())
            {
                DailyLimitsExceeded?.Invoke(this, LimitType.API);
                return;
            }

            if (CustomDailyLimitExceeded())
            {
                DailyLimitsExceeded?.Invoke(this, LimitType.Custom);
            }
        }
    }
}
