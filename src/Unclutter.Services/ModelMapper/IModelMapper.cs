namespace Unclutter.Services.ModelMapper
{
    public interface IModelMapper
    {
        TDestination MapOrDefault<TDestination>(object obj);
    }
}