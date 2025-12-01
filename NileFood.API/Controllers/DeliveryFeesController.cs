using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NileFood.API.Extensions;
using NileFood.Application.Contracts.DeliveryFees;
using NileFood.Application.Services.Interfaces;
using NileFood.Domain.Consts;

namespace NileFood.API.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
[Authorize(Roles = $"{DefaultRoles.Restaurant.Name},{DefaultRoles.Admin.Name},{DefaultRoles.Branch.Name}")]
public class DeliveryFeesController(IDeliveryFeeService deliveryFeeService) : ControllerBase
{
    private readonly IDeliveryFeeService _deliveryFeeService = deliveryFeeService;



    [HttpGet]
    public async Task<IActionResult> GetAll(int? branchId)
    {
        var result = await _deliveryFeeService.GetAllAsync(branchId);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _deliveryFeeService.GetAsync(id);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpPost]
    public async Task<IActionResult> Add(DeliveryFeeRequest request)
    {
        var result = await _deliveryFeeService.CreateAsync(request);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }



    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, DeliveryFeeRequest request)
    {
        var result = await _deliveryFeeService.UpdateAsync(id, request);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _deliveryFeeService.DeleteAsync(id);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ChangeActiveStatus(int id)
    {
        var result = await _deliveryFeeService.ChangeActiveStatusAsync(id);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
