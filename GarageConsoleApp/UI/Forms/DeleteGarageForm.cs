using GarageConsoleApp.Models;
using GarageConsoleApp.Storage;
using static GarageConsoleApp.UI.ConsoleUI;

namespace GarageConsoleApp.UI.Forms
{
    internal static class DeleteGarageForm
    {
        /// <summary>Asks for confirmation, then deletes the saved garage. Returns true if it was deleted.</summary>
        internal static bool Run(Garage<Vehicle> garage)
        {
            Title("Delete garage");
            Text($"This deletes \"{garage.Name}\" and all {garage.GetRegisteredVehicles().Count} vehicles in its registry.");
            Text("It cannot be undone.");
            Console.WriteLine();

            // "No" is first, so it is the choice you get if you just press Enter
            int choice = Choose("Are you sure?", ["No, keep it", "Yes, delete it"]);
            if (choice != 1)
                return false;

            GarageStorage.Delete();

            Console.WriteLine();
            Text("The garage has been deleted.");
            PressAnyKey();
            return true;
        }
    }
}