using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiTenantDbContext.Data.Customer;
using MultiTenantDbContext.Factories;

namespace MultiTenantDbContext.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly CustomerDbContext _customerDbContext;

    public VehiclesController(CustomerDbContext customerDbContext)
    {
        _customerDbContext = customerDbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetVehicles([FromQuery] VehicleType vehicleType)
    {
        try
        {
            var query = VehicleQueryFactory.GetVehicleQuery(_customerDbContext, vehicleType);
            var result = await query.ToListAsync().ConfigureAwait(false);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
