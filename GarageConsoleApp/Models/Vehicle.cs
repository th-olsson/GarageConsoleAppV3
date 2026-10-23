internal abstract class Vehicle
{
    private readonly int? _customSizeInUnits;
    /// <summary>The names of all concrete vehicle types, e.g. "Car" and "Boat".</summary>
    public static IReadOnlyList<string> TypeNames { get; } =
        typeof(Vehicle).Assembly.GetTypes()
            .Where(type => type.IsSubclassOf(typeof(Vehicle)) && !type.IsAbstract)
            .Select(type => type.Name)
            .OrderBy(name => name)
            .ToList();
    public string RegistrationNumber { get; }
    public string Model { get; }
    public string Color { get; }
    public int NumberOfWheels { get; }
    public string TypeName => GetType().Name;
    /// <summary>Details specific to the vehicle type, e.g. "Diesel" or "9.5 m".</summary>
    public abstract string Describe();

    /// <summary>The size a vehicle of this type has unless a custom size is given.</summary>
    protected abstract int DefaultSizeInUnits { get; }

    /// <summary>
    /// Space the vehicle needs, in thirds of a parking space (3 = one full space).
    /// </summary>
    public int SizeInUnits => _customSizeInUnits ?? DefaultSizeInUnits;

    protected Vehicle(string registrationNumber, string model, string color, int numberOfWheels, int? sizeInUnits)
    {
        RegistrationNumber = registrationNumber;
        Model = model;
        Color = color;
        NumberOfWheels = numberOfWheels;
        _customSizeInUnits = sizeInUnits;
    }
}