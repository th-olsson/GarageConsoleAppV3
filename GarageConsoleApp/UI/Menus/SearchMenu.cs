using GarageConsoleApp.Models;
using GarageConsoleApp.UI.Forms;

namespace GarageConsoleApp.UI.Menus
{
    internal static class SearchMenu
    {
        internal static void Run(IHandler handler)
        {
            MenuRunner.Run("Search vehicles", "Back",
                ("By registration number", () => FindByRegistrationForm.Run(handler)),
                ("By properties", () => SearchByPropertiesForm.Run(handler))
            );
        }
    }
}