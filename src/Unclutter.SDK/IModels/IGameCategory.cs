namespace Unclutter.SDK.IModels
{
    public interface IGameCategory
    {
        long Id { get; set; }
        long GameId { get; set; }
        string Name { get; set; }
        long? ParentCategoryId { get; set; }
    }
}
