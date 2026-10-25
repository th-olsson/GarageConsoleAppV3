using GarageConsoleApp.Storage;
using GarageConsoleApp.UI.Forms;

namespace GarageConsoleApp.UI.Menus
{
    internal static class StartMenu
    {
        internal static void Run()
        {
            // Continue where the user left off, if there is a saved garage. Otherwise null until it is created.
            Garage? garage = GarageStorage.Load();

            MenuRunner.Run("Garage Manager", "Exit", () =>
            [
                // Creates the garage the first time, then opens it
                (GarageLabel(garage), () =>
                {
                    if (garage == null)
                    {
                        garage = CreateGarageForm.Run();
                        GarageStorage.Save(garage);
                    }

                    GarageMenu.Run(garage);
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