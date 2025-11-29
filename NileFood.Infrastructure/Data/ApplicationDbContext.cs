using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NileFood.Domain.Entities;
using NileFood.Domain.Entities.Identity;

namespace NileFood.Infrastructure.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{

    public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;
    public DbSet<Location> Locations { get; set; } = null!;
    public DbSet<City> Cities { get; set; } = null!;
    public DbSet<Branch> Branches { get; set; } = null!;
    public DbSet<DeliveryFee> DeliveryFees { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; } = null!;
    public DbSet<BranchPromoCode> BranchPromoCodes { get; set; } = null!;
    public DbSet<PromoCode> PromoCodes { get; set; }
    public DbSet<PhoneNumber> PhoneNumbers { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<MenuItemSchedule> MenuItemSchedules { get; set; }
    public DbSet<MenuItemOption> MenuItemOptions { get; set; }
    public DbSet<MenuItemOptionType> MenuItemOptionTypes { get; set; }
    public DbSet<BranchMenuItem> BranchMenuItems { get; set; }
    public DbSet<BranchMenuItemOption> BranchMenuItemOptions { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    //public DbSet<Offer> Offers { get; set; }
    //public DbSet<OfferTarget> OfferTargets { get; set; }





    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

}
