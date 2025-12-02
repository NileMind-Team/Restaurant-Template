using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NileFood.API.Extensions;
using NileFood.Application.Contracts.Cities;
using NileFood.Application.Services.Interfaces;
using NileFood.Domain.Consts;

namespace NileFood.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize(Roles = DefaultRoles.Restaurant.Name + "," + DefaultRoles.Admin.Name)]
public class CitiesController(ICityService cityService) : ControllerBase
{
    private readonly ICityService _cityService = cityService;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var result = await _cityService.GetAllAsync();

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _cityService.GetAsync(id);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpPost]
    public async Task<IActionResult> Add(CityRequest request)
    {
        var result = await _cityService.CreateAsync(request);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }



    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CityRequest request)
    {
        var result = await _cityService.UpdateAsync(id, request);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }


    //[HttpDelete("{id}")]
    //public async Task<IActionResult> Delete(int id)
    //{
    //    var result = await _cityService.DeleteAsync(id);

    //    return result.IsSuccess ? NoContent() : result.ToProblem();
    //}

}
