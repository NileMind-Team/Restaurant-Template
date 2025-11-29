using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.Users;

namespace NileFood.Application.Services.Interfaces;

public interface IUserService
{
    Task<Result<UserProfileResponse>> GetProfileAsync(string userId);
    Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request);
    Task<Result<string>> ChangeImageAsync(string userId, ChangeProfileImageRequest image);
    Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request);


    Task<Result<UserResponse>> AddAsync(CreateUserRequest request);
    Task<IEnumerable<UserResponse>> GetAllAsync();

    Task<Result<UserResponse>> GetByIdAsync(string id);
    Task<Result<string>> DeleteAsync(string email);
    Task<List<string>> GetUserRolesAsync(string userId);


    Task<Result> AssignRoleAsync(string userId, string roleName);
    Task<bool> UserHasRoleAsync(string userId, string roleName);
}
