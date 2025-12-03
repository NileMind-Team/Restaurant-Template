namespace NileFood.Application.Contracts.CartItems;
public class CartItemRequest
{

    public int MenuItemId { get; set; }
    public int Quantity { get; set; }


    public List<int> Options { get; set; } = [];
}
