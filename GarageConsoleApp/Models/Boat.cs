namespace GarageConsoleApp.Models
{
    internal class Boat : Vehicle
    {
        public double Length { get; }
        public override string Describe() => $"{Length} m";
        protected override int DefaultSizeInUnits => ParkingUnits.FromSpaces(3);
        /// <summary> The length of the boat in meters </summary>

        /// <param name="length">The length of the boat in meters.</param>
        public Boat(string registrationNumber, string model, string color, double length, int? sizeInUnits = null) : base(registrationNumber, model, color, numberOfWheels: 0, sizeInUnits)
        {
            Length = length;
        }
    }
}
