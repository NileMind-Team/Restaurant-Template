namespace NileFood.Domain.Entities;

public class BranchPromoCode
{
    public int Id { get; set; }

    public int BranchId { get; set; }
    public Branch Branch { get; set; } = null!;

    public int PromoCodeId { get; set; }
    public PromoCode PromoCode { get; set; } = null!;
}