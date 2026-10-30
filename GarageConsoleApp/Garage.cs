using System.Collections;
using GarageConsoleApp.Models;

namespace GarageConsoleApp
{
    internal class Garage<T> : IEnumerable<T> where T : Vehicle
    {
        private readonly List<T> _parked = [];
        private readonly List<T> _registered = [];
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
        }

        /// <summary>All vehicles the garage knows about, parked or not.</summary>
        public List<T> GetRegisteredVehicles() => [.. _registered];
        public bool IsRegistered(string registrationNumber) =>
            _registered.Any(vehicle => HasRegistrationNumber(vehicle, registrationNumber));
        /// <summary>Adds the vehicle to the register. Returns false if the registration number is taken.</summary>
        public bool Register(T vehicle)
        {
            if (IsRegistered(vehicle.RegistrationNumber))
                return false;

            _registered.Add(vehicle);
            return true;
        }
        public bool IsParked(string registrationNumber) =>
            _parked.Any(vehicle => HasRegistrationNumber(vehicle, registrationNumber));
        public bool Remove(string registrationNumber) =>
            _parked.RemoveAll(vehicle => HasRegistrationNumber(vehicle, registrationNumber)) > 0;
        public ParkResult Park(T vehicle)
        {
            if (IsParked(vehicle.RegistrationNumber))
                return ParkResult.AlreadyParked;

            if (vehicle.SizeInUnits > FreeUnits)
                return ParkResult.NotEnoughSpace;

            if (!IsRegistered(vehicle.RegistrationNumber))
                _registered.Add(vehicle);

            _parked.Add(vehicle);
            return ParkResult.Parked;
        }
        public List<T> GetParkedVehicles() => [.. _parked];
        public List<(string TypeName, int Count)> CountByType() =>
            _parked
                .GroupBy(vehicle => vehicle.TypeName)
                .Select(group => (TypeName: group.Key, Count: group.Count()))
                .ToList();
        public T? Find(string registrationNumber) =>
            _parked.FirstOrDefault(vehicle => HasRegistrationNumber(vehicle, registrationNumber));
        public List<T> Search(VehicleFilter filter) => _parked.Where(vehicle => filter.Matches(vehicle)).ToList();
        /// <summary>True if all the vehicles together fit in an empty garage.</summary>
        public bool CanHold(List<T> vehicles) => vehicles.Sum(vehicle => vehicle.SizeInUnits) <= TotalUnits;

        /// <summary>
        /// Makes the wanted vehicles parked and the other candidates not parked.
        /// Vehicles that leave are removed first, so their space can be used by the ones that arrive.
        /// </summary>
        public List<ParkingReport> SetParked(List<T> wanted)
        {
            List<ParkingReport> reports = [];

            foreach (T vehicle in _registered.Where(vehicle => !wanted.Contains(vehicle)))
            {
                if (Remove(vehicle.RegistrationNumber))
                    reports.Add(new ParkingReport(vehicle, ParkingChange.LeftGarage));
            }

            foreach (T vehicle in wanted.Where(vehicle => !IsParked(vehicle.RegistrationNumber)))
            {
                ParkingChange change = Park(vehicle) == ParkResult.Parked
                    ? ParkingChange.Parked
                    : ParkingChange.NotEnoughSpace;
                reports.Add(new ParkingReport(vehicle, change));
            }

            return reports;
        }
        /// <summary>Parks the vehicles in random order, skipping any that do not fit. Returns how many were parked.</summary>
        public int ParkInRandomOrder(List<T> vehicles)
        {
            T[] shuffled = [.. vehicles];
            Random.Shared.Shuffle(shuffled);

            int parked = 0;
            foreach (T vehicle in shuffled)
            {
                if (Park(vehicle) == ParkResult.Parked)
                    parked++;
            }
            return parked;
        }

        /// <summary>Allows looping over the parked vehicles with foreach.</summary>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (T vehicle in _parked)
                yield return vehicle;
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private static bool HasRegistrationNumber(T vehicle, string registrationNumber) =>
            vehicle.RegistrationNumber.Equals(registrationNumber, StringComparison.OrdinalIgnoreCase);
    }
}