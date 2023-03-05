using api.Models;
using api.Persistance;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;
public class VehicleRepository : IVehicleRepository
{
    private readonly Datacontext _context;
    public VehicleRepository(Datacontext context) =>
        _context = context;

    public async Task AddRangeAsync(IEnumerable<Vehicle> vehicles)
    {
        _context.Vehicles.AddRange(vehicles);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> VehicleExists(string plate)
    {
        return await _context.Vehicles.AnyAsync(c => c.Kenteken == plate);
    }
}