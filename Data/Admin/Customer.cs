namespace MultiTenantDbContext.Data.Admin;

public class Customer
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public ICollection<User> Users { get; set; } = [];
}
