namespace Unclutter.SDK.Models
{
    public interface IGameCategory
    {
        int Id { get; set; }
        int GameId { get; set; }
        string Name { get; set; }
        int? ParentCategoryId { get; set; }
    }
}
