using api.Models;

namespace api.Repository;
public interface ICarRepository
{
    Task AddRangeAsync(IEnumerable<Car> cars);
    Task<bool> CarExists(string plate);
}