namespace Unclutter.Modules.Plugins
{
    public interface IAppWindowPlugin
    {
        object Content { get; }
        void Initialize();
    }
}
