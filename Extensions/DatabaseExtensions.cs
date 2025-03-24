using Microsoft.EntityFrameworkCore;
using MultiTenantDbContext.Data.Admin;

namespace MultiTenantDbContext.Extensions;

public static class DatabaseExtensions
{
    public static IHost ApplyAdminMigrations(this IHost host)
    {
        using var serviceScope = host.Services.CreateScope();
        using var context = (DbContext)serviceScope.ServiceProvider.GetRequiredService<AdminDbContext>();

        context.Database.Migrate();

        return host;
    }
}
