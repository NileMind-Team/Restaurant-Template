using NileFood.Application.Contracts.MenuItemOptions;
using NileFood.Application.Contracts.MenuItems;

namespace NileFood.Application.Contracts.CartItems;

public class CartItemResponse
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public MenuItemResponse MenuItem { get; set; } = null!;

    public int Quantity { get; set; }
    public bool IsOfferApplied { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public decimal TotalPrice { get; set; }

    // يجب أن يكون نفس اسم Navigation في الـ CartItemOption
    public List<MenuItemOptionResponse> MenuItemOptions { get; set; } = [];
}