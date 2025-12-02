using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NileFood.Infrastructure.Configurations;
public class ItemOfferConfiguration : IEntityTypeConfiguration<ItemOffer>
{
    public void Configure(EntityTypeBuilder<ItemOffer> builder)
    {
        builder.Property(x => x.DiscountValue)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.HasIndex(x => x.MenuItemId);
        builder.HasIndex(x => new { x.StartDate, x.EndDate });
    }
}
