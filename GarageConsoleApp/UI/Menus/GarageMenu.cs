using GarageConsoleApp.UI.Forms;
using GarageConsoleApp.UI.Views;

namespace GarageConsoleApp.UI.Menus
{
    internal static class GarageMenu
    {
        internal static void Run(Garage garage, List<Vehicle> vehicles)
        {
            MenuRunner.Run(garage.Name, "Back to start",
                ("Manage parking", () => ManageParkingForm.Run(garage, vehicles)),
                ("Add vehicle", () => CreateVehicleForm.Run(garage, vehicles)),
                ("List Vehicles", () => ListVehiclesView.Run(garage)),
                ("Vehicle types", () => TypeCountView.Run(garage)),
                ("Search Vehicles", () => SearchMenu.Run(garage))
            );
        }
    }
}