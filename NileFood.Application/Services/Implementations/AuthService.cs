using Hangfire;
using Microsoft.AspNetCore.WebUtilities;
using NileFood.Application.Abstractions;
using NileFood.Application.Authentication;
using NileFood.Application.Contracts.Authentication;
using NileFood.Application.Services.Interfaces;
using NileFood.Domain.Consts;
using NileFood.Domain.Entities.Identity;
using NileFood.Domain.Errors;
using NileFood.Domain.Helpers;
using NileFood.Infrastructure.Data;

namespace NileFood.Application.Services.Implementations;
public class AuthService(UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ApplicationDbContext context,
    IEmailSender _emailSender,
    IHttpContextAccessor _httpContextAccessor,
    IJwtProvider jwtProvider) : IAuthService
{


    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly ApplicationDbContext _context = context;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task<Result<string>> ConfirmEmailAsync(ConfirmEmailRequest request)
    {
        if (await _userManager.FindByIdAsync(request.UserId) is not { } user)
            return Result.Failure<string>(UserErrors.InvalidCode);

        if (user.EmailConfirmed)
            return Result.Failure<string>(UserErrors.DuplicatedConfirmation);

        var code = request.Code;
        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        }
        catch (FormatException)
        {
            return Result.Failure<string>(UserErrors.InvalidCode);
        }

        var result = await _userManager.ConfirmEmailAsync(user, code);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, DefaultRoles.User.Name);
            return Result.Success("Email confirmed successfully");
        }


        var error = result.Errors.First();
        return Result.Failure<string>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }

    public async Task<Result<string>> CheckEmailConfirmationAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email) is not { } user
            ? Result.Failure<string>(UserErrors.UserNotFound)
            : user.EmailConfirmed
                ? Result.Success("Email is confirmed")
                : Result.Failure<string>(UserErrors.EmailNotConfirmed);
    }

    public async Task<Result<AuthResponse>> GetTokenAsync(LoginRequest request)
    {

        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);
        if (result.Succeeded)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var jwtResult = _jwtProvider.GenerateToken(user, userRoles);

            var response = new AuthResponse(user.Id, user.Email!, user.FirstName, user.LastName, jwtResult.token, jwtResult.expiresIn);

            return Result.Success(response);
        }

        var error = result.IsNotAllowed ?
              UserErrors.EmailNotConfirmed
            : result.IsLockedOut ? UserErrors.LockedUser : UserErrors.InvalidCredentials;

        return Result.Failure<AuthResponse>(error);
    }

    public async Task<Result<string>> RegisterAsync(RegisterRequest request)
    {
        var user = request.Adapt<ApplicationUser>();

        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            await SendConfirmationEmail(user, code);

            return Result.Success("account created successfully, please check your email to confirm your email address");
        }

        var error = result.Errors.First();
        return Result.Failure<string>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }


    public async Task<Result<string>> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Failure<string>(UserErrors.UserNotFound);

        if (user.EmailConfirmed)
            return Result.Failure<string>(UserErrors.DuplicatedConfirmation);

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        await SendConfirmationEmail(user, code);

        return Result.Success("email sent successfully, please check your email to confirm your email address");
    }

    public async Task<Result<string>> SendResetPasswordCodeAsync(string email)
    {
        if (await _userManager.FindByEmailAsync(email) is not { } user)
            return Result.Failure<string>(UserErrors.UserNotFound);

        if (!user.EmailConfirmed)
            return Result.Failure<string>(UserErrors.EmailNotConfirmed);

        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        await SendResetPasswordEmail(user, code);

        return Result.Success("email sent successfully, please check your email to reset your password");
    }

    public async Task<Result<string>> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null || !user.EmailConfirmed)
            return Result.Failure<string>(UserErrors.InvalidCode);

        IdentityResult result;

        try
        {
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));

            result = await _userManager.ResetPasswordAsync(user, code, request.NewPassword);
        }
        catch (FormatException)
        {
            result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
        }


        if (result.Succeeded)
            return Result.Success<string>("Password reset successfully");


        var error = result.Errors.First();
        return Result.Failure<string>(new Error(error.Code, error.Description, StatusCodes.Status401Unauthorized));
    }



    private async Task SendConfirmationEmail(ApplicationUser user, string code)
    {
        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

        var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation.html",
            templateModel: new Dictionary<string, string>
            {
                { "{{name}}", user.UserName! },
                { "{{action_url}}", $"{origin}/auth/verify-email-address?userId={user.Id}&code={code}" },
            }

        );

        BackgroundJob.Enqueue(() =>
            _emailSender.SendEmailAsync(user.Email!, "✅ NileMind : Email Confirmation", emailBody)
        );

        await Task.CompletedTask;
    }

    private async Task SendResetPasswordEmail(ApplicationUser user, string code)
    {
        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

        var emailBody = EmailBodyBuilder.GenerateEmailBody("ForgetPassword.html",
            templateModel: new Dictionary<string, string>
            {
                {"{{name}}" , user.UserName! },
                {"{{action_url}}" , $"{origin}/reset-password?email={user.Email}&code={code}" },
            }

        );


        BackgroundJob.Enqueue(() =>
            _emailSender.SendEmailAsync(user.Email!, "✅ NileMind: Change Password", emailBody)
        );

        await System.Threading.Tasks.Task.CompletedTask;
    }


}
