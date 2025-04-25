using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiTenantDbContext.Data.Admin;
using MultiTenantDbContext.Data.Customer;
using MultiTenantDbContext.Services;

namespace MultiTenantDbContext.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerDbContext _customerDbContext;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly AdminDbContext _adminDbContext;

        public CustomersController(CustomerDbContext customerDbContext,
            AdminDbContext adminDbContext,
            IServiceScopeFactory scopeFactory)
        {
            _customerDbContext = customerDbContext;
            _scopeFactory = scopeFactory;
            _adminDbContext = adminDbContext;
        }

        [HttpGet("data")]
        public async Task<IActionResult> Get()
        {
            var data = await _customerDbContext.Data
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
            return Ok(data);
        }

        [HttpGet("all-data")]
        public async Task<IActionResult> GetAllCustomersData()
        {
            var data = new List<Data.Customer.Data>();
            var customers = await _adminDbContext.Customers
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
            foreach (var customer in customers)
            {
                using var scope = _scopeFactory.CreateScope();
                var customerIdService = scope.ServiceProvider.GetRequiredService<ICustomerIdService>();
                customerIdService.CustomerId = customer.Id;
                var customerDbContext = scope.ServiceProvider.GetRequiredService<CustomerDbContext>();
                var customerData = await customerDbContext.Data
                    .AsNoTracking()
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);
                data.Add(customerData);
            }
            return Ok(data);
        }
    }
}
