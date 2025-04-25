namespace MultiTenantDbContext.Data.Customer;

public abstract class Vehicle
{
    public Guid Id { get; set; }
    public string Model { get; set; } = string.Empty;
}

public class Car : Vehicle
{
    public int NumberOfDoors { get; set; }
}

public class Truck : Vehicle
{
    public int LoadCapacity { get; set; }
}

public class Motorcycle : Vehicle
{
    public bool HasSidecar { get; set; }
}
