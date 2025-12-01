namespace NileFood.Application.Contracts.PhoneNumbers;
public class PhoneNumberResponse
{
    public int Id { get; set; }
    public int BranchId { get; set; }

    public string Phone { get; set; } = null!;
    public string Type { get; set; } = null!;

    public bool IsWhatsapp { get; set; }
}
