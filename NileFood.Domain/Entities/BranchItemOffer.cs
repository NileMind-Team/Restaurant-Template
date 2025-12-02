namespace NileFood.Domain.Entities;
public class BranchItemOffer
{
    public int Id { get; set; }

    public int BranchId { get; set; }
    public Branch Branch { get; set; } = null!;

    public int ItemOfferId { get; set; }
    public ItemOffer ItemOffer { get; set; } = null!;

}

