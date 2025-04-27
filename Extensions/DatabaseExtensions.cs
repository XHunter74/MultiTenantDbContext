using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MultiTenantDbContext.Data.Admin;
using MultiTenantDbContext.Data.Customer;
using MultiTenantDbContext.Interceptors;
using MultiTenantDbContext.Services;

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

    public static void ConfigureCustomerContext(this IServiceCollection services)
    {
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<ICustomerIdService, CustomerIdService>();
        services.AddTransient<IInterceptor, ContextInterceptor>();
        services.AddDbContext<CustomerDbContext>();
    }

    public static IHost RunCustomerDbAction(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        var customerConnectionString = configuration.GetConnectionString("CustomerConnection");
        var adminDbContext = scope.ServiceProvider.GetRequiredService<AdminDbContext>();

        var customers = adminDbContext.Customers.ToArray();

        foreach (var customer in customers)
        {
            try
            {
                var connectionString = string.Format(customerConnectionString, customer.Id);
                var customerOptionsBuilder = new DbContextOptionsBuilder<CustomerDbContext>();
                customerOptionsBuilder.UseNpgsql(connectionString);
                using var customerDbContext = new CustomerDbContext(customerOptionsBuilder.Options, null);
                customerDbContext.Database.Migrate();

                var data = customerDbContext.Data.ToArray();
                if (data.Length == 0)
                {
                    var newData = new Data.Customer.Data
                    {
                        Id = Guid.NewGuid(),
                        Text = customer.Name
                    };
                    customerDbContext.Data.Add(newData);
                    customerDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Startup.ConfigureCustomerData -> " +
                              $"Exception occurred for database migration 'Test.{customer.Id}': \r\n" + ex.Message);
            }
        }

        return host;
    }
}
