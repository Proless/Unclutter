using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Unclutter.SDK.Data;
using Unclutter.SDK.Models;
using Unclutter.SDK.Plugins;
using Unclutter.SDK.Services;
using Unclutter.Services.Data;
using Unclutter.Services.Games;
using Unclutter.Services.Images;

namespace Unclutter.Services.Profiles
{
    public class ProfilesManager : IProfilesManager
    {
        /* Services */
        private readonly IDatabaseProvider _dbProvider;
        private readonly IImageProvider _imageProvider;
        private readonly IPluginProvider _pluginProvider;

        /* Properties */
        public string ProfilesDirectory { get; }
        public IUserProfile CurrentProfile { get; private set; }

        /* Events */
        public event Action<ProfileChangedArgs> ProfileChanged;

        /* Constructors */
        public ProfilesManager(
            ISqliteDatabaseFactory sqliteDatabaseFactory,
            IImageProvider imageProvider,
            IDirectoryService directoryService,
            IPluginProvider pluginProvider)
        {
            _dbProvider = sqliteDatabaseFactory.CreateOrGet(LocalIdentifiers.Database.App);
            _imageProvider = imageProvider;
            _pluginProvider = pluginProvider;

            ProfilesDirectory = Path.Combine(directoryService.DataDirectory, "profiles");
            CurrentProfile = null;

            directoryService.EnsureDirectoryAccess(ProfilesDirectory);
        }

        /* Methods */
        public IEnumerable<IUserProfile> EnumerateProfiles()
        {
            return ReadAll();
        }

        public IEnumerable<IUserDetails> EnumerateUsers()
        {
            return _dbProvider.TransactionalQuery((connection, transaction) =>
            {
                var output = new List<IUserDetails>();

                var selectUsers = "SELECT * FROM User";

                var users = connection
                    .Query<UserDetails>(selectUsers, transaction: transaction)
                    .AsList();

                foreach (var user in users)
                {
                    user.Image = _imageProvider.GetImageFor(user);
                    output.Add(user);
                }

                return output;
            });
        }

        public void Save(IEnumerable<IUserProfile> profiles)
        {
            if (profiles is null) return;

            _dbProvider.TransactionalExecuteLocked((connection, transaction) =>
            {
                foreach (var profile in profiles)
                {
                    if (InternalRead(connection, transaction, profile.Id) is null)
                    {
                        InternalInsert(connection, transaction, profile);
                    }
                    else
                    {
                        InternalUpdate(connection, transaction, profile);
                    }
                }
            });
        }

        public void LoadProfile(IUserProfile profile)
        {
            var args = new ProfileChangedArgs(profile, CurrentProfile);
            CurrentProfile = profile;
            ProfileChanged?.Invoke(args);
        }

        public IUserProfile Create(string name, string downloadsDirectory, IGameDetails game, IUserDetails user)
        {
            var profile = new UserProfile
            {
                Name = name,
                DownloadsDirectory = downloadsDirectory,
                Game = new GameDetails(game),
                User = new UserDetails(user),
                GameId = game.Id,
                UserId = user.Id
            };

            Save(new[] { profile });

            profile.Details = GetProfileDetails(profile);

            Directory.CreateDirectory(Path.Combine(ProfilesDirectory, profile.Name));

            return profile;
        }

        #region CRUD
        public IEnumerable<IUserProfile> ReadAll()
        {
            return _dbProvider.TransactionalQuery((connection, transaction) =>
            {
                var cache = new Dictionary<int, UserProfile>();

                var sql = @"SELECT * FROM Profile p INNER JOIN ""User"" u ON u.Id = p.UserId INNER JOIN Game g ON g.Id = p.GameId INNER JOIN GameCategory gc ON gc.GameId = g.Id";

                connection.Query<UserProfile, UserDetails, GameDetails, GameCategory, UserProfile>(
                        sql,
                        (profile, user, game, category) => MapProfileProps(profile, user, game, category, cache),
                        splitOn: "Id",
                        transaction: transaction)
                    .AsList();

                foreach (var profile in cache.Values)
                {
                    PopulateProfileDetails(profile);
                }

                return cache.Values;
            });
        }
        public IUserProfile Read(long id)
        {
            return _dbProvider.TransactionalQuery((connection, transaction)
                => InternalRead(connection, transaction, id));
        }
        public IUserProfile Insert(IUserProfile entity)
        {
            if (entity is null) return null;

            UserProfile result = null;

            _dbProvider.TransactionalExecuteLocked((connection, transaction) =>
            {
                result = InternalInsert(connection, transaction, entity);
            });

            PopulateProfileDetails(result);

            return result;
        }
        public IUserProfile Update(IUserProfile entity)
        {
            if (entity is null) return null;

            _dbProvider.TransactionalExecute((connection, transaction)
                => InternalUpdate(connection, transaction, entity));

            return entity;
        }
        public IUserProfile Delete(IUserProfile entity)
        {
            if (entity is null) return null;

            _dbProvider.TransactionalExecute((connection, transaction)
                => InternalDelete(connection, transaction, entity));

            return entity;
        }
        #endregion

