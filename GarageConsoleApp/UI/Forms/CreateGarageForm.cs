using static GarageConsoleApp.UI.ConsoleUI;

namespace GarageConsoleApp.UI.Forms
{
    internal static class CreateGarageForm
    {
        private const int MinCapacity = 1;
        private const int MaxCapacity = 100;

        internal static void Run(IHandler handler)
        {
            Title("Create garage");
            string name = ReadString("What do you want to call your garage: ");
            int capacity = ReadInt($"How many parking spaces do you want? ({MinCapacity}-{MaxCapacity}): ", MinCapacity, MaxCapacity);
            int selected = Choose("Do you want an empty or populated garage?", ["Empty garage", "Populated garage"]);

            int parked = handler.CreateGarage(name, capacity, populate: selected == 1);

            Console.WriteLine();
            Text($"Garage \"{name}\" created with {capacity} parking spaces.");

            if (selected == 1)
                Text($"{parked} of {handler.GetRegisteredVehicles().Count} vehicles from the register were parked.");

            PressAnyKey();
        }
    }
}