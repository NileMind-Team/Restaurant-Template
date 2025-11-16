using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NileFood.API.Extensions;
using NileFood.Application.Contracts.Users;
using NileFood.Application.Services.Interfaces;

namespace NileFood.API.Controllers;


[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class AccountController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;


    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var result = await _userService.GetProfileAsync(User.GetUserId());
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpPut]
    public async Task<IActionResult> UpdateProfile(UpdateProfileRequest request)
    {
        var result = await _userService.UpdateProfileAsync(User.GetUserId(), request);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }


    [HttpPut]
    public async Task<IActionResult> ChangeImage([FromForm] ChangeProfileImageRequest image)
    {
        var result = await _userService.ChangeImageAsync(User.GetUserId(), image);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPut]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var result = await _userService.ChangePasswordAsync(User.GetUserId(), request);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
}
