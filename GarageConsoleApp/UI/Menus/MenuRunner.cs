using static GarageConsoleApp.UI.ConsoleUI;

namespace GarageConsoleApp.UI.Menus
{
    internal static class MenuRunner
    {
        // Menu with fixed options
        internal static void Run(string title, string exitLabel,
            params (string Label, Action Action)[] options)
            => Run(title, exitLabel, () => options);

        // Menu where the options are fetched every lap
        internal static void Run(string title, string exitLabel,
            Func<(string Label, Action Action)[]> getOptions)
        {
            int selected = 0;

            while (true)
            {
                var options = getOptions();

                string[] labels = new string[options.Length];
                for (int i = 0; i < options.Length; i++)
                {
                    labels[i] = options[i].Label;
                }

                // Keep the marker in range if the number of options has shrunk (the last position is the back option)
                selected = Math.Min(selected, options.Length);

                Title(title);
                selected = Choose(null, labels, selected, backLabel: exitLabel);

                if (selected == -1)
                    return;

                Console.WriteLine();
                options[selected].Action();
            }
        }
    }
}
