namespace NileFood.Domain.Entities;
public class PromoCode
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;

    public DiscountType DiscountType { get; set; }

    public decimal DiscountValue { get; set; }

    public decimal? MaxDiscountAmount { get; set; }

    public decimal? MinimumOrderAmount { get; set; }

    public int? UsageLimit { get; set; }
    public int UsedCount { get; set; } = 0;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedIn { get; set; }
    public DateTime ExpirationDate { get; set; }

    public List<BranchPromoCode> BranchPromoCodes { get; set; } = [];
}

public enum DiscountType
{
    Percentage = 1,
    FixedAmount = 2
}
