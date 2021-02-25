using System.Data;

namespace Unclutter.Services.Data
{
    public interface IDatabaseProvider
    {
        IDbConnection GetConnection();
    }
}
