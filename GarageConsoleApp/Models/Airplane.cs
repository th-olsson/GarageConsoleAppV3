namespace GarageConsoleApp.Models
{
    internal class Airplane : Vehicle
    {
        protected override int DefaultSizeInUnits => ParkingUnits.FromSpaces(8);
        public int NumberOfEngines { get; }

        public Airplane(string registrationNumber, string model, string color, int numberOfWheels, int numberOfEngines, int? sizeInUnits = null) : base(registrationNumber, model, color, numberOfWheels, sizeInUnits)
        {
            NumberOfEngines = numberOfEngines;
        }
    }
}
