using Microsoft.EntityFrameworkCore.Diagnostics;
using MultiTenantDbContext.Data.Admin;
using MultiTenantDbContext.Services;
using System.Data.Common;

namespace MultiTenantDbContext.Interceptors;

public class ContextInterceptor : IDbConnectionInterceptor
{
    private readonly AdminDbContext _context;
    private readonly ICustomerIdService _customerIdService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ContextInterceptor(IHttpContextAccessor httpContextAccessor, ICustomerIdService customerIdService,
    AdminDbContext context)
    {
        _context = context;
        _customerIdService = customerIdService;
        _httpContextAccessor = httpContextAccessor;
    }
    void IDbConnectionInterceptor.ConnectionClosed(DbConnection connection, ConnectionEndEventData eventData)
    {
        // Do nothing
    }

    Task IDbConnectionInterceptor.ConnectionClosedAsync(DbConnection connection, ConnectionEndEventData eventData)
    {
        return Task.CompletedTask;
    }

    InterceptionResult IDbConnectionInterceptor.ConnectionClosing(DbConnection connection,
        ConnectionEventData eventData, InterceptionResult result)
    {
        return result;
    }

    ValueTask<InterceptionResult> IDbConnectionInterceptor.ConnectionClosingAsync(DbConnection connection,
        ConnectionEventData eventData, InterceptionResult result)
    {
        return new ValueTask<InterceptionResult>(result);
    }

    void IDbConnectionInterceptor.ConnectionFailed(DbConnection connection, ConnectionErrorEventData eventData)
    {
        // Do nothing
    }

    Task IDbConnectionInterceptor.ConnectionFailedAsync(DbConnection connection, ConnectionErrorEventData eventData,
        CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    void IDbConnectionInterceptor.ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
    {
        // Do nothing
    }

    Task IDbConnectionInterceptor.ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData,
        CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    InterceptionResult IDbConnectionInterceptor.ConnectionOpening(DbConnection connection,
        ConnectionEventData eventData, InterceptionResult result)
    {
        ProcessChangeConnectionString(connection);
        return result;
    }

    ValueTask<InterceptionResult> IDbConnectionInterceptor.ConnectionOpeningAsync(DbConnection connection,
        ConnectionEventData eventData, InterceptionResult result, CancellationToken cancellationToken)
    {
        ProcessChangeConnectionString(connection);
        return new ValueTask<InterceptionResult>(result);
    }

    private DbConnection ProcessChangeConnectionString(DbConnection connection)
    {
        if (!connection.ConnectionString.Contains("{0}")) return connection;

        connection.ConnectionString =
            string.Format(connection.ConnectionString, GetCustomerId());

        return connection;
    }

    private Guid GetCustomerId()
    {
        if (_customerIdService.CustomerId.HasValue)
        {
            return _customerIdService.CustomerId.Value;
        }

        var customerId = GetCustomerIdFromRequest(_httpContextAccessor?.HttpContext);
        return customerId;
    }

    private Guid GetCustomerIdFromRequest(HttpContext httpContext)
    {
        var customerId = Guid.Empty;
        if (httpContext == null) throw new Exception("Could not get customer Id because HttpContext is null");
        if (httpContext.User.Identity.IsAuthenticated && httpContext.User.Identity.Name != null)
        {
            var userName = httpContext.User.Identity.Name;
            if (httpContext.User.Claims.Any(e => e.Type == Constants.CustomerIdClaims))
            {
                var customerIdClaims = httpContext.User.Claims
                    .FirstOrDefault(e => e.Type == Constants.CustomerIdClaims);
                if (customerIdClaims != null) customerId = new Guid(customerIdClaims.Value);
            }
            else
            {
                var user = _context.Users
                    .FirstOrDefault(e => e.Name.ToLower() == userName);
                if (user != null) customerId = user.CustomerId;
            }
        }

        if (customerId == Guid.Empty) throw new Exception("Could not get customer Id from HttpContext");
        return customerId;
    }
}