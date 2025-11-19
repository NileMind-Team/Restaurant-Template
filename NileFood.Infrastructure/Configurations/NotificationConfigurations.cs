using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NileFood.Domain.Entities;

namespace NileFood.Infrastructure.Configurations;
public class NotificationConfigurations : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder
            .Property(n => n.SenderType)
            .HasConversion<string>();

        builder
            .Property(n => n.ReceiverType)
            .HasConversion<string>();
    }
}
