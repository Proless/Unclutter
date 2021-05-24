namespace Unclutter.Services.Data
{
    public class SqliteScripts
    {
        public class Table
        {
            public class Game
            {
                public const string Create = "CREATE TABLE IF NOT EXISTS Game (Id INTEGER NOT NULL, Name TEXT, ForumUrl TEXT, NexusModsUrl TEXT, Genre TEXT, FileCount INTEGER, Downloads INTEGER, DomainName TEXT NOT NULL, ApprovedDate INTEGER, FileViews INTEGER, Authors INTEGER, FileEndorsements INTEGER, Mods INTEGER, PRIMARY KEY(Id))";
                public const string Insert = "INSERT INTO Game (Id, Name, DomainName, Downloads, Mods, ForumUrl, NexusModsUrl, Genre, FileCount, ApprovedDate, FileViews, Authors, FileEndorsements) VALUES (@Id, @Name, @DomainName, @Downloads, @Mods, @ForumUrl, @NexusModsUrl, @Genre, @FileCount, @ApprovedDate, @FileViews, @Authors, @FileEndorsements)";
                public const string Update = "UPDATE Game SET Id = @Id , Name = @Name, DomainName = @DomainName, Downloads = @Downloads, Mods = @Mods, ForumUrl = @ForumUrl, NexusModsUrl = @NexusModsUrl, Genre = @Genre, FileCount = @FileCount, ApprovedDate = @ApprovedDate, FileViews = @FileViews, Authors = @Authors, FileEndorsements = @FileEndorsements WHERE Id = @Id";
                public const string Delete = "DELETE FROM Game WHERE Id = @Id";
            }

            public class GameCategory
            {
                public const string Create = "CREATE TABLE IF NOT EXISTS GameCategory (Id INTEGER NOT NULL, Name TEXT, ParentCategoryId INTEGER, GameId INTEGER NOT NULL, FOREIGN KEY(GameId) REFERENCES Game(Id) ON DELETE CASCADE, PRIMARY KEY(Id, GameId))";
                public const string Insert = "INSERT INTO GameCategory (Id, GameId, Name, ParentCategoryId) VALUES (@Id, @GameId, @Name, @ParentCategoryId)";
                public const string Update = "UPDATE GameCategory SET Id = @Id, GameId = @GameId, Name = @Name, ParentCategoryId = @ParentCategoryId WHERE Id = @Id AND GameId = @GameId";
                public const string Delete = "DELETE FROM GameCategory WHERE Id = @Id AND GameId = @GameId";
            }

            public class User
            {
                public const string Create = "CREATE TABLE IF NOT EXISTS User (Id INTEGER NOT NULL, Key TEXT NOT NULL, Name TEXT NOT NULL, Email TEXT NOT NULL, ProfileUri TEXT NOT NULL, IsSupporter INTEGER NOT NULL, IsPremium INTEGER NOT NULL, PRIMARY KEY(Id))";
                public const string Insert = "INSERT INTO User (Id, Key, Name, Email, ProfileUri, IsSupporter, IsPremium) VALUES (@Id, @Key, @Name, @Email, @ProfileUri, @IsSupporter, @IsPremium)";
                public const string Update = "UPDATE User SET Id = @Id, Key = @Key, Name = @Name, Email = @Email, ProfileUri = @ProfileUri, IsSupporter = @IsSupporter, IsPremium = @IsPremium WHERE Id = @Id";
                public const string Delete = "DELETE FROM User WHERE Id = @Id";
            }

            public class Profile
            {
                public const string Create = "CREATE TABLE IF NOT EXISTS Profile (Id INTEGER NOT NULL, Name TEXT NOT NULL, DownloadsDirectory TEXT NOT NULL, GameId INTEGER NOT NULL, UserId INTEGER NOT NULL, FOREIGN KEY(GameId) REFERENCES Game(Id), FOREIGN KEY(UserId) REFERENCES User(Id), PRIMARY KEY(Id AUTOINCREMENT))";
                public const string Insert = "INSERT INTO Profile (Name, DownloadsDirectory, GameId, UserId) VALUES (@Name, @DownloadsDirectory, @GameId, @UserId)";
                public const string Update = "UPDATE Profile SET Name = @Name, DownloadsDirectory = @DownloadsDirectory, GameId = @GameId, UserId = @UserId WHERE Id = @Id";
                public const string Delete = "DELETE FROM Profile WHERE Id = @Id";
            }
        }
    }
}
