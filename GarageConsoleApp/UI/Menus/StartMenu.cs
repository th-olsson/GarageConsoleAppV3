using GarageConsoleApp.UI.Forms;

namespace GarageConsoleApp.UI.Menus
{
    internal static class StartMenu
    {
        internal static void Run(IHandler handler)
        {
            // Continue where the user left off, if there is a saved garage
            handler.LoadGarage();

            MenuRunner.Run("Garage Manager", "Exit", () =>
            {
                var options = new List<(string Label, Action Action)>
                {
                    // Creates the garage the first time, then opens it
                    (GarageLabel(handler), () =>
                    {
                        if (!handler.HasGarage)
                            CreateGarageForm.Run(handler);

                        GarageMenu.Run(handler);
                    })
                };

                if (handler.HasGarage)
                    options.Add(("Delete garage", () => DeleteGarageForm.Run(handler)));

                return [.. options];
            });
        }

        // Text for the garage option: "Create garage" if none exists, otherwise its name and occupancy
        private static string GarageLabel(IHandler handler) =>
            !handler.HasGarage
                ? "Create garage"
                : $"{handler.Name} ({ParkingUnits.Format(handler.UsedUnits)} of {handler.Capacity} parking spaces used)";
    }
}