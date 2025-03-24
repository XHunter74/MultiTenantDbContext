namespace MultiTenantDbContext.Services;

public interface ICustomerIdService
{
    public Guid? CustomerId { get; set; }
}
