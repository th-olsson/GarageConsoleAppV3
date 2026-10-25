using GarageConsoleApp.UI.Forms;
using GarageConsoleApp.UI.Views;
using GarageConsoleApp.Storage;

namespace GarageConsoleApp.UI.Menus
{
    internal static class GarageMenu
    {
        internal static void Run(Garage garage)
        {
            MenuRunner.Run(garage.Name, "Back to start",
                ("Manage parking", () =>
                {
                    ManageParkingForm.Run(garage);
                    GarageStorage.Save(garage);
                }
            ),
                ("Add vehicle", () =>
                {
                    CreateVehicleForm.Run(garage);
                    GarageStorage.Save(garage);
                }
            ),
                ("List Vehicles", () => ListVehiclesView.Run(garage)),
                ("Vehicle types", () => TypeCountView.Run(garage)),
                ("Search Vehicles", () => SearchMenu.Run(garage))
            );
        }
    }
}