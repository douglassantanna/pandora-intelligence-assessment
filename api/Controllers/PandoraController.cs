using api.Authenticaion;
using api.Helpers;
using api.Models;
using api.Persistance;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/pandora")]
[ServiceFilter(typeof(ApiKeyAuthFilter))]
public class PandoraController : ControllerBase
{
    private readonly IOpenDataService _openData;
    private readonly Datacontext _context;
    private const int _maxPageSize = 20;


    public PandoraController(IOpenDataService openData,
                             Datacontext context)
    {
        _openData = openData;
        _context = context;
    }

    /// <summary>
    /// Get vehicle info by plate
    /// </summary>
    /// <param name="plate">Vehicle plate</param>
    /// <response code="200">Returns the vehicle info</response>
    /// <response code="404">If the vehicle is not found</response>
    /// <returns></returns>
    [HttpGet("{plate}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicleInfo(string plate)
    {
        var vehicle = await _openData.GetVehicleByPlate(plate);
        if (vehicle is null)
        {
            return NotFound();
        }
        return Ok(vehicle);
    }

    /// <summary>
    /// Get all vehicles from the database
    /// </summary>
    /// <param name="sort">Sort by plate</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <response code="200">Returns the vehicles</response>
    /// <returns></returns>
    [HttpGet("addedvehicles")]
    public ActionResult<(IEnumerable<VehicleDTO>, Pagination<VehicleDTO>)> QueryVehiclesFromDatabase(string sort = "desc",
                                                                                int pageIndex = 0,
                                                                                int pageSize = 10)
    {
        if (pageSize > _maxPageSize)
        {
            pageSize = _maxPageSize;
        }

        var collection = _context.Vehicles.Select(vehicle => new VehicleDTO
        {
            Id = vehicle.Id,
            Kenteken = vehicle.Kenteken,
            Merk = vehicle.Merk,
            Voertuigsoort = vehicle.Voertuigsoort,
            Eerste_kleur = vehicle.Eerste_kleur,
        }).AsQueryable<VehicleDTO>();

        if (sort == "desc")
        {
            collection = collection.OrderByDescending(c => c.Kenteken.ToLower());
        }
        else
        {
            collection = collection.OrderBy(c => c.Kenteken.ToLower());
        }
        var pagination = new Pagination<VehicleDTO>(collection,
                                                pageIndex,
                                                pageSize);

        return Ok(pagination);
    }

}
