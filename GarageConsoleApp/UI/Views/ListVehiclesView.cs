using GarageConsoleApp.Models;
using static GarageConsoleApp.UI.ConsoleUI;

namespace GarageConsoleApp.UI.Views
{
    internal static class ListVehiclesView
    {
        internal static void Run(Garage<Vehicle> garage)
        {
            Title($"Parked vehicles - {garage.Name}");

            List<Vehicle> parked = garage.GetParkedVehicles();

            if (parked.Count == 0)
                Text("The garage is empty.");
            else
                VehicleTable.Print(parked);

            Console.WriteLine();
            Text($"Parking spaces used: {ParkingUnits.Format(garage.UsedUnits)} / {garage.Capacity}");
            PressAnyKey();
        }
    }
}