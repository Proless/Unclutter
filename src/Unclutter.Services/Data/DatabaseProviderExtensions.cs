using System;
using System.Data;
using Unclutter.SDK.Data;

namespace Unclutter.Services.Data
{
    public static class DatabaseProviderExtensions
    {
        public static T TransactionalQuery<T>(this IDatabaseProvider dbProvider, Func<IDbConnection, IDbTransaction, T> query)
        {
            T result = default;

            if (query is null)
                return result;

            using var connection = dbProvider.GetConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                result = query(connection, transaction);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
            finally
            {
                connection.Close();
            }
            return result;
        }

        public static void TransactionalExecute(this IDatabaseProvider dbProvider, Action<IDbConnection, IDbTransaction> execute)
        {
            if (execute is null) return;

            using var connection = dbProvider.GetConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                execute(connection, transaction);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
            finally
            {
                connection.Close();
            }
        }

        public static void TransactionalExecuteLocked(this IDatabaseProvider dbProvider, Action<IDbConnection, IDbTransaction> execute)
        {
            lock (dbProvider)
            {
                if (execute is null) return;

                using var connection = dbProvider.GetConnection();

                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using var transaction = connection.BeginTransaction();

                try
                {
                    execute(connection, transaction);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
