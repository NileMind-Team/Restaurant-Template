using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NileFood.API.Extensions;
using NileFood.Application.Contracts.Common;
using NileFood.Application.Contracts.MenuItems;
using NileFood.Application.Services.Interfaces;
using NileFood.Domain.Consts;

namespace NileFood.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize(Roles = $"{DefaultRoles.Restaurant.Name},{DefaultRoles.Admin.Name},{DefaultRoles.Branch.Name}")]
public class MenuItemsController(IMenuItemService menuItemService) : ControllerBase
{
    private readonly IMenuItemService _menuItemService = menuItemService;


    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Dictionary<string, string> filterDto, [FromQuery] UserParams userParams, int? categoryId)
    {
        var filters = FilterDto.BindFromDictionary(filterDto);

        var result = await _menuItemService.GetAllAsync(filters, userParams, categoryId);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllWithoutPagination(int? categoryId)
    {
        var result = await _menuItemService.GetAllAsync(categoryId);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _menuItemService.GetAsync(id);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpPost]
    public async Task<IActionResult> Add([FromForm] MenuItemRequest request)
    {
        var result = await _menuItemService.CreateAsync(request, User.GetUserId());

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromForm] UpdateMenuItemRequest request)
    {
        var result = await _menuItemService.UpdateAsync(id, request);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _menuItemService.DeleteAsync(id);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> ChangeMenuItemActiveStatus(int id)
    {
        var result = await _menuItemService.ChangeMenuItemActiveStatusAsync(id);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
