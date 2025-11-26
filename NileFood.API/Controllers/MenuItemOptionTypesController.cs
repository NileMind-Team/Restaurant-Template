using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NileFood.API.Extensions;
using NileFood.Application.Contracts.MenuItemOptionTypes;
using NileFood.Application.Services.Interfaces;
using NileFood.Domain.Consts;

namespace NileFood.API.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
[Authorize(Roles = $"{DefaultRoles.Restaurant.Name},{DefaultRoles.Admin.Name},{DefaultRoles.Branch.Name}")]
public class MenuItemOptionTypesController(IMenuItemOptionTypeService menuItemOptionTypeService) : ControllerBase
{

    private readonly IMenuItemOptionTypeService _menuItemOptionTypeService = menuItemOptionTypeService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _menuItemOptionTypeService.GetAllAsync();

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _menuItemOptionTypeService.GetAsync(id);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpPost]
    public async Task<IActionResult> Add(MenuItemOptionTypeRequest request)
    {
        var result = await _menuItemOptionTypeService.CreateAsync(request);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }



    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, MenuItemOptionTypeRequest request)
    {
        var result = await _menuItemOptionTypeService.UpdateAsync(id, request);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _menuItemOptionTypeService.DeleteAsync(id);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

}
