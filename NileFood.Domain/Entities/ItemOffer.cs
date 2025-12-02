using NileFood.Domain.Entities;

public class ItemOffer
{
    public int Id { get; set; }

    public int MenuItemId { get; set; }
    public MenuItem MenuItem { get; set; } = null!;

    public bool IsPercentage { get; set; }
    public decimal DiscountValue { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }


    public bool IsEnabled { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<BranchItemOffer> BranchItemOffers { get; set; } = [];


    //public decimal ApplyDiscount(decimal basePrice)
    //{
    //    if (!IsActive) return basePrice;

    //    return IsPercentage
    //        ? basePrice - (basePrice * (DiscountValue / 100))
    //        : basePrice - DiscountValue;
    //}
}