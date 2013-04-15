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

using EnvDTE;
using System;

namespace SteveCadwallader.CodeMaid.Helpers
{
    internal static class CodeCommentHelper
    {
        /// <summary>
        /// Get the comment prefix (regex) for the given document's language.
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
        /// Check if the text is commented-out code.
        /// </summary>
        /// <param name="text">The text to check.</param>
        /// <param name="prefix">The comment prefix.</param>
        /// <returns><c>true</c> if commented-out code, else <c>false</c>.</returns>
        internal static bool IsCommentedCode(string text, string prefix)
        {
            // Use regex with alternation rather than character class, because otherwise "$" fails.
            return System.Text.RegularExpressions.Regex.IsMatch(text, String.Format(@"^\s*{0}(?!(\s|\r|\n|$))", prefix));
        }

        /// <summary>
        /// Check if the line after the cursor is commented-out code.
        /// </summary>
        /// <param name="cursor"></param>
        /// <param name="prefix">The comment prefix.</param>
        /// <returns><c>true</c> if commented-out code, else <c>false</c>.</returns>
        internal static bool IsCommentedCodeAfter(EditPoint cursor, string prefix)
        {
            if (cursor.AtEndOfDocument)
                return false;

            var to = cursor.CreateEditPoint();
            to.LineDown();
            to.EndOfLine();
            var from = to.CreateEditPoint();
            to.StartOfLine();
            return IsCommentedCode(cursor.GetText(to), prefix);
        }

        /// <summary>
        /// Check if the line before the cursor is commented-out code.
        /// </summary>
        /// <param name="cursor"></param>
        /// <param name="prefix">The comment prefix.</param>
        /// <returns><c>true</c> if commented-out code, else <c>false</c>.</returns>
        internal static bool IsCommentedCodeBefore(EditPoint cursor, string prefix)
        {
            if (cursor.AtStartOfDocument)
                return false;

            var from = cursor.CreateEditPoint();
            from.LineUp();
            from.StartOfLine();
            var to = from.CreateEditPoint();
            to.EndOfLine();
            return IsCommentedCode(from.GetText(to), prefix);
        }

        /// <summary>
        /// Create multi-line Regex pattern based on the comment prefix.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        internal static string PrefixToPattern(string prefix)
        {
            return String.Format(@"(?<prefix>{0})(?<line>( .*)?)(\r?\n\s*\k<prefix>(?<line>( .*)?))*", prefix);
        }

        /// <summary>
        /// Get the user's indenting settings.
        /// </summary>
        /// <param name="package">Package used to reference DTE.</param>
        /// <param name="document">Docoument used to get language.</param>
        /// <returns>An instance of indenting settings for this document.</returns>
        internal static CodeCommentIndentSettings GetIndentSettings(CodeMaidPackage package, TextDocument document)
        {
            return new CodeCommentIndentSettings(package.IDE.Properties["TextEditor", document.Parent.Language]);
        }

        internal class CodeCommentIndentSettings
        {
            public CodeCommentIndentSettings()
            {
                IndentStyle = vsIndentStyle.vsIndentStyleSmart;
                IndentSize = 4;
                TabSize = 4;
                InsertTabs = false;
            }

            public CodeCommentIndentSettings(EnvDTE.Properties settings)
            {
                IndentStyle = (vsIndentStyle)settings.Item("IndentStyle").Value;
                IndentSize = (short)settings.Item("IndentSize").Value;
                TabSize = (short)settings.Item("TabSize").Value;
                InsertTabs = (bool)settings.Item("InsertTabs").Value;
            }

            public short IndentSize { get; private set; }

            public vsIndentStyle IndentStyle { get; private set; }

            public bool InsertTabs { get; private set; }

            public short TabSize { get; private set; }
        }
    }
}