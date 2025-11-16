using Microsoft.AspNetCore.Mvc;
using NileFood.API.Extensions;
using NileFood.Application.Contracts.Authentication;
using NileFood.Application.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace NileFood.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromForm] LoginRequest request)
    {
        var result = await _authService.GetTokenAsync(request);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromForm] RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailRequest request)
    {
        var result = await _authService.ConfirmEmailAsync(request);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpGet]
    public async Task<IActionResult> CheckConfirmationEmail([FromQuery][EmailAddress] string email)
    {
        var result = await _authService.CheckEmailConfirmationAsync(email);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResendConfirmationEmail(ResendConfirmationEmailRequest request)
    {
        var result = await _authService.ResendConfirmationEmailAsync(request);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ForgetPassword(ForgetPasswordRequest request)
    {
        var result = await _authService.SendResetPasswordCodeAsync(request.Email);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
    {
        var result = await _authService.ResetPasswordAsync(request);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

}
