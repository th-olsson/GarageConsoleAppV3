namespace GarageConsoleApp.Models
{
    internal class Car : Vehicle
    {
        public FuelType FuelType { get; }
        public override string Describe() => FuelType.ToString();
        protected override int DefaultSizeInUnits => ParkingUnits.FromSpaces(1);

        public Car(string registrationNumber, string model, string color,
            FuelType fuelType, int numberOfWheels = 4, int? sizeInUnits = null)
            : base(registrationNumber, model, color, numberOfWheels, sizeInUnits)
        {
            FuelType = fuelType;
        }
    }
}