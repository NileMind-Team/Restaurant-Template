using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NileFood.API.Extensions;
using NileFood.Application.Contracts.Users;
using NileFood.Application.Services.Interfaces;
using NileFood.Domain.Consts;

namespace NileFood.API.Controllers;


[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = DefaultRoles.Admin.Name)]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;



    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Add(CreateUserRequest request)
    {
        var result = await _userService.AddAsync(request);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _userService.GetByIdAsync(id);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AssignRole(string userId, string role)
    {
        var result = await _userService.AssignRoleAsync(userId, role);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }


    [HttpDelete("{email}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(string email)
    {
        var result = await _userService.DeleteAsync(email);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
