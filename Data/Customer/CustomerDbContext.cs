using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MultiTenantDbContext.Data.Customer;

public class CustomerDbContext : DbContext
{
    private readonly IEnumerable<IInterceptor> _interceptors;
    private readonly IConfiguration _configuration;

    public DbSet<Data> Data { get; set; }

    public CustomerDbContext(DbContextOptions<CustomerDbContext> options,
        IEnumerable<IInterceptor> interceptors = null,
        IConfiguration configuration = null) : base(options)
    {
        _interceptors = interceptors;
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured && _configuration != null)
        {
            if (_interceptors != null) optionsBuilder.AddInterceptors(_interceptors);
            var connectionString = _configuration.GetConnectionString("CustomerConnection");
            optionsBuilder.UseNpgsql(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Data>(entity =>
        {
            entity.HasKey(x => x.Id);
        });
    }
}
