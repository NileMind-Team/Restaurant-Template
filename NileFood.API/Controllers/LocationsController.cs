using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NileFood.API.Extensions;
using NileFood.Application.Contracts.Locations;
using NileFood.Application.Services.Interfaces;

namespace NileFood.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class LocationsController(ILocationService locationService) : ControllerBase
{
    private readonly ILocationService _locationService = locationService;

    [HttpGet]    
    public async Task<IActionResult> GetAllForUser(string? userId)
    {
        var result = await _locationService.GetAllForUserAsync(userId ?? User.GetUserId());

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _locationService.GetAsync(id);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpPost]
    public async Task<IActionResult> Add(LocationRequest request)
    {
        var result = await _locationService.CreateAsync(User.GetUserId(), request);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> ChangeDefaultLocation(int id)
    {
        var result = await _locationService.ChangeDefaultLocationAsync(id,User.GetUserId());

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id,LocationRequest request)
    {
        var result = await _locationService.UpdateAsync(id, request);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _locationService.DeleteAsync(id);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }




}
