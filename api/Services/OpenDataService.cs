using api.Models;
using api.Repository;
using Flurl.Http;
using Microsoft.Extensions.Options;

namespace api.Services;
public class OpenDataService : IOpenDataService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly OpenDataSettings _openDataSettings;
    private readonly ILogger<OpenDataService> _logger;

    public OpenDataService(IVehicleRepository vehicleRepository,
                   IOptions<OpenDataSettings> openDataSettings,
                   ILogger<OpenDataService> logger)
    {
        _openDataSettings = openDataSettings.Value;
        _vehicleRepository = vehicleRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<VehicleDTO>> GetVehicleByPlate(string plate)
    {
        _logger.LogInformation($"Getting vehicle by plate: {plate}");
        var vehicles = await $"{_openDataSettings.Url}?kenteken={plate.ToUpper()}"
            .GetJsonAsync<IEnumerable<VehicleDTO>>();

        if (vehicles.Count() > 0)
        {
            List<Vehicle> vehiclesList = new();
            foreach (var vehicle in vehicles)
            {
                if (!await _vehicleRepository.VehicleExists(vehicle.Kenteken))
                {
                    _logger.LogInformation($"Vehicle with plate: {plate} does not exist in database, adding it to database");
                    vehiclesList.Add(new Vehicle(vehicle.Kenteken, vehicle.Merk, vehicle.Voertuigsoort, vehicle.Eerste_kleur));
                    continue;
                }
                _logger.LogInformation($"Vehicle with plate: {plate} already exists in database");
            }
            await _vehicleRepository.AddRangeAsync(vehiclesList);
        }
        else
        {
            _logger.LogInformation($"Vehicle with plate: {plate} was not found in open data");
            return null;
        }

        return vehicles;
    }
}