namespace Unclutter.Extensions.Modules
{
    public interface IModuleMetadata
    {
        string Name { get; }
        string Version { get; }
        string Author { get; }
        string Description { get; }
    }
}
