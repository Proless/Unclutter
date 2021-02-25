using Dapper;
using System.Collections.Generic;
using Unclutter.SDK.IModels;
using Unclutter.Services.Data;
using Unclutter.Services.Games;

namespace Unclutter.Services.Profiles
{
    public class ProfilesManager : IProfilesManager
    {
        /* Services */
        private readonly IAppDatabaseProvider _appDatabaseProvider;
        private readonly IGamesProvider _gamesProvider;

        /* Constructors */
        public ProfilesManager(IAppDatabaseProvider appDatabaseProvider, IGamesProvider gamesProvider)
        {
            _appDatabaseProvider = appDatabaseProvider;
            _gamesProvider = gamesProvider;
        }

        /* Methods */
        public IEnumerable<IUserProfile> EnumerateProfiles()
        {
            return ReadAll();
        }

        public void Save(IEnumerable<IUserProfile> profiles)
        {
            if (profiles is null) return;

            _appDatabaseProvider.TransactionalSqlCommand((db, transaction) =>
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

        #region CRUD
        public IEnumerable<IUserProfile> ReadAll()
        {
            return _appDatabaseProvider.TransactionalSqlQuery<IEnumerable<IUserProfile>>((db, transaction) =>
            {
                var selectProfiles = "SELECT * FROM Profile";
                var selectUser = "SELECT * FROM User WHERE Id = @Id";

                var profiles = db.Query<UserProfile>(selectProfiles, transaction: transaction).AsList();
                foreach (var profile in profiles)
                {
                    var user = db.QueryFirstOrDefault<UserDetails>(selectUser, new { Id = profile.UserId }, transaction);
                    var game = _gamesProvider.ReadById(profile.GameId);
                    profile.User = user;
                    profile.Game = game;
                }

                return profiles;
            });
        }

        public IUserProfile ReadById(long id)
        {
            return _appDatabaseProvider.TransactionalSqlQuery<IUserProfile>((db, transaction) =>
            {
                var selectProfile = "SELECT * FROM Profile WHERE Id = @Id";
                var selectUser = "SELECT * FROM User WHERE Id = @Id";
                var profile = db.QueryFirstOrDefault<UserProfile>(selectProfile, new { Id = id }, transaction);

                if (profile is null) return null;

                var user = db.QueryFirstOrDefault<UserDetails>(selectUser, new { Id = profile.UserId }, transaction);
                var game = _gamesProvider.ReadById(profile.GameId);

                profile.User = user;
                profile.Game = game;
                return profile;

            });
        }

        public IUserProfile Insert(IUserProfile entity)
        {
            if (entity is null) return null;

            return _appDatabaseProvider.TransactionalSqlQuery<IUserProfile>((db, transaction) =>
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
                    Game = entity.Game,
                    User = entity.User
                };
            });
        }

        public IUserProfile Update(IUserProfile entity)
        {
            if (entity is null) return null;

            _appDatabaseProvider.TransactionalSqlCommand((db, transaction) =>
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

            _appDatabaseProvider.TransactionalSqlCommand((db, transaction) =>
            {
                db.Execute(SqliteScripts.Table.Profile.Delete, new { entity.Id }, transaction);
            });

            return entity;
        }
        #endregion

    }
}
