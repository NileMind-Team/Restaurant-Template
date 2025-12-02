using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NileFood.API.Extensions;
using NileFood.Application.Contracts.ItemOffers;
using NileFood.Application.Services.Interfaces;
using NileFood.Domain.Consts;

namespace NileFood.API.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
[Authorize(Roles = $"{DefaultRoles.Restaurant.Name},{DefaultRoles.Admin.Name},{DefaultRoles.Branch.Name}")]
public class ItemOffersController(IItemOfferService itemOfferService) : ControllerBase
{
    private readonly IItemOfferService _itemOfferService = itemOfferService;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var result = await _itemOfferService.GetAllActiveAsync();

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _itemOfferService.GetAsync(id);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost]
    public async Task<IActionResult> Add(ItemOfferRequest request)
    {
        var result = await _itemOfferService.CreateAsync(request);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }



    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ItemOfferRequest request)
    {
        var result = await _itemOfferService.UpdateAsync(id, request);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _itemOfferService.DeleteAsync(id);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
