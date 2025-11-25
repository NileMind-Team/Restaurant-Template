namespace NileFood.Domain.Entities;
public class MenuItem
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal BasePrice { get; set; }
    public string ImageUrl { get; set; } = null!;

    public int? Calories { get; set; }
    public int PreparationTimeStart { get; set; }
    public int PreparationTimeEnd { get; set; }

    public bool IsAllTime { get; set; } = true;
    public bool IsActive { get; set; } = true;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;


    public List<BranchMenuItem> BranchMenuItems { get; set; } = [];
    public List<MenuItemOption> MenuItemOptions { get; set; } = [];
    //public List<OfferTarget> OfferTargets { get; set; } = [];    
    public List<MenuItemSchedule> MenuItemSchedules { get; set; } = [];
}