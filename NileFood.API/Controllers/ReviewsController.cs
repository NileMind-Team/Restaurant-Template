using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NileFood.API.Extensions;
using NileFood.Application.Contracts.Reviews;
using NileFood.Application.Services.Interfaces;

namespace NileFood.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class ReviewsController(IReviewService reviewService) : ControllerBase
{
    private readonly IReviewService _reviewService = reviewService;


    [HttpGet]
    public async Task<IActionResult> GetAllForUser()
    {
        var result = await _reviewService.GetAllForUserAsync(User.GetUserId());

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _reviewService.GetAsync(id);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpPost]
    public async Task<IActionResult> Add(ReviewRequest request)
    {
        var result = await _reviewService.CreateAsync(request, User.GetUserId());

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }



    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ReviewRequest request)
    {
        var result = await _reviewService.UpdateAsync(id, request);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _reviewService.DeleteAsync(id);

        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
