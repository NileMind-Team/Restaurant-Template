using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NileFood.API.Extensions;
using NileFood.Application.Contracts.Branches;
using NileFood.Application.Services.Interfaces;
using NileFood.Domain.Consts;


namespace NileFood.API.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
[Authorize(Roles = $"{DefaultRoles.Admin.Name}")]
public class BranchesController(IBranchService branchService) : ControllerBase
{
    private readonly IBranchService _branchService = branchService;



    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _branchService.GetAllAsync();

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _branchService.GetAsync(id);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpPost]
    public async Task<IActionResult> Add([FromBody] BranchRequest request)
    {
        var result = await _branchService.CreateAsync(request);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] BranchRequest request)
    {
        var result = await _branchService.UpdateAsync(id, request);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> ChangeActiveStatus(int id)
    {
        var result = await _branchService.ChangeActiveStatusAsync(id);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

}
