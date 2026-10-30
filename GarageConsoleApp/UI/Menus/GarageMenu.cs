using GarageConsoleApp.UI.Forms;
using GarageConsoleApp.UI.Views;

namespace GarageConsoleApp.UI.Menus
{
    internal static class GarageMenu
    {
        internal static void Run(IHandler handler)
        {
            MenuRunner.Run(handler.Name, "Back to start",
                ("Manage parking", () => ManageParkingForm.Run(handler)),
                ("Add vehicle", () => CreateVehicleForm.Run(handler)),
                ("List Vehicles", () => ListVehiclesView.Run(handler)),
                ("Vehicle types", () => TypeCountView.Run(handler)),
                ("Search Vehicles", () => SearchMenu.Run(handler))
            );
        }
    }
}