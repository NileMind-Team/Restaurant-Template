using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NileFood.Domain.Consts;

namespace NileFood.Infrastructure.Configurations;
public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData([
            new IdentityRole(){
                Id = DefaultRoles.Admin.Id,
                Name = DefaultRoles.Admin.Name,
                NormalizedName = DefaultRoles.Admin.Name.ToUpper(),
                ConcurrencyStamp = DefaultRoles.Admin.ConcurrencyStamp,
            },
            new IdentityRole(){
                Id = DefaultRoles.Restaurant.Id,
                Name = DefaultRoles.Restaurant.Name,
                NormalizedName = DefaultRoles.Restaurant.Name.ToUpper(),
                ConcurrencyStamp = DefaultRoles.Restaurant.ConcurrencyStamp,
            },
            new IdentityRole(){
                Id = DefaultRoles.User.Id,
                Name = DefaultRoles.User.Name,
                NormalizedName = DefaultRoles.User.Name.ToUpper(),
                ConcurrencyStamp = DefaultRoles.User.ConcurrencyStamp,
            },
            new IdentityRole(){
                Id = DefaultRoles.Branch.Id,
                Name = DefaultRoles.Branch.Name,
                NormalizedName = DefaultRoles.Branch.Name.ToUpper(),
                ConcurrencyStamp = DefaultRoles.Branch.ConcurrencyStamp,
            }
        ]);
    }
}
