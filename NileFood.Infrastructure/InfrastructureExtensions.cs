using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NileFood.Infrastructure.Data;

namespace NileFood.Infrastructure;
public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureExtensions(this IServiceCollection services, IConfiguration configuration)
    {

        var sqlConnectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        return services;
    }
}
