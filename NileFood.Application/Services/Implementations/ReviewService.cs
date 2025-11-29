using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.Reviews;
using NileFood.Application.Services.Interfaces;
using NileFood.Domain.Entities;
using NileFood.Domain.Errors;
using NileFood.Infrastructure.Data;

namespace NileFood.Application.Services.Implementations;
public class ReviewService(ApplicationDbContext context) : IReviewService
{
    private readonly ApplicationDbContext _context = context;


    public async Task<Result<List<ReviewResponse>>> GetAllForUserAsync(string userId)
    {
        var reviews = await _context.Reviews
            .Where(x => x.UserId == userId)
            .ProjectToType<ReviewResponse>()
            .AsNoTracking()
            .ToListAsync();

        return Result.Success(reviews);
    }

    public async Task<Result<ReviewResponse>> GetAsync(int id)
    {
        var review = await _context.Reviews
            .Where(x => x.Id == id)
            .ProjectToType<ReviewResponse>()
            .FirstOrDefaultAsync();

        return review is null ? Result.Failure<ReviewResponse>(ReviewsErrors.ReviewNotFound) : Result.Success(review);
    }


    public async Task<Result<ReviewResponse>> CreateAsync(ReviewRequest request, string userId)
    {
        if (!await _context.Branches.AnyAsync(x => x.Id == request.BranchId))
            return Result.Failure<ReviewResponse>(ReviewsErrors.ReviewNotFound);

        var review = request.Adapt<Review>();
        review.UserId = userId;

        await _context.AddAsync(review);
        await _context.SaveChangesAsync();


        var reviewResponse = review.Adapt<ReviewResponse>();

        return Result.Success(reviewResponse);
    }


    public async Task<Result> UpdateAsync(int id, ReviewRequest request)
    {
        if (await _context.Reviews.FirstOrDefaultAsync(x => x.Id == id) is not { } review)
            return Result.Failure(ReviewsErrors.ReviewNotFound);

        request.BranchId = review.BranchId;

        request.Adapt(review);

        _context.Update(review);
        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        if (await _context.Reviews.FirstOrDefaultAsync(x => x.Id == id) is not { } review)
            return Result.Failure(CityErrors.CityNotFound);


        _context.Remove(review);
        await _context.SaveChangesAsync();

        return Result.Success();
    }



}
