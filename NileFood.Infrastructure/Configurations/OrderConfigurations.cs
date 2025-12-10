using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NileFood.Domain.Entities;

namespace NileFood.Infrastructure.Configurations;
public class OrderConfigurations : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder
            .Property(o => o.Status)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.HasOne(o => o.DeliveryFee)
            .WithMany()
            .HasForeignKey(o => o.DeliveryFeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Location)
            .WithMany()
            .HasForeignKey(o => o.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Branch)
            .WithMany(b => b.Orders)
            .HasForeignKey(o => o.BranchId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
