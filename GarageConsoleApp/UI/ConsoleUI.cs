namespace GarageConsoleApp.UI
{
    internal static class ConsoleUI
    {
        internal static void Title(string text)
        {
            string line = new string('─', text.Length + 2);

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"   ┌{line}┐");
            Console.WriteLine($"   │ {text} │");
            Console.WriteLine($"   └{line}┘");
            Console.ResetColor();
            Console.WriteLine();
        }
        internal static void Text(string text) => Console.WriteLine($"    {text}");
        internal static void TextInline(string text) => Console.Write($"    {text}");
        internal static void HighlightedText(string text, bool withArrow = true)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            if (withArrow)
                Console.WriteLine($" -> {text}");
            else
                Text(text);
            Console.ResetColor();
        }
        internal static void HintText(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Text(text);
            Console.ResetColor();
        }
        internal static void ErrorText(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Text(text);
            Console.ResetColor();
        }

        private const string HelpText = "Use the arrow keys to choose, then press Enter.";
        /// <summary>
        /// Lets the user pick one option with the arrow keys. Returns the index of the chosen option.
        /// If backLabel is given it is added as the last option, and choosing it returns -1.
        /// </summary>
        internal static int Choose(string? prompt, string[] options, int initialIndex = 0, bool separateLast = false, string? backLabel = null)
        {
            // The back option goes last, with a gap before it
            string[] shown = options;
            if (backLabel != null)
                shown = [.. options, backLabel];

            if (prompt != null)
            {
                Text(prompt);
                Console.WriteLine();
            }
            HintText(HelpText);
            Console.WriteLine();
            Console.CursorVisible = false;

            int selected = initialIndex;
            int rows = shown.Length + (backLabel != null ? 1 : 0);
            int top = -1; // first row of the option list, known after the first draw

            while (true)
            {
                if (top != -1)
                    Console.SetCursorPosition(0, top);

                for (int i = 0; i < shown.Length; i++)
                {
                    if (backLabel != null && i == shown.Length - 1)
                        Console.WriteLine(); // spacer before the back option

                    Option(shown[i], i == selected);
                }

                if (top == -1)
                    top = Console.CursorTop - rows;

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.DownArrow:
                        selected = selected == shown.Length - 1 ? 0 : selected + 1;
                        break;
                    case ConsoleKey.UpArrow:
                        selected = selected == 0 ? shown.Length - 1 : selected - 1;
                        break;
                    case ConsoleKey.Enter:
                        Console.CursorVisible = true;
                        return backLabel != null && selected == shown.Length - 1 ? -1 : selected;
                }
            }
        }
        internal static void Option(string label, bool highlighted)
        {
            if (highlighted)
                HighlightedText(label);
            else
                Text(label);
        }
        internal static void PressAnyKey()
        {
            Console.WriteLine();
            Text("Press any key to continue...");
            Console.ReadKey();
        }
        internal static string ReadString(string prompt)
        {
            while (true)
            {
                Console.CursorVisible = true;
                TextInline(prompt);
                string? input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                    return input.Trim();

                ErrorText("Input cannot be empty.");
            }
        }
        internal static string? ReadOptionalString(string prompt)
        {
            Console.CursorVisible = true;
            TextInline(prompt);
            string? input = Console.ReadLine();

            return string.IsNullOrWhiteSpace(input) ? null : input.Trim();
        }

        internal static int ReadInt(string prompt, int min = int.MinValue, int max = int.MaxValue)
        {
            while (true)
            {
                Console.CursorVisible = true;
                TextInline(prompt);

                if (!int.TryParse(Console.ReadLine(), out int value))
                {
                    ErrorText("That is not a whole number. Please try again.");
                    continue;
                }

                if (value < min || value > max)
                {
                    ErrorText($"Please enter {DescribeRange(min, max)}.");
                    continue;
                }

                return value;
            }
        }
        internal static int? ReadOptionalInt(string prompt, int min = int.MinValue, int max = int.MaxValue)
        {
            while (true)
            {
                Console.CursorVisible = true;
                TextInline(prompt);
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    return null;

                if (!int.TryParse(input, out int value))
                {
                    ErrorText("That is not a whole number. Try again.");
                    continue;
                }

                if (value < min || value > max)
                {
                    ErrorText($"Please enter {DescribeRange(min, max)}.");
                    continue;
                }

                return value;
            }
        }
        // Describes only the limits that exist, so "no maximum" never shows up as a huge number
        private static string DescribeRange(int min, int max)
        {
            if (min != int.MinValue && max != int.MaxValue)
                return $"a number from {min} to {max}";
            if (min != int.MinValue)
                return $"a number that is {min} or more";
            if (max != int.MaxValue)
                return $"a number that is {max} or less";

            return "a whole number";
        }
    }
}