using Dapper;
using System;
using System.Collections.Generic;
using System.IO;
using Unclutter.SDK.Data;
using Unclutter.SDK.IModels;
using Unclutter.SDK.IServices;
using Unclutter.Services.Data;
using Unclutter.Services.Games;
using Unclutter.Services.Images;

namespace Unclutter.Services.Profiles
{
    public class ProfilesManager : IProfilesManager
    {
        /* Services */
        private readonly IDatabaseProvider _dbProvider;
        private readonly IGamesProvider _gamesProvider;
        private readonly IImageProvider _imageProvider;

        /* Properties */
        public string ProfilesDirectory { get; }
        public IUserProfile CurrentProfile { get; private set; }

        /* Events */
        public event Action<ProfileChangedArgs> ProfileChanged;

        /* Constructors */
        public ProfilesManager(ISqliteDatabaseFactory sqliteDatabaseFactory, IGamesProvider gamesProvider, IImageProvider imageProvider, IDirectoryService directoryService)
        {
            _dbProvider = sqliteDatabaseFactory.CreateOrGet(LocalIdentifiers.Database.App);
            _gamesProvider = gamesProvider;
            _imageProvider = imageProvider;

            ProfilesDirectory = Path.Combine(directoryService.DataDirectory, "profiles");
            CurrentProfile = null;

            directoryService.EnsureDirectoryAccess(ProfilesDirectory);

            Initialize();
        }

        /* Methods */
        public IEnumerable<IUserProfile> EnumerateProfiles()
        {
            return ReadAll();
        }

        public IEnumerable<IUserDetails> EnumerateUsers()
        {
            return _dbProvider.TransactionalSqlQuery<IEnumerable<IUserDetails>>((db, transaction) =>
            {
                var selectUsers = "SELECT * FROM User";

                var users = db.Query<UserDetails>(selectUsers, transaction: transaction).AsList();
                foreach (var user in users)
                {
                    user.ImageSource = _imageProvider.GetImageFor(user);
                }

                return users;
            });
        }

        public void Save(IEnumerable<IUserProfile> profiles)
        {
            if (profiles is null) return;

            _dbProvider.TransactionalSqlCommand((db, transaction) =>
            {
                foreach (var profile in profiles)
                {
                    if (ReadById(profile.Id) is null)
                    {
                        Insert(profile);
                    }
                    else
                    {
                        Update(profile);
                    }
                }
            });
        }

        public void ChangeProfile(IUserProfile profile)
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
                Game = game as GameDetails, // TODO: temporary workaround
                User = user as UserDetails, // TODO: temporary workaround
                GameId = game.Id,
                UserId = user.Id
            };

            Save(new[] { profile });

            return profile;
        }

        #region CRUD
        public IEnumerable<IUserProfile> ReadAll()
        {
            return _dbProvider.TransactionalSqlQuery<IEnumerable<IUserProfile>>((db, transaction) =>
            {
                var selectProfiles = "SELECT * FROM Profile";
                var selectUser = "SELECT * FROM User WHERE Id = @Id";

                var profiles = db.Query<UserProfile>(selectProfiles, transaction: transaction).AsList();
                foreach (var profile in profiles)
                {
                    var user = db.QueryFirstOrDefault<UserDetails>(selectUser, new { Id = profile.UserId }, transaction);
                    user.ImageSource = _imageProvider.GetImageFor(user);
                    var game = _gamesProvider.ReadById(profile.GameId);
                    profile.User = user;
                    profile.Game = game as GameDetails; // TODO: temporary workaround
                }

                return profiles;
            });
        }

        public IUserProfile ReadById(long id)
        {
            return _dbProvider.TransactionalSqlQuery<IUserProfile>((db, transaction) =>
            {
                var selectProfile = "SELECT * FROM Profile WHERE Id = @Id";
                var selectUser = "SELECT * FROM User WHERE Id = @Id";
                var profile = db.QueryFirstOrDefault<UserProfile>(selectProfile, new { Id = id }, transaction);

                if (profile is null) return null;

                var user = db.QueryFirstOrDefault<UserDetails>(selectUser, new { Id = profile.UserId }, transaction);
                user.ImageSource = _imageProvider.GetImageFor(user);
                var game = _gamesProvider.ReadById(profile.GameId);

                profile.User = user;
                profile.Game = game as GameDetails; // TODO: temporary workaround
                return profile;

            });
        }

        public IUserProfile Insert(IUserProfile entity)
        {
            if (entity is null) return null;

            return _dbProvider.TransactionalSqlQuery<IUserProfile>((db, transaction) =>
            {
                db.Execute(
                    db.QueryFirstOrDefault<UserDetails>("SELECT * FROM User WHERE Id = @Id", new { entity.User.Id },
                        transaction) is null
                        ? SqliteScripts.Table.User.Insert
                        : SqliteScripts.Table.User.Update, entity.User, transaction);

                _gamesProvider.Save(new List<IGameDetails> { entity.Game });

                db.Execute(SqliteScripts.Table.Profile.Insert, new { entity.Name, entity.DownloadsDirectory, UserId = entity.User.Id, GameId = entity.Game.Id }, transaction);
                var id = db.QuerySingle<long>("SELECT last_insert_rowid()", transaction: transaction);
                return new UserProfile
                {
                    Id = id,
                    DownloadsDirectory = entity.DownloadsDirectory,
                    Name = entity.Name,
                    Game = entity.Game as GameDetails, // TODO: temporary workaround
                    User = entity.User as UserDetails //  TODO: temporary workaround
                };
            });
        }

        public IUserProfile Update(IUserProfile entity)
        {
            if (entity is null) return null;

            _dbProvider.TransactionalSqlCommand((db, transaction) =>
            {
                db.Execute(
                    db.QueryFirstOrDefault<UserDetails>("SELECT * FROM User WHERE Id = @Id", new { entity.User.Id },
                        transaction) is null
                        ? SqliteScripts.Table.User.Insert
                        : SqliteScripts.Table.User.Update, entity.User, transaction);

                _gamesProvider.Save(new List<IGameDetails> { entity.Game });

                db.Execute(SqliteScripts.Table.Profile.Update, new { entity.Name, entity.DownloadsDirectory, UserId = entity.User.Id, GameId = entity.Game.Id }, transaction);
            });

            return entity;
        }

        public IUserProfile Delete(IUserProfile entity)
        {
            if (entity is null) return null;

            _dbProvider.TransactionalSqlCommand((db, transaction) =>
            {
                db.Execute(SqliteScripts.Table.Profile.Delete, new { entity.Id }, transaction);
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
                db.Execute(SqliteScripts.Table.User.Create, transaction);
                db.Execute(SqliteScripts.Table.Profile.Create, transaction);
            });
        }
    }
}
