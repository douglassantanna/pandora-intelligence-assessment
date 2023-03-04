using api.Models;

namespace api.Contracts;

public interface IOpenData
{
    Task<IEnumerable<Car>> GetVehiacleByPlate(string plate);
}
