using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NileFood.Domain.Entities;
using NileFood.Infrastructure.Data;

namespace NileFood.Infrastructure.Configurations;


public class BranchConfigurations : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Address).HasMaxLength(100).IsRequired();
        builder.Property(x => x.LocationUrl).HasMaxLength(500).IsRequired();
        builder.Property(x => x.Status).HasMaxLength(100).IsRequired();
    }
}


public class ReviewConfigurations : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.Property(x => x.Comment).HasMaxLength(100).IsRequired();


        builder.HasOne(r => r.Branch)
        .WithMany(b => b.Reviews)
        .HasForeignKey(r => r.BranchId)
        .OnDelete(DeleteBehavior.Restrict);


        builder.HasOne(r => r.User)
        .WithMany(u => u.Reviews)
        .HasForeignKey(r => r.UserId)
        .OnDelete(DeleteBehavior.Restrict);


    }
}


public class PromoCodeConfigurations : IEntityTypeConfiguration<PromoCode>
{
    public void Configure(EntityTypeBuilder<PromoCode> builder)
    {
        builder.Property(x => x.Code).HasMaxLength(100).IsRequired();

    }
}


public class PhoneNumberConfigurations : IEntityTypeConfiguration<PhoneNumber>
{
    public void Configure(EntityTypeBuilder<PhoneNumber> builder)
    {
        builder.Property(x => x.Phone).HasMaxLength(100).IsRequired();

        builder.Property(x => x.Type).HasMaxLength(100).IsRequired();
    }
}
