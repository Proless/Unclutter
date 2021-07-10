using Dapper;
using System;
using System.Data;

namespace Unclutter.Services.Converters
{
    public class UriTypeHandler : SqlMapper.TypeHandler<Uri>

    {
        public override void SetValue(IDbDataParameter parameter, Uri value)
        {
            parameter.Value = value.ToString();
        }

        public override Uri Parse(object value)
        {
            if (value is not string str) return null;
            try
            {
                return new Uri(str);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
