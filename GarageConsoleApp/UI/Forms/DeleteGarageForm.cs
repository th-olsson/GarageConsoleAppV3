using static GarageConsoleApp.UI.ConsoleUI;

namespace GarageConsoleApp.UI.Forms
{
    internal static class DeleteGarageForm
    {
        /// <summary>Asks for confirmation, then deletes the garage.</summary>
        internal static void Run(IHandler handler)
        {
            Title("Delete garage");
            Text($"This deletes \"{handler.Name}\" and all {handler.GetRegisteredVehicles().Count} vehicles in its registry.");
            Text("It cannot be undone.");
            Console.WriteLine();

            // "No" is first, so it is the choice you get if you just press Enter
            int choice = Choose("Are you sure?", ["No, keep it", "Yes, delete it"]);
            if (choice != 1)
                return;

            handler.DeleteGarage();

            Console.WriteLine();
            Text("The garage has been deleted.");
            PressAnyKey();
        }
    }
}