#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

using System.Text.RegularExpressions;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A helper class that wraps <see cref="Regex"/> with safe handling for null input scenarios.
    /// </summary>
    public static class RegexNullSafe
    {
        /// <summary>
        /// A wrapper around <see cref="Regex.IsMatch(string,string)"/> that returns false if either
        /// input or pattern are null instead of throwing an <see cref="System.ArgumentNullException"/>.
        /// </summary>
        /// <param name="input">The string to search for a match.</param>
        /// <param name="pattern">The regular expression pattern to match.</param>
        /// <returns>True if the regular expression finds a match; otherwise, false.</returns>
        public static bool IsMatch(string input, string pattern)
        {
            if (input == null || pattern == null)
            {
                OutputWindowHelper.WarningWriteLine("Returned false due to a null input or pattern passed to RegexNullSafe's IsMatch.");
                return false;
            }

            return Regex.IsMatch(input, pattern);
        }
    }
}