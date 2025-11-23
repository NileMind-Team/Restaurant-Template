namespace NileFood.Domain.Entities;
public class MenuItem
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal BasePrice { get; set; }
    public string ImageUrl { get; set; } = null!;

    //public bool IsAvailableNow { get; set; }
    public bool IsAllTime { get; set; }
    public bool IsActive { get; set; } = true;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;


    public List<BranchMenuItem> BranchMenuItems { get; set; } = [];
    public List<MenuItemOption> MenuItemOptions { get; set; } = [];
    //public List<OfferTarget> OfferTargets { get; set; } = [];
}