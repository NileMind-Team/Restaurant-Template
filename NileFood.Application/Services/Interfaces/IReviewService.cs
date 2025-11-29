using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.Reviews;

namespace NileFood.Application.Services.Interfaces;
public interface IReviewService
{
    Task<Result<List<ReviewResponse>>> GetAllForUserAsync(string userId);
    Task<Result<ReviewResponse>> GetAsync(int id);
    Task<Result<ReviewResponse>> CreateAsync(ReviewRequest request, string userId);
    Task<Result> UpdateAsync(int id, ReviewRequest request);
    Task<Result> DeleteAsync(int id);
}
