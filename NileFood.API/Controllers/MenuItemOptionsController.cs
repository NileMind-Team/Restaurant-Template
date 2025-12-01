using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NileFood.API.Extensions;
using NileFood.Application.Contracts.MenuItemOptions;
using NileFood.Application.Services.Interfaces;
using NileFood.Domain.Consts;

namespace NileFood.API.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
[Authorize(Roles = $"{DefaultRoles.Restaurant.Name},{DefaultRoles.Admin.Name},{DefaultRoles.Branch.Name}")]
public class MenuItemOptionsController(IMenuItemOptionService menuItemOptionService) : ControllerBase
{
    private readonly IMenuItemOptionService _menuItemOptionService = menuItemOptionService;


    [HttpPost]
    public async Task<IActionResult> Add(MenuItemOptionDetailsRequest request)
    {
        var result = await _menuItemOptionService.CreateAsync(request, User.GetUserId());

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateMenuItemOptionDetailsRequest request)
    {
        var result = await _menuItemOptionService.UpdateAsync(id, request, User.GetUserId());

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _menuItemOptionService.DeleteAsync(id, User.GetUserId());

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
