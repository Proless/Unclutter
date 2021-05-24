using System;
using System.Data;
using System.Data.Common;
using Unclutter.SDK.Data;

namespace Unclutter.Services.Data
{
    public class DatabaseProvider : IDatabaseProvider
    {
        /* Fields */
        private bool _isDisposed;
        private DbConnection _cachedConnection;
        private readonly string _connectionString;
        private readonly Func<string, DbConnection> _dbConnectionFactory;

        /* Constructor */
        public DatabaseProvider(string connectionString, Func<string, DbConnection> dbConnectionFactory)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _dbConnectionFactory = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
        }

        /* Methods */
        public IDbConnection GetConnection()
        {
            if (_cachedConnection is null || _isDisposed)
            {
                if (_cachedConnection != null) _cachedConnection.Disposed -= OnConnectionDisposed;
                _cachedConnection = _dbConnectionFactory.Invoke(_connectionString);
                _cachedConnection.Disposed += OnConnectionDisposed;
            }

            return _cachedConnection;
        }

        public void TransactionalSqlCommand(Action<IDbConnection, IDbTransaction> process)
        {
            var db = GetConnection();

            if (db.State == ConnectionState.Closed)
            {
                db.Open();
            }

            using var transaction = db.BeginTransaction();
            try
            {
                process.Invoke(db, transaction);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
            //finally
            //{
            //    db.Close();
            //    db.Dispose();
            //}
        }

        public T TransactionalSqlQuery<T>(Func<IDbConnection, IDbTransaction,T> process)
        {
            T result = default;
            var db = GetConnection();

            if (db.State == ConnectionState.Closed)
            {
                db.Open();
            }

            using var transaction = db.BeginTransaction();
            try
            {
                result = process.Invoke(db, transaction);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
            //finally
            //{
            //    db.Close();
            //    db.Dispose();
            //}
            return result;
        }

        /* Helpers */
        private void OnConnectionDisposed(object sender, EventArgs e)
        {
            _isDisposed = true;
        }
    }
}
