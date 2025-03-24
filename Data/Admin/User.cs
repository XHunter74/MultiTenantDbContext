namespace MultiTenantDbContext.Data.Admin;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid CustomerId { get; set; }

    public Customer Customer { get; set; }
}
