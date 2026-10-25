using static GarageConsoleApp.UI.ConsoleUI;

namespace GarageConsoleApp.UI.Forms
{
    internal static class ManageParkingForm
    {
        internal static void Run(Garage garage)
        {
            List<Vehicle> vehicles = garage.GetRegisteredVehicles();

            // Start from what is parked right now
            bool[] isChecked = new bool[vehicles.Count];
            for (int i = 0; i < vehicles.Count; i++)
                isChecked[i] = garage.IsParked(vehicles[i].RegistrationNumber);

            string[] tableLines = BuildTable(vehicles);

            int selected = 0;
            int first = 0; // index of the first vehicle shown, so a long list can scroll
            bool running = true;
            Console.CursorVisible = false;

            while (running)
            {
                List<Vehicle> wanted = GetChecked(vehicles, isChecked);
                int neededUnits = wanted.Sum(vehicle => vehicle.SizeInUnits);
                bool fits = garage.CanHold(wanted);

                // Show only as many vehicles as fit in the window, scrolled so the selected one is visible
                int visibleRows = Math.Max(3, Console.WindowHeight - 12);
                first = Math.Min(first, Math.Max(0, vehicles.Count - visibleRows));
                if (selected < first)
                    first = selected;
                if (selected >= first + visibleRows)
                    first = selected - visibleRows + 1;
                int last = Math.Min(vehicles.Count, first + visibleRows); // one past the last shown

                Title($"Manage parking - {garage.Name}");
                Text("Up/Down: move   Spacebar: check/uncheck   Enter: apply   Esc: cancel");
                Console.WriteLine();

                Text($"    {tableLines[0]}"); // 4 spaces = width of "[x] "
                for (int i = first; i < last; i++)
                {
                    string row = FormatRow(tableLines[i + 1], isChecked[i], garage.IsParked(vehicles[i].RegistrationNumber));

                    if (i == selected)
                        HighlightedText(row);
                    else
                        Text(row);
                }

                Console.WriteLine();
                if (vehicles.Count > visibleRows)
                    HintText($"Showing {first + 1}-{last} of {vehicles.Count}");

                string summary = $"Parking spaces needed: {ParkingUnits.Format(neededUnits)} / {garage.Capacity}";
                if (fits)
                {
                    Text(summary);
                }
                else
                {
                    ErrorText($"{summary} - too much, uncheck something");
                }

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        selected = selected == 0 ? vehicles.Count - 1 : selected - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selected = selected == vehicles.Count - 1 ? 0 : selected + 1;
                        break;
                    case ConsoleKey.Spacebar:
                        isChecked[selected] = !isChecked[selected];
                        break;
                    case ConsoleKey.Enter:
                        if (fits)
                        {
                            Console.WriteLine();
                            Apply(garage, vehicles, isChecked);
                            PressAnyKey();
                            running = false;
                        }
                        break;
                    case ConsoleKey.Escape:
                        running = false;
                        break;
                }
            }

            Console.CursorVisible = true;
        }

        private static string[] BuildTable(List<Vehicle> vehicles)
        {
            string[][] rows = new string[vehicles.Count][];
            for (int i = 0; i < vehicles.Count; i++)
            {
                Vehicle vehicle = vehicles[i];
                rows[i] =
                [
                    vehicle.RegistrationNumber,
                    vehicle.Model,
                    vehicle.TypeName,
                    ParkingUnits.Format(vehicle.SizeInUnits)
                ];
            }

            // Column 3 (Space) is right-aligned
            return Table.Format(["Reg", "Model", "Type", "Space"], rows, 3);
        }
        private static string FormatRow(string tableLine, bool isChecked, bool isParked)
        {
            string box = isChecked ? "[x]" : "[ ]";
            string status = (isChecked, isParked) switch
            {
                (true, false) => "park",
                (false, true) => "leave",
                (true, true) => "parked",
                _ => ""
            };

            return $"{box} {tableLine}  {status}";
        }
        private static List<Vehicle> GetChecked(List<Vehicle> vehicles, bool[] isChecked)
        {
            List<Vehicle> result = [];
            for (int i = 0; i < vehicles.Count; i++)
            {
                if (isChecked[i])
                    result.Add(vehicles[i]);
            }
            return result;
        }
        private static void Apply(Garage garage, List<Vehicle> vehicles, bool[] isChecked)
        {
            Console.Clear();

            List<ParkingReport> reports = garage.SetParked(GetChecked(vehicles, isChecked));

            foreach (ParkingReport report in reports)
            {
                string message = report.Change switch
                {
                    ParkingChange.LeftGarage => "left the garage",
                    ParkingChange.Parked => "parked",
                    _ => "could not be parked, not enough space"
                };

                Text($"{report.Vehicle.RegistrationNumber} {report.Vehicle.Model}: {message}");
            }

            if (reports.Count == 0)
                Text("No changes.");
        }
    }
}