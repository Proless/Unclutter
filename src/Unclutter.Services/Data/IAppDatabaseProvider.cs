using System;
using System.Data;

namespace Unclutter.Services.Data
{
    public interface IAppDatabaseProvider : IDatabaseProvider
    {
        void TransactionalSqlCommand(Action<IDbConnection, IDbTransaction> process);
        T TransactionalSqlQuery<T>(Func<IDbConnection, IDbTransaction, T> process);
    }
}
