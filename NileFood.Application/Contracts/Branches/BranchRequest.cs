using NileFood.Application.Contracts.PhoneNumbers;

namespace NileFood.Application.Contracts.Branches;
public class BranchRequest
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string? LocationUrl { get; set; }
    public string Status { get; set; } = null!;



    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }

    public bool IsActive { get; set; }

    public int CityId { get; set; }

    public string ManagerId { get; set; } = null!;

    public List<PhoneNumberRequest> PhoneNumbers { get; set; } = [];
}
