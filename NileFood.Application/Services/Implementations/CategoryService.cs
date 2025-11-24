using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.Categories;
using NileFood.Application.Services.Interfaces;
using NileFood.Domain.Entities;
using NileFood.Domain.Errors;
using NileFood.Infrastructure.Data;

namespace NileFood.Application.Services.Implementations;
public class CategoryService(ApplicationDbContext context) : ICategoryService
{
    private readonly ApplicationDbContext _context = context;


    public async Task<Result<List<CategoryResponse>>> GetAllAsync()
    {
        var categories = await _context.Categories
            .ProjectToType<CategoryResponse>()
            .AsNoTracking()
            .ToListAsync();

        return Result.Success(categories);
    }

    public async Task<Result<CategoryResponse>> GetAsync(int id)
    {
        var category = await _context.Categories
            .Where(x => x.Id == id)
            .ProjectToType<CategoryResponse>()
            .FirstOrDefaultAsync();

        return category is null ? Result.Failure<CategoryResponse>(CategoryErrors.CategoryNotFound) : Result.Success(category);
    }

    public async Task<Result<CategoryResponse>> CreateAsync(CategoryRequest request)
    {
        if (await _context.Categories.AnyAsync(x => x.Name == request.Name))
            return Result.Failure<CategoryResponse>(CategoryErrors.CategoryNameAlreadyUsed);

        var category = request.Adapt<Category>();

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();


        var categoryResponse = category.Adapt<CategoryResponse>();

        return Result.Success(categoryResponse);
    }

    public async Task<Result> UpdateAsync(int id, CategoryRequest request)
    {
        if (await _context.Categories.FirstOrDefaultAsync(x => x.Id == id) is not { } category)
            return Result.Failure(CategoryErrors.CategoryNotFound);

        if (await _context.Categories.AnyAsync(x => x.Name == request.Name && x.Id != id))
            return Result.Failure(CategoryErrors.CategoryNameAlreadyUsed);

        request.Adapt(category);

        _context.Update(category);
        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> ChangeCategoryActiveStatusAsync(int id)
    {
        if (id <= 0)
            return Result.Failure(CategoryErrors.InvalidCategoryId);

        if (await _context.Categories.FirstOrDefaultAsync(x => x.Id == id) is not { } category)
            return Result.Failure(CategoryErrors.CategoryNotFound);

        category.IsActive = !category.IsActive;
        await _context.SaveChangesAsync();

        return Result.Success();
    }
}
