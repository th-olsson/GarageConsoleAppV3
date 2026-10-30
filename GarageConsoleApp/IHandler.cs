using GarageConsoleApp.Models;

namespace GarageConsoleApp
{
    /// <summary>What the user interface can ask for. The UI only knows this interface.</summary>
    internal interface IHandler
    {
        // The current garage
        bool HasGarage { get; }
        string Name { get; }
        int Capacity { get; }
        int UsedUnits { get; }

        void LoadGarage();
        /// <summary>Creates and saves a new garage. Returns how many vehicles were parked.</summary>
        int CreateGarage(string name, int capacity, bool populate);
        void DeleteGarage();
        List<Vehicle> GetRegisteredVehicles();
        List<Vehicle> GetParkedVehicles();
        bool IsRegistered(string registrationNumber);
        bool IsParked(string registrationNumber);
        /// <summary>Registers the vehicle and tries to park it.</summary>
        ParkResult AddVehicle(Vehicle vehicle);
        bool CanHold(List<Vehicle> vehicles);
        List<ParkingReport> SetParked(List<Vehicle> wanted);
        Vehicle? Find(string registrationNumber);
        List<Vehicle> Search(VehicleFilter filter);
        List<(string TypeName, int Count)> CountByType();
    }
}