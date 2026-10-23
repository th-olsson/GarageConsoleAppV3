using GarageConsoleApp.Models;
using static GarageConsoleApp.UI.ConsoleUI;

namespace GarageConsoleApp.UI
{
    internal static class VehicleTable
    {
        internal static void Print(List<Vehicle> vehicles)
        {
            string[][] rows = new string[vehicles.Count][];
            for (int i = 0; i < vehicles.Count; i++)
            {
                Vehicle v = vehicles[i];
                rows[i] =
                [
                    v.TypeName,
                    v.RegistrationNumber,
                    v.Model,
                    v.Color,
                    v.NumberOfWheels.ToString(),
                    ParkingUnits.Format(v.SizeInUnits),
                    v.Describe()
                ];
            }

            // Columns 4 (Wheels) and 5 (Space) are right-aligned
            string[] lines = Table.Format(["Type", "Reg", "Model", "Color", "Wheels", "Space", "Details"], rows, 4, 5);

            Text(lines[0]);
            Text(new string('-', lines[0].Length));
            for (int i = 1; i < lines.Length; i++)
                Text(lines[i]);
        }
    }
}