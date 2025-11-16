using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NileFood.API.Extensions;
using NileFood.Application.Services.Interfaces;
using NileFood.Domain.Consts;

namespace NileFood.API.Controllers;

[ApiController]
[Authorize(Roles = DefaultRoles.Admin.Name)]
[Route("api/[controller]/[action]")]
public class RolesController(IRoleService roleService) : ControllerBase
{
    private readonly IRoleService _roleService = roleService;


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _roleService.GetAllAsync();
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


}

