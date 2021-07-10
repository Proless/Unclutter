using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using Unclutter.SDK.Data;
using Unclutter.SDK.Models;
using Unclutter.SDK.Services;
using Unclutter.Services.Data;
using Unclutter.Services.Images;

namespace Unclutter.Services.Games
{
    public class GamesProvider : IGamesProvider
    {
        /* Services */
        private readonly IDatabaseProvider _dbProvider;
        private readonly IJsonService _jsonService;
        private readonly IImageProvider _imageProvider;

        /* Constructors */
        public GamesProvider(
            ISqliteDatabaseFactory sqliteDatabaseFactory,
            IJsonService jsonService,
            IImageProvider imageProvider)
        {
            _dbProvider = sqliteDatabaseFactory.CreateOrGet(LocalIdentifiers.Database.App);
            _jsonService = jsonService;
            _imageProvider = imageProvider;
        }

        /* Methods */
        public IEnumerable<IGameDetails> EnumerateGames()
        {
            return ReadAll();
        }
        public void Save(IEnumerable<IGameDetails> games)
        {
            _dbProvider.TransactionalExecute((connection, transaction) =>
            {
                var selectGames = "SELECT * FROM Game WHERE Id = @Id";

                foreach (var game in games)
                {
                    connection.Execute(
                        connection.Query<GameDetails>(selectGames, new { game.Id }, transaction).Any()
                            ? SqliteScripts.Table.Game.Update
                            : SqliteScripts.Table.Game.Insert, game, transaction);

                    SaveCategories(connection, transaction, game);
                }
            });
        }

        #region CRUD
        public IEnumerable<IGameDetails> ReadAll()
        {
            return _dbProvider.TransactionalQuery((connection, transaction) =>
            {
                var cache = new Dictionary<int, GameDetails>();

                var sql = "SELECT * FROM Game g INNER JOIN GameCategory gc ON g.Id = gc.GameId";

                connection.Query<GameDetails, GameCategory, GameDetails>(
                        sql,
                        (gameEntry, category) => MapGameCategories(gameEntry, category, cache),
                        splitOn: "Id",
                        transaction: transaction)
                        .AsList();

                return cache.Values;
            });
        }
        public IGameDetails Read(long id)
        {
            return _dbProvider.TransactionalQuery((connection, transaction)
                => InternalRead(connection, transaction, id));
        }
        public IGameDetails Insert(IGameDetails entity)
        {
            if (entity is null) return null;

            _dbProvider.TransactionalExecute((connection, transaction)
                => InternalInsert(connection, transaction, entity));

            return entity;
        }
        public IGameDetails Update(IGameDetails entity)
        {
            if (entity is null) return null;

            _dbProvider.TransactionalExecute((connection, transaction)
                => InternalUpdate(connection, transaction, entity));

            return entity;
        }
        public IGameDetails Delete(IGameDetails entity)
        {
            if (entity is null) return null;

            _dbProvider.TransactionalExecute((connection, transaction)
                => InternalDelete(connection, transaction, entity));

            return entity;
        }
        #endregion

        #region InternalCRUD
        private IGameDetails InternalRead(IDbConnection connection, IDbTransaction transaction, long id)
        {
            var cache = new Dictionary<int, GameDetails>();

            var sql = "SELECT * FROM Game g INNER JOIN GameCategory gc ON g.Id = gc.GameId WHERE g.Id = @Id";

            connection.Query<GameDetails, GameCategory, GameDetails>(
                    sql,
                    (game, category) => MapGameCategories(game, category, cache),
                    new { Id = id },
                    splitOn: "Id",
                    transaction: transaction)
                .AsList();

            return cache.Values.FirstOrDefault();
        }
        private void InternalInsert(IDbConnection connection, IDbTransaction transaction, IGameDetails entity)
        {
            connection.Execute(SqliteScripts.Table.Game.Insert, entity, transaction);
            SaveCategories(connection, transaction, entity);
        }
        private void InternalUpdate(IDbConnection connection, IDbTransaction transaction, IGameDetails entity)
        {
            connection.Execute(SqliteScripts.Table.Game.Update, entity, transaction);
            SaveCategories(connection, transaction, entity);
        }
        private void InternalDelete(IDbConnection connection, IDbTransaction transaction, IGameDetails entity)
        {
            connection.Execute(SqliteScripts.Table.Game.Delete, new { entity.Id }, transaction);
        }
        #endregion

        #region Helpers
        private void Initialize()
        {
            _dbProvider.TransactionalExecute((connection, transaction) =>
            {
                // Create the database required tables
                // Order of Table creation is important.
                connection.Execute(SqliteScripts.Table.Game.Create, transaction: transaction);
                connection.Execute(SqliteScripts.Table.GameCategory.Create, transaction: transaction);

                // check if the table is populated.
                var sql = @"SELECT * FROM Game LIMIT 10";

                var games = connection.Query<GameDetails>(sql, transaction: transaction);

                if (games.Any()) return;

                Save(GetEmbeddedGames());
            });
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
        private void SaveCategories(IDbConnection connection, IDbTransaction transaction, IGameDetails game)
        {
            // Delete categories 
            connection.Execute("DELETE FROM GameCategory WHERE GameId = @GameId", new { GameId = game.Id }, transaction);

            // Insert categories
            foreach (var category in game.Categories)
            {
                connection.Execute(SqliteScripts.Table.GameCategory.Insert, new { category.Id, category.Name, category.ParentCategoryId, GameId = game.Id }, transaction);
            }
        }
        private GameDetails MapGameCategories(GameDetails game, GameCategory category, IDictionary<int, GameDetails> games)
        {
            if (!games.TryGetValue(game.Id, out var gameEntry))
            {
                gameEntry = game;
                gameEntry.Categories = new List<GameCategory>();
                gameEntry.Image = _imageProvider.GetImageFor(gameEntry);
                games.Add(gameEntry.Id, gameEntry);
            }

            gameEntry.Categories.Add(category);
            return gameEntry;
        }
        #endregion
    }
}
