using GarageConsoleApp.Models;
using GarageConsoleApp.UI.Forms;

namespace GarageConsoleApp.UI.Menus
{
    internal static class SearchMenu
    {
        internal static void Run(Garage<Vehicle> garage)
        {
            MenuRunner.Run("Search vehicles", "Back",
                ("By registration number", () => FindByRegistrationForm.Run(garage)),
                ("By properties", () => SearchByPropertiesForm.Run(garage))
            );
        }
    }
}