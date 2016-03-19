using System.Text.RegularExpressions;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A simple helper class for formatting type definitions.
    /// </summary>
    public static class TypeFormatHelper
    {
        /// <summary>
        /// Formats the specified input type string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The formatted output string.</returns>
        public static string Format(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var result = Regex.Replace(input, @"[^ \t,<>\[\]]*\.", string.Empty);

            return result;
        }
    }
}