using GarageConsoleApp.Models;
using static GarageConsoleApp.UI.ConsoleUI;

namespace GarageConsoleApp.UI.Forms
{
    internal static class CreateVehicleForm
    {
        internal static void Run(Garage garage)
        {
            Title("Add vehicle");

            string type = Vehicle.TypeNames[Choose("What type of vehicle?", [.. Vehicle.TypeNames])];

            Console.WriteLine();
            string registrationNumber = ReadRegistrationNumber(type, garage);
            string model = ReadString("Model, e.g. Volvo V70: ");
            string color = ReadString("Color, e.g. Black: ");
            Console.WriteLine();

            Vehicle vehicle = type switch
            {
                nameof(Car) => CreateCar(registrationNumber, model, color),
                nameof(Motorcycle) => CreateMotorcycle(registrationNumber, model, color),
                nameof(Bus) => CreateBus(registrationNumber, model, color),
                nameof(Boat) => CreateBoat(registrationNumber, model, color),
                nameof(Airplane) => CreateAirplane(registrationNumber, model, color),
                _ => throw new InvalidOperationException($"The form does not support {type} yet.")
            };

            garage.Register(vehicle);

            Console.WriteLine();
            Text("Vehicle added:");
            VehicleTable.Print([vehicle]);
            Console.WriteLine();

            if (garage.Park(vehicle) == ParkResult.Parked)
                Text("It has been parked in the garage.");
            else
                ErrorText("There is not enough room in the garage right now. You can park it later under Manage parking.");

            PressAnyKey();
        }

        // Asks until the number follows the format for the type and is not already registered
        private static string ReadRegistrationNumber(string type, Garage garage)
        {
            RegistrationFormat format = type == nameof(Airplane)
                ? RegistrationFormat.Aircraft
                : RegistrationFormat.Road;

            while (true)
            {
                string input = ReadString("Registration number: ");

                if (!format.IsValid(input))
                {
                    ErrorText($"A registration number is {format.Description}.");
                    continue;
                }

                string registrationNumber = RegistrationFormat.Normalize(input);

                if (garage.IsRegistered(registrationNumber))
                {
                    ErrorText($"{registrationNumber} is already registered.");
                    continue;
                }

                return registrationNumber;
            }
        }

        private static Car CreateCar(string registrationNumber, string model, string color)
        {
            FuelType fuelType = ChooseEnum<FuelType>("Fuel type?");
            return new Car(registrationNumber, model, color, fuelType);
        }

        private static Motorcycle CreateMotorcycle(string registrationNumber, string model, string color)
        {
            TransmissionType transmission = ChooseEnum<TransmissionType>("Transmission?");
            return new Motorcycle(registrationNumber, model, color, transmission);
        }

        private static Bus CreateBus(string registrationNumber, string model, string color)
        {
            int seats = ReadInt("Number of seats: ", 1, 200);
            int wheels = ReadInt("Number of wheels: ", 4, 12);
            return new Bus(registrationNumber, model, color, seats, wheels);
        }

        private static Boat CreateBoat(string registrationNumber, string model, string color)
        {
            int length = ReadInt("Length in whole meters: ", 1, 100);
            return new Boat(registrationNumber, model, color, length);
        }

        private static Airplane CreateAirplane(string registrationNumber, string model, string color)
        {
            int wheels = ReadInt("Number of wheels: ", 1, 20);
            int engines = ReadInt("Number of engines: ", 1, 8);
            return new Airplane(registrationNumber, model, color, wheels, engines);
        }

        // Shows every value of an enum as a choice, e.g. Gasoline, Diesel, Electric, Hybrid
        private static T ChooseEnum<T>(string prompt) where T : struct, Enum
        {
            T[] values = Enum.GetValues<T>();
            string[] names = [.. values.Select(value => value.ToString())];

            return values[Choose(prompt, names)];
        }
    }
}