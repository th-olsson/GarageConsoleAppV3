using GarageConsoleApp.UI.Menus;

namespace GarageConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IHandler handler = new GarageHandler();
            IUI ui = new StartMenu(handler);
            ui.Run();
        }
    }
}