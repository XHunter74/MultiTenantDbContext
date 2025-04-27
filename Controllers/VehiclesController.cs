using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiTenantDbContext.CQRS;
using MultiTenantDbContext.Data.Customer;
using MultiTenantDbContext.Factories;
using MultiTenantDbContext.Features.Queries;

namespace MultiTenantDbContext.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly IQueryHandler<GetVehiclesQuery, List<object>> _getVehiclesQueryHandler;

    public VehiclesController(IQueryHandler<GetVehiclesQuery, List<object>> getVehiclesQueryHandler
        )
    {
        _getVehiclesQueryHandler = getVehiclesQueryHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetVehicles([FromQuery] VehicleType vehicleType)
    {
        try
        {
            var query = new GetVehiclesQuery(vehicleType);
            var result = await _getVehiclesQueryHandler.HandleAsync(query);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
