using api.Models;

namespace api.Services;

public interface IOpenDataService
{
    Task<IEnumerable<Car>> GetVehicleByPlate(string plate);
}
