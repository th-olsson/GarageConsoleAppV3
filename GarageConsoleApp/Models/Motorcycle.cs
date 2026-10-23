namespace GarageConsoleApp.Models
{
    internal class Motorcycle : Vehicle
    {
        protected override int DefaultSizeInUnits => 1; // a third of a parking space
        public TransmissionType TransmissionType { get; }

        public Motorcycle(string registrationNumber, string model, string color, TransmissionType transmissionType, int numberOfWheels = 2, int? sizeInUnits = null) : base(registrationNumber, model, color, numberOfWheels, sizeInUnits)
        {
            TransmissionType = transmissionType;
        }
    }
}
