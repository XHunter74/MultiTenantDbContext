using Microsoft.EntityFrameworkCore;
using MultiTenantDbContext.CQRS;
using MultiTenantDbContext.Data.Customer;
using MultiTenantDbContext.Factories;

namespace MultiTenantDbContext.Features.Queries;

public class GetVehiclesQuery : IQuery<List<object>>
{
    public VehicleType VehicleType { get; set; }

    public GetVehiclesQuery(VehicleType vehicleType)
    {
        VehicleType = vehicleType;
    }
}

public class GetVehiclesQueryHandler : IQueryHandler<GetVehiclesQuery, List<object>>
{
    private readonly CustomerDbContext _context;

    public GetVehiclesQueryHandler(CustomerDbContext context)
    {
        _context = context;
    }

    public async Task<List<object>> HandleAsync(GetVehiclesQuery query)
    {
        var contextQuery = VehicleQueryFactory.GetVehicleQuery(_context, query.VehicleType);
        var result = await contextQuery
            .AsNoTracking()
            .ToListAsync().ConfigureAwait(false);
        return result;
    }
}



