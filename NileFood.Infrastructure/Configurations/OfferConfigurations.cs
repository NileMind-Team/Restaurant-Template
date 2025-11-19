using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace NileFood.Infrastructure.Configurations;

public class OfferConfigurations : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(o => o.Description)
            .HasMaxLength(500);

        builder.Property(o => o.ImageUrl)
            .HasMaxLength(500);

        builder.Property(o => o.DiscountValue)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(o => o.MinOrderAmount)
            .HasColumnType("decimal(10,2)");

        builder.Property(o => o.DiscountType)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(o => o.Level)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(o => o.CreatedAt)
            .IsRequired();

       
        builder.Ignore(o => o.TimeRemaining);
        builder.Ignore(o => o.DaysRemaining);
        builder.Ignore(o => o.HoursRemaining);
        builder.Ignore(o => o.UsagePercentage);

        
        builder.HasIndex(o => new { o.StartDate, o.EndDate });
        builder.HasIndex(o => o.Level);        
    }
}

public class OfferTargetConfigurations : IEntityTypeConfiguration<OfferTarget>
{
    public void Configure(EntityTypeBuilder<OfferTarget> builder)
    {
        builder.HasKey(ot => ot.Id);

        
        builder.HasOne(ot => ot.Offer)
            .WithMany(o => o.OfferTargets)
            .HasForeignKey(ot => ot.OfferId)
            .OnDelete(DeleteBehavior.Cascade);

        
        builder.HasOne(ot => ot.Branch)
            .WithMany()
            .HasForeignKey(ot => ot.BranchId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(ot => ot.Category)
            .WithMany()
            .HasForeignKey(ot => ot.CategoryId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(ot => ot.MenuItem)
            .WithMany()
            .HasForeignKey(ot => ot.MenuItemId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        
        builder.HasIndex(t => new { t.OfferId, t.BranchId, t.CategoryId, t.MenuItemId })
            .HasDatabaseName("IX_OfferTarget_Composite");

        
        builder.HasIndex(t => t.OfferId);
        builder.HasIndex(t => t.BranchId);
        builder.HasIndex(t => t.CategoryId);
        builder.HasIndex(t => t.MenuItemId);
    }
}