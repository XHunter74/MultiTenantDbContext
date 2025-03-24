using Microsoft.EntityFrameworkCore;

namespace MultiTenantDbContext.Data.Admin;

public class AdminDbContext : DbContext
{
    public AdminDbContext(DbContextOptions<AdminDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Customer>(entity =>
        {
            entity.HasKey(x => x.Id);
        });

        builder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasOne(x => x.Customer)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.CustomerId);
        });
    }
}
