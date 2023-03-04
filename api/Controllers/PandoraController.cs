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
    /// Get car info by plate
    /// </summary>
    /// <param name="plate">Car plate</param>
    /// <response code="200">Returns the car info</response>
    /// <response code="404">If the car is not found</response>
    /// <returns></returns>
    [HttpGet("{plate}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<IEnumerable<Car>>> GetCarInfo(string plate)
    {
        var car = await _openData.GetVehicleByPlate(plate);
        if (car.Count() == 0)
        {
            return NotFound();
        }
        return Ok(car);
    }

    /// <summary>
    /// Get all cars from the database
    /// </summary>
    /// <param name="sort">Sort by plate</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <response code="200">Returns the cars</response>
    /// <returns></returns>
    [HttpGet("addedcars")]
    public ActionResult<(IEnumerable<CarDTO>, Pagination<CarDTO>)> GetAddedCars(string sort = "desc",
                                                                                int pageIndex = 1,
                                                                                int pageSize = 10)
    {
        if (pageSize > _maxPageSize)
        {
            pageSize = _maxPageSize;
        }

        var collection = _context.Cars.Select(car => new CarDTO
        {
            Id = car.Id,
            Kenteken = car.Kenteken,
            Merk = car.Merk,
            Voertuigsoort = car.Voertuigsoort,
            Eerste_kleur = car.Eerste_kleur,
        }).AsQueryable<CarDTO>();

        if (sort == "desc")
        {
            collection = collection.OrderByDescending(c => c.Kenteken.ToLower());
        }
        else
        {
            collection = collection.OrderBy(c => c.Kenteken.ToLower());
        }
        var pagination = new Pagination<CarDTO>(collection,
                                                pageIndex,
                                                pageSize);

        return Ok(pagination);
    }

}
