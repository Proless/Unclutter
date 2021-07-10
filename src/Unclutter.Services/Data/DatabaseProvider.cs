using System;
using System.Data;
using System.Data.Common;
using Unclutter.SDK.Data;

namespace Unclutter.Services.Data
{
    public class DatabaseProvider : IDatabaseProvider
    {
        /* Fields */
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
            return _dbConnectionFactory.Invoke(_connectionString);
        }
    }
}
