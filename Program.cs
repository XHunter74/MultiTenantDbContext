using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MultiTenantDbContext;
using MultiTenantDbContext.Data.Admin;
using MultiTenantDbContext.Extensions;
using System.Reflection;
using System.Text;

namespace MultiTenantDbContext;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args)
            .Build()
            // Applying migrations
            .ApplyAdminMigrations()
            .Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}