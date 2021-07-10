using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Unclutter.SDK;
using Unclutter.SDK.Data;
using Unclutter.SDK.Models;
using Unclutter.SDK.Services;
using Unclutter.SDK.Plugins;
using Unclutter.SDK.Progress;
using Unclutter.Services.Games;

namespace Unclutter.Services.Data
{
    public class AppDbProvider : IAppDbProvider
    {
        /* Fields */
        private readonly IGamesProvider _gamesProvider;
        private readonly IJsonService _jsonService;
        private readonly ILocalizationProvider _localizationProvider;
        private readonly IDatabaseProvider _dbProvider;
        private bool _initialized;

        /* Events */
        public event Action<ProgressReport> ProgressChanged;

        /* Properties */
        public double? Order { get; set; }
        public LoadOptions LoaderOptions => new LoadOptions();

        /* Constructor */
        public AppDbProvider(ISqliteDatabaseFactory factory,
            IGamesProvider gamesProvider,
            IJsonService jsonService,
            ILocalizationProvider localizationProvider)
        {
            _gamesProvider = gamesProvider;
            _jsonService = jsonService;
            _localizationProvider = localizationProvider;
            _dbProvider = factory.CreateOrGet(LocalIdentifiers.Database.App);
        }

        /* Methods */
        public Task Load()
        {
            if (_initialized) return Task.CompletedTask;

            using var connection = _dbProvider.GetConnection();
            // Create the database required tables
            // Order of Table creation is important.
            connection.Execute(SqliteScripts.Table.Game.Create);
            connection.Execute(SqliteScripts.Table.GameCategory.Create);

            // check if the table is populated.
            var sql = @"SELECT * FROM Game LIMIT 10";

            var games = connection.Query<GameDetails>(sql);

            if (!games.Any())
            {
                OnProgressChanged(new ProgressReport(_localizationProvider.GetLocalizedString(ResourceKeys.Games_Preparing)));
                _gamesProvider.Save(GetEmbeddedGames());
            }

            // Order of Table creation is important.
            connection.Execute(SqliteScripts.Table.User.Create);
            connection.Execute(SqliteScripts.Table.Profile.Create);

            _initialized = true;
            return Task.CompletedTask;
        }
        public IDbConnection GetConnection()
        {
            return _dbProvider.GetConnection();
        }

        /* Helpers */
        private void OnProgressChanged(ProgressReport report)
        {
            ProgressChanged?.Invoke(report);
        }
        private IEnumerable<IGameDetails> GetEmbeddedGames()
        {
            // use the Embedded resource file "games.json"
            var resourceName = Assembly.GetExecutingAssembly()
                .GetManifestResourceNames()
                .First(r => r.EndsWith("games.json"));

            using var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream(resourceName);

            using var reader = new StreamReader(stream ?? throw new InvalidOperationException("Couldn't find the embedded resource file games.json"));
            var json = reader.ReadToEnd();
            var games = _jsonService.Deserialize<GameDetails[]>(json);
            return games;
        }
    }
}
