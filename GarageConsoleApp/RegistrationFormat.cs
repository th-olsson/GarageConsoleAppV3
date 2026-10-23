using System.Text.RegularExpressions;

namespace GarageConsoleApp
{
    /// <summary>A set of rules for what a valid registration number looks like.</summary>
    internal sealed class RegistrationFormat
    {
        /// <summary>ABC123 or ABC12A. Used for all vehicles except aircraft.</summary>
        public static readonly RegistrationFormat Road =
            new("three letters followed by three digits or two digits and a letter, e.g. ABC123 or ABC12A",
                @"^[A-Z]{3}[0-9]{2}[0-9A-Z]$");

        /// <summary>SE- followed by three letters, e.g. SE-ABC.</summary>
        public static readonly RegistrationFormat Aircraft =
            new("SE- followed by three letters, e.g. SE-ABC", @"^SE-[A-Z]{3}$");

        private readonly Regex _pattern;

        /// <summary>Explains the format, to show the user when a registration number is rejected.</summary>
        public string Description { get; }

        private RegistrationFormat(string description, string pattern)
        {
            Description = description;
            _pattern = new Regex(pattern);
        }

        /// <summary>Removes spaces around the text and makes it upper case.</summary>
        public static string Normalize(string? text) => (text ?? "").Trim().ToUpperInvariant();

        public bool IsValid(string? text) => _pattern.IsMatch(Normalize(text));
    }
}