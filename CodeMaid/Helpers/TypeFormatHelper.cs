#region CodeMaid is Copyright 2007-2012 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2012 Steve Cadwallader.

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