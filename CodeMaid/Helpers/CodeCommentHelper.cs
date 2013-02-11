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

using EnvDTE;
using System;

namespace SteveCadwallader.CodeMaid.Helpers
{
    internal static class CodeCommentHelper
    {
        /// <summary>
        /// Get a collection of available comment regex patters for the given document.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        internal static string[] GetCommentPatternsForDocument(TextDocument document, string prefix)
        {
            string defaultPattern = String.Format(@"(?<prefix>{0}) (?<line>.*)(\r?\n\s*\k<prefix> (?<line>.*))*", prefix);

            switch (document.Parent.Language)
            {
                case "CSharp":
                case "C/C++":
                case "JavaScript":
                case "JScript":
                    return new[] {
                        @"(?<prefix>/\*) (?<line>.*)(\r?\n\s*\* (?<line>.*))*(\r?\n\s*)?\*/",
                        defaultPattern
                    };

                case "Basic":
                    return new[] { defaultPattern };

                default:
                    return null;
            }
        }

        /// <summary>
        /// Get the comment prefix for the given document's language.
        /// </summary>
        /// <param name="document"></param>
        /// <returns>The comment prefix regex, without trailing spaces.</returns>
        internal static string GetCommentPrefixForDocument(TextDocument document)
        {
            switch (document.Parent.Language)
            {
                case "CSharp":
                case "C/C++":
                case "JavaScript":
                case "JScript":
                    return "//+";

                case "Basic":
                    return "'+";

                default:
                    return null;
            }
        }

        /// <summary>
        /// Check if the line after the cursor is commented-out code.
        /// </summary>
        /// <param name="cursor"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        internal static bool IsCommentedCodeAfter(EditPoint cursor, string prefix)
        {
            var to = cursor.CreateEditPoint();
            to.LineDown();
            to.EndOfLine();
            var text = cursor.GetText(to);
            return System.Text.RegularExpressions.Regex.IsMatch(text, String.Format(@"{0}(?! )", prefix));
        }

        /// <summary>
        /// Check if the line before the cursor is commented-out code.
        /// </summary>
        /// <param name="cursor"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        internal static bool IsCommentedCodeBefore(EditPoint cursor, string prefix)
        {
            var from = cursor.CreateEditPoint();
            from.LineUp();
            from.StartOfLine();
            var text = from.GetText(cursor);
            return System.Text.RegularExpressions.Regex.IsMatch(text, String.Format(@"{0}(?! )", prefix));
        }
    }
}