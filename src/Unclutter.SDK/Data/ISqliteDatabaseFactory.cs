namespace Unclutter.SDK.Data
{
    public interface ISqliteDatabaseFactory
    {
        /// <summary>
        /// Create or Get a database with the specified name
        /// </summary>
        /// <param name="name">Name of the database</param>
        /// <returns>A <see cref="IDatabaseProvider"/> object that represents the database</returns>
        IDatabaseProvider CreateOrGet(string name);
    }
}
