namespace GarageConsoleApp.Models
{
    internal class Motorcycle : Vehicle
    {
        public TransmissionType TransmissionType { get; }
        public override string Describe() => TransmissionType.ToString();
        protected override int DefaultSizeInUnits => 1; // a third of a parking space

        public Motorcycle(string registrationNumber, string model, string color, TransmissionType transmissionType, int numberOfWheels = 2, int? sizeInUnits = null) : base(registrationNumber, model, color, numberOfWheels, sizeInUnits)
        {
            TransmissionType = transmissionType;
        }
    }
}
