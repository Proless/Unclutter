using System.Data;

namespace Unclutter.SDK.Data
{
    public interface IDatabaseProvider
    {
        IDbConnection GetConnection();
    }
}
