namespace NileFood.Domain.Entities;
public class PhoneNumber
{
    public int Id { get; set; }

    public string Phone { get; set; } = null!;
    public string Type { get; set; } = null!;

    public bool IsWhatsapp { get; set; }

    public int BranchId { get; set; }
    public Branch Branch { get; set; } = null!;
}

