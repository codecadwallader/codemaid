#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

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
            return String.Format(@"(?<=^[ \t]*)(?<prefix>{0})(?<line>([ \t][^\r\n]*))(\r?\n[ \t]*\k<prefix>(?<line>([ \t][^\r\n]*)?))*", prefix);
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
                case "CSS":
                case "JavaScript":
                case "JScript":
                case "LESS":
                case "TypeScript":
                    return "///?";

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

        /// <summary>
        /// Creates the XML open tag string for an XElement.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>
        /// The XML open tag. In case of an element without value, the tag is self-closing.
        /// </returns>
        internal static string CreateXmlOpenTag(System.Xml.Linq.XElement element)
        {
            var builder = new System.Text.StringBuilder();
            builder.Append("<");
            builder.Append(element.Name);
            if (element.HasAttributes)
            {
                foreach (var attr in element.Attributes())
                {
                    builder.Append(" ");
                    builder.Append(attr.ToString());
                }
            }

            if (element.IsEmpty)
            {
                builder.Append("/");
            }

            builder.Append(">");
            return builder.ToString();
        }

        /// <summary>
        /// Creates the XML close tag string for an XElement.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>
        /// The XML close tag, or <c>null</c> if the element has no value and is a self-closing tag.
        /// </returns>
        internal static string CreateXmlCloseTag(System.Xml.Linq.XElement element)
        {
            if (element.IsEmpty)
                return null;

            return string.Format("</{0}>", element.Name);
        }
    }
}