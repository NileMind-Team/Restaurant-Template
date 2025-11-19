using Egyptos.Domain.Consts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NileFood.Domain.Entities.Identity;

namespace NileFood.Infrastructure.Configurations;
public class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {

        builder.Property(x => x.FirstName).HasMaxLength(50);

        builder.Property(x => x.LastName).HasMaxLength(50);

        //builder.HasOne(u => u.DefaultLocation)
        //.WithOne()
        //.HasForeignKey<ApplicationUser>(u => u.DefaultLocationId)
        //.OnDelete(DeleteBehavior.Restrict);


        var Users = new List<ApplicationUser>()
        {
            new ApplicationUser
            {
                Id = DefaultUser.AdminId,
                FirstName = "Admin",
                LastName = "Role",
                PhoneNumber = "1234567890",
                ImageUrl = "profiles/Default-Image.jpg",
                UserName = DefaultUser.AdminEmail,
                NormalizedUserName = DefaultUser.AdminEmail.ToUpper(),
                Email = DefaultUser.AdminEmail,
                NormalizedEmail = DefaultUser.AdminEmail.ToUpper(),
                SecurityStamp = DefaultUser.AdminSecurityStamp,
                ConcurrencyStamp = DefaultUser.AdminConcurrencyStamp,
                EmailConfirmed = true,
                PasswordHash = DefaultUser.AdminPasswordHash,
            }
        };



        builder.HasData(Users);
    }
}
