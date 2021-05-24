using Dapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Unclutter.SDK.Data;
using Unclutter.SDK.IModels;
using Unclutter.SDK.IServices;
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
        public GamesProvider(ISqliteDatabaseFactory sqliteDatabaseFactory, IJsonService jsonService, IImageProvider imageProvider)
        {
            _dbProvider = sqliteDatabaseFactory.CreateOrGet(LocalIdentifiers.Database.App);
            _jsonService = jsonService;
            _imageProvider = imageProvider;
            Initialize();
        }

        /* Methods */
        public IEnumerable<IGameDetails> EnumerateGames()
        {
            return ReadAll();
        }

        public void Save(IEnumerable<IGameDetails> games)
        {
            _dbProvider.TransactionalSqlCommand((db, transaction) =>
            {
                foreach (var game in games)
                {
                    if (ReadById(game.Id) is null)
                    {
                        Insert(game);
                    }
                    else
                    {
                        Update(game);
                    }
                }
            });
        }

        #region CRUD
        public IEnumerable<IGameDetails> ReadAll()
        {
            return _dbProvider.TransactionalSqlQuery<IEnumerable<IGameDetails>>((db, transaction) =>
            {
                // TODO: do inner join it is faster, but it makes the implementation more complicated.
                var selectGames = "SELECT * FROM Game";
                var selectCategories = "SELECT * FROM GameCategory WHERE GameId = @Id";
                var games = db.Query<GameDetails>(selectGames, transaction: transaction).AsList();
                foreach (var game in games)
                {
                    var categories = db.Query<GameCategory>(selectCategories, new { game.Id }, transaction);
                    game.Categories = categories;
                    game.ImageSource = _imageProvider.GetImageFor(game);
                }
                return games;
            });
        }

        public IGameDetails ReadById(long id)
        {
            return _dbProvider.TransactionalSqlQuery<IGameDetails>((db, transaction) =>
            {
                // TODO: do inner join.
                var selectGame = "SELECT * FROM Game WHERE Id = @Id";
                var selectCategories = "SELECT * FROM GameCategory WHERE GameId = @GameId";
                var game = db.QuerySingleOrDefault<GameDetails>(selectGame, new { Id = id }, transaction);

                if (game is null) return null;

                var categories = db.Query<GameCategory>(selectCategories, new { GameId = id }, transaction);
                game.Categories = categories;
                game.ImageSource = _imageProvider.GetImageFor(game);

                return game;
            });
        }

        public IGameDetails Insert(IGameDetails entity)
        {
            if (entity is null) return null;

            _dbProvider.TransactionalSqlCommand((db, transaction) =>
            {
                db.Execute(SqliteScripts.Table.Game.Insert, entity, transaction);
                foreach (var category in entity.Categories)
                {
                    db.Execute(SqliteScripts.Table.GameCategory.Insert,
                        new { category.Id, category.Name, category.ParentCategoryId, GameId = entity.Id }, transaction);
                }
            });
            return entity;
        }

        public IGameDetails Update(IGameDetails entity)
        {
            if (entity is null) return null;

            _dbProvider.TransactionalSqlCommand((db, transaction) =>
            {
                db.Execute(SqliteScripts.Table.Game.Update, entity, transaction);

                // Delete all categories 
                db.Execute("DELETE FROM GameCategory WHERE GameId = @GameId", new { GameId = entity.Id }, transaction);

                foreach (var category in entity.Categories)
                {
                    db.Execute(SqliteScripts.Table.GameCategory.Insert,
                        new { category.Id, category.Name, category.ParentCategoryId, GameId = entity.Id }, transaction);
                }
            });
            return entity;
        }

        public IGameDetails Delete(IGameDetails entity)
        {
            if (entity is null) return null;

            _dbProvider.TransactionalSqlCommand((db, transaction) =>
            {
                db.Execute(SqliteScripts.Table.Game.Delete, new { entity.Id }, transaction);
            });
            return entity;
        }
        #endregion

        /* Helpers */
        private void Initialize()
        {
            // Create database required tables
            _dbProvider.TransactionalSqlCommand((db, transaction) =>
            {
                // Order of Table creation is important.
                db.Execute(SqliteScripts.Table.Game.Create, transaction);
                db.Execute(SqliteScripts.Table.GameCategory.Create, transaction);
            });

            var sql = @"SELECT * FROM Game LIMIT 10";

            var dbConnection = _dbProvider.GetConnection();
            var games = dbConnection.Query<GameDetails>(sql);

            if (!games.Any())
                Save(GetEmbeddedGames());
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
