namespace NileFood.Application.Contracts.ItemOffers;
public class ItemOfferResponse
{
    public int Id { get; set; }

    public int MenuItemId { get; set; }

    public bool IsPercentage { get; set; }
    public decimal DiscountValue { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }


    public bool IsEnabled { get; set; } = true;


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
