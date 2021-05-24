using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using Unclutter.SDK.Data;
using Unclutter.SDK.IServices;
using Unclutter.Services.Converters;

namespace Unclutter.Services.Data
{
    public class SqliteDatabaseFactory : ISqliteDatabaseFactory
    {
        /* Fields */
        private readonly IDirectoryService _directoryService;
        private readonly Dictionary<string, IDatabaseProvider> _databaseProviders;

        /* Constructor */
        public SqliteDatabaseFactory(IDirectoryService directoryService)
        {
            _directoryService = directoryService;
            _databaseProviders = new Dictionary<string, IDatabaseProvider>();

            Dapper.SqlMapper.AddTypeHandler(new UriTypeHandler());
            Dapper.SqlMapper.RemoveTypeMap(typeof(DateTimeOffset));
            Dapper.SqlMapper.AddTypeHandler(new DateTimeOffsetTypeHandler());
        }

        /* Methods */
        public IDatabaseProvider CreateOrGet(string name)
        {
            if (_databaseProviders.TryGetValue(name, out var existingDatabaseProvider))
            {
                return existingDatabaseProvider;
            }

            var dbFile = GetDatabaseFile(name);
            if (!File.Exists(dbFile))
            {
                SQLiteConnection.CreateFile(dbFile);
            }

            var newDatabaseProvider = new DatabaseProvider(GetConnectionString(dbFile), GetConnection);
            _databaseProviders[name] = newDatabaseProvider;
            return newDatabaseProvider;
        }

        /* Helpers */
        private DbConnection GetConnection(string connectionString)
        {
            return new SQLiteConnection(connectionString);
        }

        private string GetConnectionString(string dbFile)
        {
            var connectionStringBuilder = new SQLiteConnectionStringBuilder
            {
                DataSource = dbFile,
                ForeignKeys = true,
                Version = 3,
                DateTimeFormat = SQLiteDateFormats.UnixEpoch,
                Flags = SQLiteConnectionFlags.Default | SQLiteConnectionFlags.AllowNestedTransactions,
                DefaultIsolationLevel = IsolationLevel.Serializable
            };

            return connectionStringBuilder.ToString();
        }

        private string GetDatabaseFile(string name)
        {
            return Path.Combine(_directoryService.DataDirectory, $"{name}.db3");
        }
    }
}
