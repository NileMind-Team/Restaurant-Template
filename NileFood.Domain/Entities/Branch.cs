using NileFood.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace NileFood.Domain.Entities;
public class Branch
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string LocationUrl { get; set; } = null!;
    public string Status { get; set; } = null!;
    public double Rating_Avgarage { get; set; }

    public DateTime OpeningTime { get; set; }
    public DateTime ClosingTime { get; set; }
    public bool IsActive { get; set; }


    public int CityId { get; set; }
    public City City { get; set; } = null!;

    public List<DeliveryFee> DeliveryFee { get; set; } = [];

    public string ManagerId { get; set; } = null!;
    [ForeignKey(nameof(ManagerId))]
    public ApplicationUser User { get; set; } = null!;


    public List<Review> Reviews { get; set; } = [];
    public List<BranchPromoCode> BranchPromoCodes { get; set; } = [];
    public List<BranchMenuItem> BranchMenuItems { get; set; } = [];
}


