using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.Categories;

namespace NileFood.Application.Services.Interfaces;
public interface ICategoryService
{
    Task<Result<List<CategoryResponse>>> GetAllAsync();
    Task<Result<CategoryResponse>> GetAsync(int id);
    Task<Result<CategoryResponse>> CreateAsync(CategoryRequest request);
    Task<Result> UpdateAsync(int id, CategoryRequest request);
    Task<Result> ChangeCategoryActiveStatusAsync(int id);
}
