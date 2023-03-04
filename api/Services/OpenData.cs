using api.Models;
using api.Repository;
using Flurl.Http;
using Microsoft.Extensions.Options;

namespace api.Services;
public class OpenDataService : IOpenDataService
{
    private readonly ICarRepository _carRepository;
    private readonly OpenDataSettings _openDataSettings;
    private readonly ILogger<OpenDataService> _logger;

    public OpenDataService(ICarRepository carRepository,
                   IOptions<OpenDataSettings> openDataSettings,
                   ILogger<OpenDataService> logger)
    {
        _openDataSettings = openDataSettings.Value;
        _carRepository = carRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Car>> GetVehicleByPlate(string plate)
    {
        _logger.LogInformation($"Getting vehicle by plate: {plate}");
        var httpResult = await $"{_openDataSettings.Url}?kenteken={plate.ToUpper()}"
        .GetJsonAsync<IEnumerable<Car>>();

        if (httpResult.Count() == 0)
        {
            _logger.LogInformation($"Vehicle with plate: {plate} was not fount in open data");
            return httpResult;

        }
        else
        {
            foreach (var car in httpResult)
            {
                if (await _carRepository.CarExists(car.Kenteken))
                {
                    _logger.LogInformation($"Vehicle with plate: {plate} already exists in database");
                    return httpResult;
                }
            }
            _logger.LogInformation($"Vehicle with plate: {plate} does not exist in database, adding it to database");
            await _carRepository.AddRangeAsync(httpResult);
            return httpResult;
        }
    }
}