using static GarageConsoleApp.UI.ConsoleUI;

namespace GarageConsoleApp.UI.Forms
{
    internal static class FindByRegistrationForm
    {
        internal static void Run(Garage garage)
        {
            Title("Find vehicle by registration number");
            string registrationNumber = ReadString("Registration number: ");

            Console.WriteLine();
            Vehicle? vehicle = garage.Find(registrationNumber);

            if (vehicle == null)
                ErrorText($"No parked vehicle with registration number {registrationNumber.ToUpper()}.");
            else
                VehicleTable.Print([vehicle]);

            PressAnyKey();
        }
    }
}