        #region InternalCRUD
        private UserProfile InternalRead(IDbConnection connection, IDbTransaction transaction, long id)
        {
            var cache = new Dictionary<int, UserProfile>();

            var sql = @"SELECT * FROM Profile p INNER JOIN ""User"" u ON u.Id = p.UserId INNER JOIN Game g ON g.Id = p.GameId INNER JOIN GameCategory gc ON gc.GameId = g.Id WHERE p.Id = @Id";

            connection.Query<UserProfile, UserDetails, GameDetails, GameCategory, UserProfile>(
                     sql,
                     (profile, user, game, category) => MapProfileProps(profile, user, game, category, cache),
                     new { Id = id },
                     splitOn: "Id",
                     transaction: transaction)
                 .AsList();

            var result = cache.Values.FirstOrDefault();

            PopulateProfileDetails(result);

            return result;
        }
        private UserProfile InternalInsert(IDbConnection connection, IDbTransaction transaction, IUserProfile entity)
        {
            SaveUser(connection, transaction, entity.User);

            SaveGame(connection, transaction, entity.Game);

            connection.Execute(SqliteScripts.Table.Profile.Insert, new { entity.Name, entity.DownloadsDirectory, GameId = entity.Game.Id, UserId = entity.User.Id }, transaction);

            var profileGeneratedId = connection.ExecuteScalar<long>("SELECT last_insert_rowid()");

            return InternalRead(connection, transaction, profileGeneratedId);
        }
        private void InternalUpdate(IDbConnection connection, IDbTransaction transaction, IUserProfile entity)
        {
            SaveUser(connection, transaction, entity.User);

            SaveGame(connection, transaction, entity.Game);

            connection.Execute(SqliteScripts.Table.Profile.Update, new { entity.Id, entity.Name, entity.DownloadsDirectory, GameId = entity.Game.Id, UserId = entity.User.Id }, transaction);
        }
        private void InternalDelete(IDbConnection connection, IDbTransaction transaction, IUserProfile entity)
        {
            connection.Execute(SqliteScripts.Table.Profile.Delete, new { entity.Id }, transaction);
        }
        #endregion

        #region Helpers
        private void Initialize()
        {
            _dbProvider.TransactionalExecute((connection, transaction) =>
            {
                // Order of Table creation is important.
                connection.Execute(SqliteScripts.Table.User.Create, transaction: transaction);
                connection.Execute(SqliteScripts.Table.Profile.Create, transaction: transaction);
            });
        }
        private UserProfile MapProfileProps(UserProfile profile, UserDetails user, GameDetails game, GameCategory category, IDictionary<int, UserProfile> cache)
        {
            if (!cache.TryGetValue(profile.Id, out var profileEntry))
            {
                profileEntry = profile;
                cache.Add(profileEntry.Id, profileEntry);

                profileEntry.User = user;
                profileEntry.Game = game;

                profileEntry.Game.Categories = new List<GameCategory>();

                profileEntry.User.Image = _imageProvider.GetImageFor(profileEntry.User);
                profileEntry.Game.Image = _imageProvider.GetImageFor(profileEntry.Game);
            }

            profileEntry.Game.Categories.Add(category);

            return profileEntry;
        }
        private void SaveUser(IDbConnection connection, IDbTransaction transaction, IUserDetails user)
        {
            connection.Execute(
                connection.Query<UserDetails>("SELECT * FROM User u WHERE u.Id = @Id", new { user.Id },
                    transaction).FirstOrDefault() is null
                    ? SqliteScripts.Table.User.Insert
                    : SqliteScripts.Table.User.Update, user, transaction);
        }
        private void SaveGame(IDbConnection connection, IDbTransaction transaction, IGameDetails game)
        {
            var selectGame = "SELECT * FROM Game WHERE Id = @Id";

            connection.Execute(
                connection.Query<GameDetails>(selectGame, new { game.Id }, transaction).Any()
                    ? SqliteScripts.Table.Game.Update
                    : SqliteScripts.Table.Game.Insert, game, transaction);

            SaveCategories(connection, transaction, game);
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
        private IEnumerable<ProfileDetail> GetProfileDetails(IUserProfile userProfile)
        {
            var details = new List<ProfileDetail>();

            if (userProfile is null) return details;

            var detailProviders = _pluginProvider.Container.GetExportedValues<IProfileDetailsProvider>();
            foreach (var provider in detailProviders)
            {
                if (provider.PopulateDetails(userProfile))
                {
                    details.AddRange(provider.Details);
                }
            }

            return details;
        }
        private void PopulateProfileDetails(UserProfile profile)
        {
            if (profile != null)
            {
                profile.Details = GetProfileDetails(profile);
            }
        }
        #endregion
    }
}
