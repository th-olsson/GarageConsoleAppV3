using GarageConsoleApp.Models;
using static GarageConsoleApp.UI.ConsoleUI;

namespace GarageConsoleApp.UI.Views
{
    internal static class ListVehiclesView
    {
        internal static void Run(IHandler handler)
        {
            Title($"Parked vehicles - {handler.Name}");

            List<Vehicle> parked = handler.GetParkedVehicles();

            if (parked.Count == 0)
                Text("The garage is empty.");
            else
                VehicleTable.Print(parked);

            Console.WriteLine();
            Text($"Parking spaces used: {ParkingUnits.Format(handler.UsedUnits)} / {handler.Capacity}");
            PressAnyKey();
        }
    }
}