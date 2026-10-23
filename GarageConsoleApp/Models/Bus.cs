namespace GarageConsoleApp.Models
{
    internal class Bus : Vehicle
    {
        public int NumberOfSeats { get; }
        public override string Describe() => $"{NumberOfSeats} seats";
        protected override int DefaultSizeInUnits => ParkingUnits.FromSpaces(3);

        public Bus(string registrationNumber, string model, string color, int numberOfSeats, int numberOfWheels, int? sizeInUnits = null) : base(registrationNumber, model, color, numberOfWheels, sizeInUnits)
        {
            NumberOfSeats = numberOfSeats;
        }
    }
}
