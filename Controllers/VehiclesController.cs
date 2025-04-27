using CQRSMediatr.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenantDbContext.Data.Customer;
using MultiTenantDbContext.Features.Queries;

namespace MultiTenantDbContext.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly ICqrsMediatr _mediatr;

    public VehiclesController(ICqrsMediatr mediatr)
    {
        _mediatr = mediatr;
    }

    [HttpGet]
    public async Task<IActionResult> GetVehicles([FromQuery] VehicleType vehicleType)
    {
        try
        {
            var query = new GetVehiclesQuery(vehicleType);
            var result = await _mediatr.QueryAsync(query);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
