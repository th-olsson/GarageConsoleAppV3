using System.Text.Json;
using System.Text.Json.Serialization;
using GarageConsoleApp.Models;

namespace GarageConsoleApp.Storage
{
    /// <summary>The garage and its vehicles, as they are written to the file.</summary>
    internal class GarageData
    {
        public string Name { get; set; } = "";
        public int Capacity { get; set; }
        public List<VehicleData> Vehicles { get; set; } = [];
    }

    /// <summary>One vehicle as it is written to the file. Only the fields for its type are filled in.</summary>
    internal class VehicleData
    {
        public string Type { get; set; } = "";
        public string RegistrationNumber { get; set; } = "";
        public string Model { get; set; } = "";
        public string Color { get; set; } = "";
        public int NumberOfWheels { get; set; }
        public int SizeInUnits { get; set; }
        public bool Parked { get; set; }

        public FuelType? FuelType { get; set; }                  // Car
        public TransmissionType? TransmissionType { get; set; }  // Motorcycle
        public int? NumberOfSeats { get; set; }                  // Bus
        public double? Length { get; set; }                      // Boat
        public int? NumberOfEngines { get; set; }                // Airplane
    }

    internal static class GarageStorage
    {
        private static readonly string FilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GarageApp", "garage.json");

        private static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };

        internal static void Save(Garage garage)
        {
            var data = new GarageData { Name = garage.Name, Capacity = garage.Capacity };

            foreach (Vehicle vehicle in garage.GetRegisteredVehicles())
                data.Vehicles.Add(ToData(vehicle, garage.IsParked(vehicle.RegistrationNumber)));

            Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
            File.WriteAllText(FilePath, JsonSerializer.Serialize(data, Options));
        }

        /// <summary>Returns the saved garage and vehicles, or null if there is no usable file.</summary>
        internal static Garage? Load()
        {
            if (!File.Exists(FilePath))
                return null;

            try
            {
                GarageData? data = JsonSerializer.Deserialize<GarageData>(File.ReadAllText(FilePath), Options);
                if (data == null)
                    return null;

                var garage = new Garage(data.Name, data.Capacity);

                foreach (VehicleData item in data.Vehicles)
                {
                    Vehicle vehicle = ToVehicle(item);
                    garage.Register(vehicle);

                    if (item.Parked)
                        garage.Park(vehicle);
                }

                return garage;
            }
            catch (Exception ex) when (ex is JsonException or InvalidDataException or ArgumentException)
            {
                // A broken file is treated as if there was no file
                return null;
            }
        }

        /// <summary>Deletes the saved garage, if there is one.</summary>
        internal static void Delete()
        {
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }

        private static VehicleData ToData(Vehicle vehicle, bool parked)
        {
            var data = new VehicleData
            {
                Type = vehicle.TypeName,
                RegistrationNumber = vehicle.RegistrationNumber,
                Model = vehicle.Model,
                Color = vehicle.Color,
                NumberOfWheels = vehicle.NumberOfWheels,
                SizeInUnits = vehicle.SizeInUnits,
                Parked = parked
            };

            // Each type has its own extra property
            switch (vehicle)
            {
                case Car car: data.FuelType = car.FuelType; break;
                case Motorcycle motorcycle: data.TransmissionType = motorcycle.TransmissionType; break;
                case Bus bus: data.NumberOfSeats = bus.NumberOfSeats; break;
                case Boat boat: data.Length = boat.Length; break;
                case Airplane airplane: data.NumberOfEngines = airplane.NumberOfEngines; break;
            }

            return data;
        }

        private static Vehicle ToVehicle(VehicleData d) => d.Type switch
        {
            nameof(Car) => new Car(d.RegistrationNumber, d.Model, d.Color,
                d.FuelType ?? throw Missing(d, "FuelType"), d.NumberOfWheels, d.SizeInUnits),
            nameof(Motorcycle) => new Motorcycle(d.RegistrationNumber, d.Model, d.Color,
                d.TransmissionType ?? throw Missing(d, "TransmissionType"), d.NumberOfWheels, d.SizeInUnits),
            nameof(Bus) => new Bus(d.RegistrationNumber, d.Model, d.Color,
                d.NumberOfSeats ?? throw Missing(d, "NumberOfSeats"), d.NumberOfWheels, d.SizeInUnits),
            nameof(Boat) => new Boat(d.RegistrationNumber, d.Model, d.Color,
                d.Length ?? throw Missing(d, "Length"), d.SizeInUnits),
            nameof(Airplane) => new Airplane(d.RegistrationNumber, d.Model, d.Color, d.NumberOfWheels,
                d.NumberOfEngines ?? throw Missing(d, "NumberOfEngines"), d.SizeInUnits),
            _ => throw new InvalidDataException($"Unknown vehicle type '{d.Type}'.")
        };

        private static InvalidDataException Missing(VehicleData d, string field) =>
            new($"{d.Type} {d.RegistrationNumber} is missing {field}.");
    }
}