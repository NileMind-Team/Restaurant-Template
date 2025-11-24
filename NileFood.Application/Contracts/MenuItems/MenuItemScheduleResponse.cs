namespace NileFood.Application.Contracts.MenuItems;
public class MenuItemScheduleResponse
{
    public int Id { get; set; }

    //public int MenuItemId { get; set; }

    public bool IsActive { get; set; }
    public string? Notes { get; set; }


    public string Day { get; set; } = null!;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }


    public int? BranchId { get; set; }
}
