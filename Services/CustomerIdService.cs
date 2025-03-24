
namespace MultiTenantDbContext.Services;

public class CustomerIdService : ICustomerIdService
{
    public Guid? CustomerId { get; set; }
}
