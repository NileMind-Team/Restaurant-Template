using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.Users;
using NileFood.Application.Services.Interfaces;
using NileFood.Domain.Entities.Identity;
using NileFood.Domain.Errors;
using NileFood.Infrastructure.Data;

namespace NileFood.Application.Services.Implementations;

public class UserService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IFileService fileService, IRoleService roleService) : IUserService
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    private readonly IFileService _fileService = fileService;
    private readonly IRoleService _roleService = roleService;

    public async Task<Result<UserProfileResponse>> GetProfileAsync(string userId)
    {
        if (await _userManager.FindByIdAsync(userId) is not { } user)
            return Result.Failure<UserProfileResponse>(UserErrors.UserNotFound);

        var roles = await _userManager.GetRolesAsync(user);

        var userMapped = user.Adapt<UserProfileResponse>();
        userMapped.Roles = roles;

        return Result.Success(userMapped);
    }

    public async Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request)
    {
        if (await _userManager.FindByIdAsync(userId) is not { } user)
            return Result.Failure(UserErrors.UserNotFound);

        if (user.PhoneNumber != request.PhoneNumber && _userManager.Users.Any(x => x.PhoneNumber == request.PhoneNumber))
            return Result.Failure(UserErrors.PhoneNumberAlreadyExists);

        user = request.Adapt(user);

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));

            var error = new Error("User.UpdateFailed", $"Failed to update user: {errors}", StatusCodes.Status400BadRequest);

            return Result.Failure(error);
        }

        return Result.Success();
    }

    public async Task<Result<string>> ChangeImageAsync(string userId, ChangeProfileImageRequest request)
    {
        var user = await _userManager.Users.SingleAsync(x => x.Id == userId);

        await _fileService.DeleteAsync(user.ImageUrl);

        var imageUrl = await _fileService.UploadAsync(request.Image, "profiles");

        user.ImageUrl = imageUrl;
        await _userManager.UpdateAsync(user);

        return Result.Success(imageUrl);
    }

    public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId);

        var result = await _userManager.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);

        return result.Succeeded ? Result.Success() : Result.Failure(new Error(result.Errors.First().Code, result.Errors.First().Description, StatusCodes.Status400BadRequest));
    }



    public async Task<Result<UserResponse>> AddAsync(CreateUserRequest request)
    {
        var emailIsExists = await _userManager.Users.AnyAsync(x => x.Email == request.Email);
        if (emailIsExists)
            return Result.Failure<UserResponse>(UserErrors.DuplicatedEmail);

        var allowedRoles = (await _roleService.GetAllAsync()).Value.Select(x => x.Name);
        if (request.Roles.Except(allowedRoles).Any())
            return Result.Failure<UserResponse>(UserErrors.InvalidRoles);

        var user = request.Adapt<ApplicationUser>();
        user.EmailConfirmed = true;
        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRolesAsync(user, request.Roles);

            var response = user.Adapt<UserResponse>();
            response.Roles = request.Roles;

            return Result.Success(response);
        }

        var error = result.Errors.First();

        return Result.Failure<UserResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }

    public async Task<IEnumerable<UserResponse>> GetAllAsync()
    {
        var users = await _userManager.Users
            .ToListAsync();

        var userResponses = new List<UserResponse>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var userResponse = user.Adapt<UserResponse>();
            userResponse.Roles = await _userManager.GetRolesAsync(user);

            userResponses.Add(userResponse);
        }

        return userResponses;
    }

    public async Task<Result<string>> DeleteAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return Result.Failure<string>(UserErrors.UserNotFound);

        var result = await _userManager.DeleteAsync(user);

        return !result.Succeeded
            ? Result.Failure<string>(new Error("BadRequest", result.Errors.First().Description, StatusCodes.Status400BadRequest))
            : Result.Success("User deleted successfully");
    }

    public async Task<Result<UserResponse>> GetByIdAsync(string id)
    {
        if (await _userManager.FindByIdAsync(id) is not { } user)
            return Result.Failure<UserResponse>(UserErrors.UserNotFound);

        var roles = await _userManager.GetRolesAsync(user);

        var userMapped = user.Adapt<UserResponse>();
        userMapped.Roles = roles;

        return Result.Success(userMapped);
    }

    public async Task<bool> UserHasRoleAsync(string userId, string roleName)
    {
        return await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Join(
                _context.Roles,
                ur => ur.RoleId,
                r => r.Id,
                (ur, r) => r.Name
            )
            .AnyAsync(r => r == roleName);
    }


    public async Task<Result> AssignRoleAsync(string userId, string roleName)
    {

        if (await _userManager.FindByIdAsync(userId) is not { } user)
            return Result.Failure(UserErrors.UserNotFound);


        var allowedRoles = (await _roleService.GetAllAsync()).Value.Select(r => r.Name);
        if (!allowedRoles.Contains(roleName))
            return Result.Failure(new Error("InvalidRole", $"Role '{roleName}' does not exist.", StatusCodes.Status400BadRequest));


        if (await _userManager.IsInRoleAsync(user, roleName))
            return Result.Failure(new Error("AlreadyAssigned", $"User already has role '{roleName}'.", StatusCodes.Status400BadRequest));

        var result = await _userManager.AddToRoleAsync(user, roleName);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result.Failure(new Error("AssignRoleFailed", $"Failed to assign role: {errors}", StatusCodes.Status400BadRequest));
        }

        return Result.Success();
    }

    public async Task<List<string>> GetUserRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return user == null ? [] : (await _userManager.GetRolesAsync(user)).ToList();
    }
}
