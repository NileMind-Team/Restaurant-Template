using Microsoft.AspNetCore.Mvc;

namespace NileFood.Application.Contracts.MenuItems;

public class MenuItemRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal BasePrice { get; set; }
    public IFormFile Image { get; set; } = null!;
    public int CategoryId { get; set; }
    public bool IsActive { get; set; }

    public int? Calories { get; set; }
    public int? PreparationTimeStart { get; set; }
    public int? PreparationTimeEnd { get; set; }


    [FromForm]
    public List<MenuItemScheduleRequest> MenuItemSchedules { get; set; } = [];
}
