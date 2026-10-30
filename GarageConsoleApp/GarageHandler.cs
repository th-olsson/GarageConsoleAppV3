using GarageConsoleApp.Models;
using GarageConsoleApp.Storage;

namespace GarageConsoleApp
{
    internal class GarageHandler : IHandler
    {
        private Garage<Vehicle>? _garage;

        // The open garage. The menus make sure there is one before they use it.
        private Garage<Vehicle> Current =>
            _garage ?? throw new InvalidOperationException("There is no garage yet.");
        public bool HasGarage => _garage != null;
        public string Name => Current.Name;
        public int Capacity => Current.Capacity;
        public int UsedUnits => Current.UsedUnits;

        public void LoadGarage() => _garage = GarageStorage.Load();
        public int CreateGarage(string name, int capacity, bool populate)
        {
            var garage = new Garage<Vehicle>(name, capacity);

            // Every garage starts with the example vehicles in its register
            foreach (Vehicle vehicle in VehicleRegistry.CreateSeed())
                garage.Register(vehicle);

            int parked = populate ? garage.ParkInRandomOrder(garage.GetRegisteredVehicles()) : 0;

            _garage = garage;
            GarageStorage.Save(garage);
            return parked;
        }
        public void DeleteGarage()
        {
            GarageStorage.Delete();
            _garage = null;
        }
        public List<Vehicle> GetRegisteredVehicles() => Current.GetRegisteredVehicles();
        public List<Vehicle> GetParkedVehicles() => Current.GetParkedVehicles();
        public bool IsRegistered(string registrationNumber) => Current.IsRegistered(registrationNumber);
        public bool IsParked(string registrationNumber) => Current.IsParked(registrationNumber);
        public ParkResult AddVehicle(Vehicle vehicle)
        {
            Current.Register(vehicle);
            ParkResult result = Current.Park(vehicle);
            GarageStorage.Save(Current);
            return result;
        }
        public bool CanHold(List<Vehicle> vehicles) => Current.CanHold(vehicles);
        public List<ParkingReport> SetParked(List<Vehicle> wanted)
        {
            List<ParkingReport> reports = Current.SetParked(wanted);
            GarageStorage.Save(Current);
            return reports;
        }
        public Vehicle? Find(string registrationNumber) => Current.Find(registrationNumber);
        public List<Vehicle> Search(VehicleFilter filter) => Current.Search(filter);
        public List<(string TypeName, int Count)> CountByType() => Current.CountByType();
    }
}