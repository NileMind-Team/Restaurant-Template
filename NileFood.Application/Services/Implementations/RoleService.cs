using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.Rolse;
using NileFood.Application.Services.Interfaces;

namespace NileFood.Application.Services.Implementations;
public class RoleService(RoleManager<IdentityRole> roleManager) : IRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;


    public async Task<Result<IEnumerable<RoleResponse>>> GetAllAsync()
    {
        var roles = await _roleManager.Roles
            .ProjectToType<RoleResponse>()
            .AsNoTracking()
            .ToListAsync();

        return Result.Success<IEnumerable<RoleResponse>>(roles);
    }
}
