using api.Models;
using api.Persistance;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;
public class CarRepository : ICarRepository
{
    private readonly Datacontext _context;
    public CarRepository(Datacontext context) =>
        _context = context;

    public async Task AddRangeAsync(IEnumerable<Car> cars)
    {
        _context.Cars.AddRange(cars);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CarExists(string plate)
    {
        return await _context.Cars.AnyAsync(c => c.Kenteken == plate);
    }
}