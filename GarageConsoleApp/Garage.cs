using GarageConsoleApp.Models;

namespace GarageConsoleApp
{
    internal class Garage
    {
        private readonly List<Vehicle> _vehicles;
        public string Name { get; }
        /// <summary>The number of parking spaces.</summary>
        public int Capacity { get; private set; }
        public int TotalUnits => ParkingUnits.FromSpaces(Capacity);
        public int UsedUnits
        {
            get
            {
                int used = 0;
                foreach (Vehicle? vehicle in _vehicles)
                {
                    if (vehicle != null)
                        used += vehicle.SizeInUnits;
                }
                return used;
            }
        }
        public int FreeUnits => TotalUnits - UsedUnits;

        public Garage(string name, int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than 0.");

            Name = name;
            Capacity = capacity;
            _vehicles = [];
        }

        public bool IsParked(string registrationNumber) => FindIndex(registrationNumber) != -1;
        public bool Remove(string registrationNumber)
        {
            int index = FindIndex(registrationNumber);
            if (index == -1)
                return false;

            _vehicles.RemoveAt(index);
            return true;
        }
        public ParkResult Park(Vehicle vehicle)
        {
            if (FindIndex(vehicle.RegistrationNumber) != -1)
                return ParkResult.AlreadyParked;

            if (vehicle.SizeInUnits > FreeUnits)
                return ParkResult.NotEnoughSpace;

            _vehicles.Add(vehicle);
            return ParkResult.Parked;
        }
        public List<Vehicle> GetParkedVehicles()
        {
            List<Vehicle> parked = [];

            foreach (Vehicle vehicle in _vehicles)
            {
                parked.Add(vehicle);
            }

            return parked;
        }
        public List<(string TypeName, int Count)> CountByType() =>
            _vehicles
                .GroupBy(vehicle => vehicle.TypeName)
                .Select(group => (TypeName: group.Key, Count: group.Count()))
                .ToList();
        public Vehicle? Find(string registrationNumber)
        {
            int index = FindIndex(registrationNumber);
            return index == -1 ? null : _vehicles[index];
        }
        public List<Vehicle> Search(VehicleFilter filter)
        {
            List<Vehicle> results = [];

            foreach (Vehicle vehicle in GetParkedVehicles())
            {
                if (filter.Matches(vehicle))
                {
                    results.Add(vehicle);
                }
            }

            return results;
        }
        private int FindIndex(string registrationNumber)
        {
            for (int i = 0; i < _vehicles.Count; i++)
            {
                Vehicle? vehicle = _vehicles[i];
                if (vehicle != null &&
                    vehicle.RegistrationNumber.Equals(registrationNumber, StringComparison.OrdinalIgnoreCase))
                    return i;
            }
            return -1;
        }
    }
}