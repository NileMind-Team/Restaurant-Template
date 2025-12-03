using NileFood.Application.Contracts.BranchMenuItemOptions;
using NileFood.Application.Contracts.Categories;
using NileFood.Application.Contracts.ItemOffers;

namespace NileFood.Application.Contracts.MenuItems;
public class MenuItemDetailsResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal BasePrice { get; set; }
    public string ImageUrl { get; set; } = null!;

    public bool IsAllTime { get; set; }
    public bool IsActive { get; set; } = true;

    public int? Calories { get; set; }
    public int? PreparationTimeStart { get; set; }
    public int? PreparationTimeEnd { get; set; }


    public CategoryResponse Category { get; set; } = null!;
    public ItemOfferResponse ItemOffer { get; set; } = null!;
    public List<MenuItemScheduleResponse> MenuItemSchedules { get; set; } = [];


    public List<BranchMenuItemResponse> BranchMenuItems { get; set; } = [];
    public List<OptionTypeWithOptions> TypesWithOptions { get; set; } = [];
}
