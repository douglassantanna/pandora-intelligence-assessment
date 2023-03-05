using api.Models;

namespace api.Repository;
public interface IVehicleRepository
{
    Task AddRangeAsync(IEnumerable<Vehicle> vehicles);
    Task<bool> VehicleExists(string plate);
}