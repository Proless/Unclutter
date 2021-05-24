using System;
using System.Data;
using Dapper;

namespace Unclutter.Services.Converters
{
    public class DateTimeOffsetTypeHandler : SqlMapper.TypeHandler<DateTimeOffset>
    {
        public override void SetValue(IDbDataParameter parameter, DateTimeOffset value)
        {
            parameter.Value = value.ToUnixTimeSeconds();
        }

        public override DateTimeOffset Parse(object value)
        {
            if (value is not long seconds) return new DateTimeOffset();
            try
            {
                var time = DateTimeOffset.FromUnixTimeSeconds(seconds);
                return time;
            }
            catch (Exception)
            {
                return new DateTimeOffset();
            }
        }
    }
}
