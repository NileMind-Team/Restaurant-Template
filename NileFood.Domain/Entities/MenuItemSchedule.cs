namespace NileFood.Domain.Entities;

public class MenuItemSchedule
{
    public int Id { get; set; }

    public int MenuItemId { get; set; }
    public MenuItem MenuItem { get; set; } = null!;

    public bool IsActive { get; set; }
    public string? Notes { get; set; }


    public string Day { get; set; } = null!;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }


    public int? BranchId { get; set; }
    public Branch? Branch { get; set; }
}