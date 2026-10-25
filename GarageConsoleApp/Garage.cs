using GarageConsoleApp.Models;

namespace GarageConsoleApp
{
    internal class Garage
    {
        private readonly List<Vehicle> _parked;
        private readonly List<Vehicle> _registered = [];
        public string Name { get; }
        /// <summary>The number of parking spaces.</summary>
        public int Capacity { get; }
        public int TotalUnits => ParkingUnits.FromSpaces(Capacity);
        public int UsedUnits => _parked.Sum(vehicle => vehicle.SizeInUnits);
        public int FreeUnits => TotalUnits - UsedUnits;

        public Garage(string name, int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than 0.");

            Name = name;
            Capacity = capacity;
            _parked = [];
        }

        /// <summary>All vehicles the garage knows about, parked or not.</summary>
        public List<Vehicle> GetRegisteredVehicles() => [.. _registered];
        public bool IsRegistered(string registrationNumber) =>
            _registered.Any(vehicle => vehicle.RegistrationNumber.Equals(registrationNumber, StringComparison.OrdinalIgnoreCase));
        /// <summary>Adds the vehicle to the register. Returns false if the registration number is taken.</summary>
        public bool Register(Vehicle vehicle)
        {
            if (IsRegistered(vehicle.RegistrationNumber))
                return false;

            _registered.Add(vehicle);
            return true;
        }
        public bool IsParked(string registrationNumber) => FindIndex(registrationNumber) != -1;
        public bool Remove(string registrationNumber)
        {
            int index = FindIndex(registrationNumber);
            if (index == -1)
                return false;

            _parked.RemoveAt(index);
            return true;
        }
        public ParkResult Park(Vehicle vehicle)
        {
            if (FindIndex(vehicle.RegistrationNumber) != -1)
                return ParkResult.AlreadyParked;

            if (vehicle.SizeInUnits > FreeUnits)
                return ParkResult.NotEnoughSpace;

            if (!IsRegistered(vehicle.RegistrationNumber))
                _registered.Add(vehicle);

            _parked.Add(vehicle);
            return ParkResult.Parked;
        }
        public List<Vehicle> GetParkedVehicles() => [.. _parked];
        public List<(string TypeName, int Count)> CountByType() =>
            _parked
                .GroupBy(vehicle => vehicle.TypeName)
                .Select(group => (TypeName: group.Key, Count: group.Count()))
                .ToList();
        public Vehicle? Find(string registrationNumber)
        {
            int index = FindIndex(registrationNumber);
            return index == -1 ? null : _parked[index];
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
        /// <summary>True if all the vehicles together fit in an empty garage.</summary>
        public bool CanHold(List<Vehicle> vehicles) => vehicles.Sum(vehicle => vehicle.SizeInUnits) <= TotalUnits;

        /// <summary>
        /// Makes the wanted vehicles parked and the other candidates not parked.
        /// Vehicles that leave are removed first, so their space can be used by the ones that arrive.
        /// </summary>
        public List<ParkingReport> SetParked(List<Vehicle> wanted)
        {
            List<ParkingReport> reports = [];

            foreach (Vehicle vehicle in _registered)
            {
                if (!wanted.Contains(vehicle) && Remove(vehicle.RegistrationNumber))
                    reports.Add(new ParkingReport(vehicle, ParkingChange.LeftGarage));
            }

            foreach (Vehicle vehicle in wanted)
            {
                if (IsParked(vehicle.RegistrationNumber))
                    continue;

                ParkingChange change = Park(vehicle) == ParkResult.Parked
                    ? ParkingChange.Parked
                    : ParkingChange.NotEnoughSpace;
                reports.Add(new ParkingReport(vehicle, change));
            }

            return reports;
        }
        /// <summary>Parks the vehicles in random order, skipping any that do not fit. Returns how many were parked.</summary>
        public int ParkInRandomOrder(List<Vehicle> vehicles)
        {
            Vehicle[] shuffled = [.. vehicles];
            Random.Shared.Shuffle(shuffled);

            int parked = 0;
            foreach (Vehicle vehicle in shuffled)
            {
                if (Park(vehicle) == ParkResult.Parked)
                    parked++;
            }
            return parked;
        }
        private int FindIndex(string registrationNumber)
        {
            for (int i = 0; i < _parked.Count; i++)
            {
                Vehicle? vehicle = _parked[i];
                if (vehicle.RegistrationNumber.Equals(registrationNumber, StringComparison.OrdinalIgnoreCase))
                    return i;
            }
            return -1;
        }
    }
}