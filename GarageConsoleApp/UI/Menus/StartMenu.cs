using GarageConsoleApp.UI.Forms;

namespace GarageConsoleApp.UI.Menus
{
    internal class StartMenu : IUI
    {
        private readonly IHandler _handler;

        public StartMenu(IHandler handler)
        {
            _handler = handler;
        }

        public void Run()
        {
            // Continue where the user left off, if there is a saved garage
            _handler.LoadGarage();

            MenuRunner.Run("Garage Manager", "Exit", () =>
            {
                var options = new List<(string Label, Action Action)>
                {
                    // Creates the garage the first time, then opens it
                    (GarageLabel(), () =>
                    {
                        if (!_handler.HasGarage)
                            CreateGarageForm.Run(_handler);

                        GarageMenu.Run(_handler);
                    })
                };

                if (_handler.HasGarage)
                    options.Add(("Delete garage", () => DeleteGarageForm.Run(_handler)));

                return [.. options];
            });
        }

        // Text for the garage option: "Create garage" if none exists, otherwise its name and occupancy
        private string GarageLabel() =>
            !_handler.HasGarage
                ? "Create garage"
                : $"{_handler.Name} ({ParkingUnits.Format(_handler.UsedUnits)} of {_handler.Capacity} parking spaces used)";
    }
}