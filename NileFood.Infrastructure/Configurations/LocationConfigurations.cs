using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NileFood.Domain.Entities;

namespace NileFood.Infrastructure.Configurations;
public class LocationConfigurations : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder
        .HasOne(l => l.User)
        .WithMany(u => u.Locations)
        .HasForeignKey(l => l.UserId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}
