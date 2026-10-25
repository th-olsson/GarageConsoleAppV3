namespace GarageConsoleApp
{
    internal enum ParkingChange { LeftGarage, Parked, NotEnoughSpace }

    /// <summary>What happened to one vehicle when the parking was updated.</summary>
    internal record ParkingReport(Vehicle Vehicle, ParkingChange Change);
}