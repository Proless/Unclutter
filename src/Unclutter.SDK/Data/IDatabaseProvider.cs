using System;
using System.Data;

namespace Unclutter.SDK.Data
{
    public interface IDatabaseProvider
    {
        IDbConnection GetConnection();
        void TransactionalSqlCommand(Action<IDbConnection, IDbTransaction> process);
        T TransactionalSqlQuery<T>(Func<IDbConnection, IDbTransaction, T> process);
    }
}
