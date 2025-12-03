using NileFood.Domain.Entities.Identity;

namespace NileFood.Domain.Entities;

public class Cart
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;

    public List<CartItem> CartItems { get; set; } = [];


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // خاصية محسوبة
    public decimal TotalPrice => CartItems?.Sum(item => item.TotalPrice) ?? 0;
    public int TotalItems => CartItems?.Sum(item => item.Quantity) ?? 0;
}