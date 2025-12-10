namespace NileFood.Application.Contracts.Orders;
public class OrderItemOptionResponse
{
    public int Id { get; set; }


    public string OptionNameAtOrder { get; set; } = null!;
    public decimal OptionPriceAtOrder { get; set; }
}
