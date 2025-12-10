using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NileFood.API.Extensions;
using NileFood.Application.Contracts.Orders;
using NileFood.Application.Services.Interfaces;
using NileFood.Domain.Consts;

namespace NileFood.API.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;

    [HttpGet]
    [Authorize(Roles = DefaultRoles.Admin.Name + "," + DefaultRoles.Restaurant.Name)]
    public async Task<IActionResult> GetAll(string? status, DateOnly? startRange, DateOnly? endRange, string? userId)
    {
        var result = await _orderService.GetAllAsync(status, startRange, endRange, userId);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpGet]
    public async Task<IActionResult> GetAllForUser(string? status, DateOnly? startRange, DateOnly? endRange)
    {
        var result = await _orderService.GetAllAsync(status, startRange, endRange, User.GetUserId());

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpGet("{id}")]
    [Authorize(Roles = DefaultRoles.Admin.Name + "," + DefaultRoles.Restaurant.Name)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _orderService.GetAsync(id);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByForUserId(int id)
    {
        var result = await _orderService.GetForUserAsync(id, User.GetUserId());

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }



    [HttpPost]
    public async Task<IActionResult> Add(OrderRequest request)
    {
        var result = await _orderService.CreateAsync(request, User.GetUserId());

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
