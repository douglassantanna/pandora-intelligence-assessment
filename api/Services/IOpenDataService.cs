using api.Models;

namespace api.Services;

public interface IOpenDataService
{
    Task<IEnumerable<VehicleDTO>> GetVehicleByPlate(string plate);
}
