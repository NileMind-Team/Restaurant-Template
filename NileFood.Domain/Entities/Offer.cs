using NileFood.Domain.Entities;


public class Offer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }

    public DiscountType DiscountType { get; set; }
    public decimal DiscountValue { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }


    public int? UsageLimit { get; set; }
    public int UsageCount { get; set; } 
    public int? MinOrderAmount { get; set; }


    public OfferLevel Level { get; set; }

    
    public TimeSpan TimeRemaining => EndDate - DateTime.Now;
    public int DaysRemaining => (int)TimeRemaining.TotalDays;
    public int HoursRemaining => (int)TimeRemaining.TotalHours;

            

    public double UsagePercentage =>
        UsageLimit.HasValue ? (UsageCount / (double)UsageLimit.Value) * 100 : 0;


    public string GetStatus()
    {
        if (DateTime.Now > EndDate) return "Expired";
        if (DateTime.Now < StartDate) return "Upcoming";
        if (UsageLimit.HasValue && UsageCount >= UsageLimit.Value) return "Exhausted";
        return "Active";
    }


    public List<OfferTarget> OfferTargets { get; set; } = [];
}


public enum OfferLevel
{
    Restaurant,  
    Branch,      
    Category,    
    MenuItem     
}


