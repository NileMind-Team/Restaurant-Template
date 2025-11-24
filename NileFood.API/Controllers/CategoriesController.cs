using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NileFood.API.Extensions;
using NileFood.Application.Contracts.Categories;
using NileFood.Application.Services.Interfaces;
using NileFood.Domain.Consts;

namespace NileFood.API.Controllers;
[Route("api/[controller]/[action]")]
[Authorize(Roles = $"{DefaultRoles.Restaurant.Name},{DefaultRoles.Admin.Name},{DefaultRoles.Branch.Name}")]
public class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    private readonly ICategoryService _categoryService = categoryService;



    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _categoryService.GetAllAsync();

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _categoryService.GetAsync(id);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpPost]
    public async Task<IActionResult> Add(CategoryRequest request)
    {
        var result = await _categoryService.CreateAsync(request);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CategoryRequest request)
    {
        var result = await _categoryService.UpdateAsync(id, request);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ChangeCategoryActiveStatus(int id)
    {
        var result = await _categoryService.ChangeCategoryActiveStatusAsync(id);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

}
