namespace GarageConsoleApp.Models
{
    internal class Airplane : Vehicle
    {
        public int NumberOfEngines { get; }
        public override string Describe() => NumberOfEngines == 1 ? "1 engine" : $"{NumberOfEngines} engines";
        protected override int DefaultSizeInUnits => ParkingUnits.FromSpaces(8);

        public Airplane(string registrationNumber, string model, string color, int numberOfWheels, int numberOfEngines, int? sizeInUnits = null) : base(registrationNumber, model, color, numberOfWheels, sizeInUnits)
        {
            NumberOfEngines = numberOfEngines;
        }
    }
}
