using System;
using System.Data;
using static Dapper.SqlMapper;

namespace Unclutter.Services.Converters
{
    public class UriTypeHandler : TypeHandler<Uri>

    {
        public override void SetValue(IDbDataParameter parameter, Uri value)
        {
            parameter.Value = value.ToString();
        }

        public override Uri Parse(object value)
        {
            if (!(value is string str)) return null;
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
