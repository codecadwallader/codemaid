#region CodeMaid is Copyright 2007-2013 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2013 Steve Cadwallader.

using System;
using System.Text.RegularExpressions;
using EnvDTE;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A set of helper methods focused around code comments.
    /// </summary>
    internal static class CodeCommentHelper
    {
        /// <summary>
        /// Creates a multi-line regular expression pattern based on the comment prefix.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns>The regular expression pattern for matching comments.</returns>
        internal static string CreateCommentPatternFromPrefix(string prefix)
        {
            return String.Format(@"(?<prefix>{0})(?<line>( [^\r\n]*)?)(\r?\n[\t ]*\k<prefix>(?<line>( [^\r\n]*)?))*", prefix);
        }

        /// <summary>
        /// Get the comment prefix (regex) for the given document's language.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>The comment prefix regex, without trailing spaces.</returns>
        internal static string GetCommentPrefixForDocument(TextDocument document)
        {
            switch (document.Language)
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
        /// Get the user's indentation settings for the specified language.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <param name="language">The language.</param>
        /// <returns>An instance of indentation settings based on the specified language.</returns>
        internal static IndentSettings GetIndentSettings(CodeMaidPackage package, string language)
        {
            return new IndentSettings(package.IDE.Properties["TextEditor", language]);
        }

        /// <summary>
        /// Determines if the specified text appears to be commented-out code.
        /// </summary>
        /// <param name="text">The text to check.</param>
        /// <param name="prefix">The comment prefix.</param>
        /// <returns><c>true</c> if commented-out code, otherwise <c>false</c>.</returns>
        internal static bool IsCommentedOutCode(string text, string prefix)
        {
            // Use regex with alternation rather than character class, because otherwise "$" fails.
            return Regex.IsMatch(text, String.Format(@"^[ \t]*{0}(?!( |\t|\r|\n|$))", prefix));
        }

        /// <summary>
        /// Determines if the line after the cursor appears to be commented-out code.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        /// <param name="prefix">The comment prefix.</param>
        /// <returns><c>true</c> if commented-out code, otherwise <c>false</c>.</returns>
        internal static bool IsCommentedOutCodeAfter(EditPoint cursor, string prefix)
        {
            if (cursor.Line == cursor.Parent.EndPoint.Line) return false;

            var text = cursor.GetLines(cursor.Line + 1, cursor.Line + 2);

            return IsCommentedOutCode(text, prefix);
        }

        /// <summary>
        /// Determines if the line before the cursor appears to be commented-out code.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        /// <param name="prefix">The comment prefix.</param>
        /// <returns><c>true</c> if commented-out code, otherwise <c>false</c>.</returns>
        internal static bool IsCommentedOutCodeBefore(EditPoint cursor, string prefix)
        {
            if (cursor.Line == cursor.Parent.StartPoint.Line) return false;

            var text = cursor.GetLines(cursor.Line - 1, cursor.Line);

            return IsCommentedOutCode(text, prefix);
        }
    }
}