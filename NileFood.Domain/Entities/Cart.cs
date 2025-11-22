using static NileFood.Domain.Consts.DefaultRoles;

namespace NileFood.Domain.Entities;

public class Cart
{
    public int Id { get; set; }
        
    public decimal TotalPrice { get; set; }
    
    public bool IsActive { get; set; }

    
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int BranchId { get; set; }
    public Branch Branch { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }


    public List<CartItem> CartItems { get; set; } = [];

    // public Order Order { get; set; }
}