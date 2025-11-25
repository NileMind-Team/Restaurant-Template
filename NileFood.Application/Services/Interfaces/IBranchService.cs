using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.Branches;

namespace NileFood.Application.Services.Interfaces;
public interface IBranchService
{
    Task<Result<List<BranchResponse>>> GetAllAsync();
    Task<Result<BranchResponse>> GetAsync(int id);
    Task<Result<BranchResponse>> CreateAsync(BranchRequest request);
    Task<Result> UpdateAsync(int id, BranchRequest request);
    Task<Result> ChangeActiveStatusAsync(int id);
}
