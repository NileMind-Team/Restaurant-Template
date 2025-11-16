using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.Rolse;

namespace NileFood.Application.Services.Interfaces;
public interface IRoleService
{
    Task<Result<IEnumerable<RoleResponse>>> GetAllAsync();
}
