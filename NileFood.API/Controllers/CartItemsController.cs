using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NileFood.API.Extensions;
using NileFood.Application.Contracts.CartItems;
using NileFood.Application.Services.Interfaces;

namespace NileFood.API.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class CartItemsController(ICartItemService cartItemService) : ControllerBase
{
    private readonly ICartItemService _cartItemService = cartItemService;


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _cartItemService.GetAllForUserAsync(User.GetUserId());

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpPost]
    public async Task<IActionResult> AddCartItem(CartItemRequest request)
    {
        var result = await _cartItemService.CreateAsync(request, User.GetUserId());

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQuantity(int id, CartItemQuantityRequest request)
    {
        var result = await _cartItemService.UpdateQuantityAsync(id, request);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _cartItemService.DeleteAsync(id);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

}
