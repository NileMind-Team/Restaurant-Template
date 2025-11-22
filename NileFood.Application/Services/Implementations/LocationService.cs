using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.Locations;
using NileFood.Application.Services.Interfaces;
using NileFood.Domain.Entities;
using NileFood.Domain.Errors;
using NileFood.Infrastructure.Data;


namespace NileFood.Application.Services.Implementations;

public class LocationService(ApplicationDbContext context) : ILocationService
{
    private readonly ApplicationDbContext _context = context;

    
    public async Task<Result<List<LocationResponse>>> GetAllForUserAsync(string userId)
    {
        var locations = await _context.Locations
            .Where(x => x.UserId == userId)
            .Include(x=>x.User)
            .Include(x=>x.City)            
            .ProjectToType<LocationResponse>()
            .AsNoTracking()
            .ToListAsync();

        return Result.Success(locations);
    }

    public async Task<Result<LocationResponse>> GetAsync(int id)
    {
        var location = await _context.Locations
            .Where(x => x.Id == id)
            .Include(x => x.City)
            .ProjectToType<LocationResponse>()
            .FirstOrDefaultAsync();

        return location is null ? Result.Failure<LocationResponse>(LocationErrors.LocationNotFound) : Result.Success(location);
    }

    public async Task<Result<LocationResponse>> CreateAsync(string userId, LocationRequest request)
    {
        var location = request.Adapt<Location>();
        location.UserId = userId;
        
        await _context.Locations.AddAsync(location);
        await _context.SaveChangesAsync();
        
        await _context.Users
            .Where(u => u.Id == userId)
            .ExecuteUpdateAsync(op => op.SetProperty(u => u.DefaultLocationId, location.Id));
        
        var locationResponse = location.Adapt<LocationResponse>();
        locationResponse.IsDefaultLocation = true;
        return Result.Success(locationResponse);
    }


    public async Task<Result> UpdateAsync(int id, LocationRequest request)
    {
        if (await _context.Locations.FirstOrDefaultAsync(x => x.Id == id) is not { } location)
            return Result.Failure(LocationErrors.LocationNotFound);

        request.Adapt(location);

        _context.Update(location);
        await _context.SaveChangesAsync();

        return Result.Success();
    }


    public async Task<Result> DeleteAsync(int id)
    {
        if (await _context.Locations.FirstOrDefaultAsync(x => x.Id == id) is not { } location)
            return Result.Failure(LocationErrors.LocationNotFound);

        
        _context.Remove(id);
        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> ChangeDefaultLocationAsync(int locationId, string userId)
    {
        if (!await _context.Locations.Where(x=>x.UserId == userId).AnyAsync(x => x.Id == locationId))
            return Result.Failure(LocationErrors.LocationNotFound);


        await _context.Users
            .Where(u => u.Id == userId)
            .ExecuteUpdateAsync(op => op.SetProperty(u => u.DefaultLocationId, locationId));        

        return Result.Success();
    }



}
