using api.Contracts;
using api.DTO;
using api.Helpers;
using api.Models;
using api.Persistance;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/pandora")]
public class PandoraController : ControllerBase
{
    private readonly IOpenData _openData;
    private readonly Datacontext _context;
    private const int _maxPageSize = 20;


    public PandoraController(IOpenData openData,
                             Datacontext context)
    {
        _openData = openData;
        _context = context;
    }
    [HttpGet("{plate}")]
    public async Task<IEnumerable<Car>> GetCarInfo(string plate)
    {
        return await _openData.GetVehiacleByPlate(plate);
    }

    [HttpGet]
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
            Plate = car.Kenteken,
            Make = car.Merk,
            VehicleType = car.Voertuigsoort,
            FirstColor = car.Eerste_kleur,
        }).AsQueryable<CarDTO>();

        if (sort == "desc")
        {
            collection = collection.OrderByDescending(c => c.Plate.ToLower());
        }
        else
        {
            collection = collection.OrderBy(c => c.Plate.ToLower());
        }
        var pagination = new Pagination<CarDTO>(collection,
                                                pageIndex,
                                                pageSize);

        return Ok(pagination);
    }

}
