using GarageConsoleApp.UI.Menus;

namespace GarageConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IHandler handler = new GarageHandler();
            StartMenu.Run(handler);
        }
    }
}