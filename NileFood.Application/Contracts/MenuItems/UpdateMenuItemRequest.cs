using Microsoft.AspNetCore.Mvc;

namespace NileFood.Application.Contracts.MenuItems;

public class UpdateMenuItemRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal BasePrice { get; set; }
    public int CategoryId { get; set; }
    public IFormFile Image { get; set; } = null!;

    public int? Calories { get; set; }
    public int PreparationTimeStart { get; set; }
    public int PreparationTimeEnd { get; set; }


    [FromForm]
    public List<MenuItemScheduleRequest> MenuItemSchedules { get; set; } = [];
}
