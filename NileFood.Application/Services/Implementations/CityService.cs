using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.Cities;
using NileFood.Application.Services.Interfaces;
using NileFood.Domain.Entities;
using NileFood.Domain.Errors;
using NileFood.Infrastructure.Data;

namespace NileFood.Application.Services.Implementations;

public class CityService(ApplicationDbContext context) : ICityService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<List<CityResponse>>> GetAllAsync()
    {
        var cities = await _context.Cities            
            .ProjectToType<CityResponse>()
            .AsNoTracking()
            .ToListAsync();

        return Result.Success(cities);
    }

    public async Task<Result<CityResponse>> GetAsync(int id)
    {
        var city = await _context.Cities
            .Where(x => x.Id == id)            
            .ProjectToType<CityResponse>()
            .FirstOrDefaultAsync();

        return city is null ? Result.Failure<CityResponse>(CityErrors.CityNotFound) : Result.Success(city);
    }

    public async Task<Result<CityResponse>> CreateAsync(CityRequest request)
    {
        var city = request.Adapt<City>();        

        await _context.Cities.AddAsync(city);
        await _context.SaveChangesAsync();

        
        var cityResponse = city.Adapt<CityResponse>();
        
        return Result.Success(cityResponse);
    }

    public async Task<Result> UpdateAsync(int id, CityRequest request)
    {
        if (await _context.Cities.FirstOrDefaultAsync(x => x.Id == id) is not { } city)
            return Result.Failure(CityErrors.CityNotFound);

        if(await _context.Cities.AnyAsync(x=>x.Name == request.Name && x.Id != id))
            return Result.Failure(CityErrors.CityNameAlreadyUsed);

        request.Adapt(city);

        _context.Update(city);
        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        if (await _context.Cities.FirstOrDefaultAsync(x => x.Id == id) is not { } city)
            return Result.Failure(CityErrors.CityNotFound);


        _context.Remove(city);
        await _context.SaveChangesAsync();

        return Result.Success();
    }        
}
