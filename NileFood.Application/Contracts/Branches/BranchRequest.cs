namespace NileFood.Application.Contracts.Branches;
public class BranchRequest
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string LocationUrl { get; set; } = null!;
    public string Status { get; set; } = null!;

    public double Rating_Avgarage { get; set; }

    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }

    public bool IsActive { get; set; }

    public int CityId { get; set; }

    public string ManagerId { get; set; } = null!;
}
