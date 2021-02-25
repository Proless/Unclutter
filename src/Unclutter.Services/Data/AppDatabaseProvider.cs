using Dapper;
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using Unclutter.SDK.IServices;

namespace Unclutter.Services.Data
{
    public class AppDatabaseProvider : IAppDatabaseProvider
    {
        /* Services */
        private readonly IDirectoryService _directoryService;

        /* Fields */
        private readonly string _dbFile;
        private bool _isDisposed;
        private SQLiteConnection _cachedConnection;

        /* Constructors */
        public AppDatabaseProvider(IDirectoryService directoryService)
        {
            _directoryService = directoryService;
            _dbFile = Path.Combine(directoryService.DataDirectory, "app.db");
            Initialize();
        }

        /* Methods */
        public IDbConnection GetConnection()
        {
            var connectionStringBuilder = new SQLiteConnectionStringBuilder
            {
                DataSource = _dbFile,
                ForeignKeys = true,
                Version = 3,
                DateTimeFormat = SQLiteDateFormats.UnixEpoch,
                Flags = SQLiteConnectionFlags.Default | SQLiteConnectionFlags.AllowNestedTransactions
            };

            if (_cachedConnection is null || _isDisposed)
            {
                if (_cachedConnection != null) _cachedConnection.Disposed -= OnConnectionDisposed;
                _cachedConnection = new SQLiteConnection(connectionStringBuilder.ToString());
                _cachedConnection.Disposed -= OnConnectionDisposed;
                _cachedConnection.Disposed += OnConnectionDisposed;
            }

            return _cachedConnection;
        }

        private void OnConnectionDisposed(object sender, EventArgs e)
        {
            _isDisposed = true;
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
        }

        public T TransactionalSqlQuery<T>(Func<IDbConnection, IDbTransaction, T> process)
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
            return result;
        }

        /* Helpers */
        private void Initialize()
        {
            _directoryService.EnsureDirectoryAccess(Path.GetDirectoryName(_dbFile));
            if (!File.Exists(_dbFile))
                SQLiteConnection.CreateFile(_dbFile);

            // Make sure that all required tables are created
            CreateTables();
        }

        private void CreateTables()
        {
            var dbConnection = GetConnection();
            // Order of Table creation is important !
            dbConnection.Execute(SqliteScripts.Table.Game.Create);
            dbConnection.Execute(SqliteScripts.Table.GameCategory.Create);
            dbConnection.Execute(SqliteScripts.Table.User.Create);
            dbConnection.Execute(SqliteScripts.Table.Profile.Create);
        }
    }
}