internal abstract class Vehicle
{
    private readonly int? _customSizeInUnits;

    public string RegistrationNumber { get; }
    public string Model { get; }
    public string Color { get; }
    public int NumberOfWheels { get; }
    public string TypeName => GetType().Name;

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