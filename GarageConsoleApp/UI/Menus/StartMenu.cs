using GarageConsoleApp.Storage;
using GarageConsoleApp.UI.Forms;

namespace GarageConsoleApp.UI.Menus
{
    internal static class StartMenu
    {
        internal static void Run()
        {
            // Kept in memory when going back to this menu. Null until the garage is created.
            Garage? garage = null;
            List<Vehicle> vehicles = [];


            // Continue where the user left off, if there is a saved garage
            var saved = GarageStorage.Load();
            if (saved != null)
                (garage, vehicles) = saved.Value;

            MenuRunner.Run("Garage Manager", "Exit", () =>
            [
                // Creates the garage the first time, then opens it
                (GarageLabel(garage), () =>
                {
                    if (garage == null)
                    {
                        vehicles = VehicleRegistry.CreateSeed();
                        garage = CreateGarageForm.Run(vehicles);
                    }

                    GarageMenu.Run(garage, vehicles);
                })
            ]);
        }

        // Text for the garage option: "Create garage" if none exists, otherwise its name and occupancy
        private static string GarageLabel(Garage? garage) =>
            garage == null
                ? "Create garage"
                : $"{garage.Name} ({ParkingUnits.Format(garage.UsedUnits)} of {garage.Capacity} parking spaces used)";
    }
}