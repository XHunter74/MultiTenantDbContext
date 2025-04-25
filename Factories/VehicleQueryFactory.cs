using MultiTenantDbContext.Data.Customer;

namespace MultiTenantDbContext.Factories;

public static class VehicleQueryFactory
{
    public static IQueryable<object> GetVehicleQuery(CustomerDbContext context, VehicleType vehicleType)
    {
        return vehicleType switch
        {
            VehicleType.Car => context.Vehicles.OfType<Car>(),
            VehicleType.Truck => context.Vehicles.OfType<Truck>(),
            VehicleType.Motorcycle => context.Vehicles.OfType<Motorcycle>(),
            _ => throw new ArgumentException("Unsupported vehicle type", nameof(vehicleType))
        };
    }
}
