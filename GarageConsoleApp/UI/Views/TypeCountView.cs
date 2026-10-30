using GarageConsoleApp.Models;
using static GarageConsoleApp.UI.ConsoleUI;

namespace GarageConsoleApp.UI.Views
{
    internal static class TypeCountView
    {
        internal static void Run(Garage<Vehicle> garage)
        {
            Title($"Vehicle types - {garage.Name}");

            List<(string TypeName, int Count)> counts = garage.CountByType();

            string[][] rows = new string[counts.Count][];
            for (int i = 0; i < counts.Count; i++)
                rows[i] = [counts[i].TypeName, counts[i].Count.ToString()];

            string[] lines = Table.Format(["Type", "Count"], rows, 1);

            Text(lines[0]);
            Text(new string('-', lines[0].Length));
            for (int i = 1; i < lines.Length; i++)
                Text(lines[i]);

            PressAnyKey();
        }
    }
}