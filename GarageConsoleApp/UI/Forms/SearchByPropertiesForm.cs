using static GarageConsoleApp.UI.ConsoleUI;
using GarageConsoleApp.Models;

namespace GarageConsoleApp.UI.Forms
{
    internal static class SearchByPropertiesForm
    {
        internal static void Run(IHandler handler)
        {
            Title("Search vehicles");
            Text("Tell us what you are looking for. Only vehicles that match everything you enter are shown.");
            Console.WriteLine();

            // The first option means "no type filter", the rest are the vehicle types
            string[] typeOptions = ["Any type", .. Vehicle.TypeNames];
            int typeChoice = Choose("What type of vehicle?", typeOptions, backLabel: "Cancel");
            if (typeChoice == -1)
                return;

            Console.WriteLine();
            string? color = ReadOptionalString("Color, e.g. Black (leave blank to skip): ");
            int? wheels = ReadOptionalInt("Number of wheels (leave blank to skip): ", 0, 100);
            string? model = ReadOptionalString("Model or part of the name, e.g. Volvo (leave blank to skip): ");

            var filter = new VehicleFilter
            {
                TypeName = typeChoice == 0 ? null : Vehicle.TypeNames[typeChoice - 1],
                Color = color,
                NumberOfWheels = wheels,
                Model = model
            };

            Console.WriteLine();

            if (filter.IsEmpty)
            {
                ErrorText("Nothing to search for. Pick a type or fill in at least one field next time.");
            }
            else
            {
                List<Vehicle> matches = handler.Search(filter);

                if (matches.Count == 0)
                {
                    Text("No parked vehicles match your search.");
                }
                else
                {
                    VehicleTable.Print(matches);
                    Console.WriteLine();
                    Text(matches.Count == 1 ? "Found 1 vehicle." : $"Found {matches.Count} vehicles.");
                }
            }

            PressAnyKey();
        }
    }
}