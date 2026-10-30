using static GarageConsoleApp.UI.ConsoleUI;
using GarageConsoleApp.Models;

namespace GarageConsoleApp.UI.Forms
{
    internal static class CreateGarageForm
    {
        private const int MinCapacity = 1;
        private const int MaxCapacity = 100;

        internal static Garage Run()
        {
            Title("Create garage");
            string name = ReadString("What do you want to call your garage: ");
            int capacity = ReadInt($"How many parking spaces do you want? ({MinCapacity}-{MaxCapacity}): ", MinCapacity, MaxCapacity);
            int selected = Choose("Do you want an empty or populated garage?", ["Empty garage", "Populated garage"]);

            var garage = new Garage(name, capacity);

            // Every garage starts with the example vehicles in its register
            foreach (Vehicle vehicle in VehicleRegistry.CreateSeed())
                garage.Register(vehicle);

            Console.WriteLine();
            Text($"Garage \"{name}\" created with {capacity} parking spaces.");

            if (selected == 1)
            {
                int parked = garage.ParkInRandomOrder(garage.GetRegisteredVehicles());
                Text($"{parked} of {garage.GetRegisteredVehicles().Count} vehicles from the register were parked.");
            }

            PressAnyKey();
            return garage;
        }
    }
}