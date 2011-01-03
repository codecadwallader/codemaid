#region CodeMaid is Copyright 2007-2011 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2011 Steve Cadwallader.

using System;
using System.Text.RegularExpressions;
using EnvDTE;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A static helper class for working with text documents.
    /// </summary>
    /// <remarks>
    /// Note:  All text replacements search against '\n' but insert/replace
    ///        with Environment.NewLine.  This handles line endings correctly.
    /// </remarks>
    internal static class TextDocumentHelper
    {
        #region Internal Constants

        /// <summary>
        /// The common set of options to be used for find and replace patterns.
        /// </summary>
        // ReSharper disable BitwiseOperatorOnEnumWihtoutFlags
        internal const int StandardFindOptions = (int)(vsFindOptions.vsFindOptionsRegularExpression |
                                                       vsFindOptions.vsFindOptionsMatchInHiddenText);
        // ReSharper restore BitwiseOperatorOnEnumWihtoutFlags

        #endregion Internal Constants

        #region Internal Methods

        /// <summary>
        /// Gets the text between the specified start point and the first match.
        /// </summary>
        /// <param name="startPoint">The start point.</param>
        /// <param name="matchString">The match string.</param>
        /// <returns>The matching text, otherwise null.</returns>
        internal static string GetTextToFirstMatch(TextPoint startPoint, string matchString)
        {
            var startEditPoint = startPoint.CreateEditPoint();
            var endEditPoint = startEditPoint.CreateEditPoint();
            TextRanges subGroupMatches = null; // Not used - required for FindPattern.

            if (endEditPoint.FindPattern(matchString, StandardFindOptions, ref endEditPoint, ref subGroupMatches))
            {
                return startEditPoint.GetText(endEditPoint);
            }

            return null;
        }

        /// <summary>
        /// Inserts a blank line before the specified point except where adjacent to a brace.
        /// Also handles moving past any preceeding comment lines.
        /// </summary>
        /// <param name="point">The point.</param>
        internal static void InsertBlankLineBeforePoint(EditPoint point)
        {
            while (!point.AtStartOfDocument) // Move the cursor above any adjacent XML document comments.
            {
                point.LineUp(1);
                point.StartOfLine();
                string text = point.GetLines(point.Line, point.Line + 1);
                if (!Regex.IsMatch(text, @"\s*//")) // Keep moving until we hit a non XML document line.
                {
                    if (Regex.IsMatch(text, @"\s*[^\s\{]")) // If it is not a scope boundary, insert newline.
                    {
                        point.EndOfLine();
                        point.Insert(Environment.NewLine);
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Inserts a blank line after the specified point except where adjacent to a brace.
        /// </summary>
        /// <param name="point">The point.</param>
        internal static void InsertBlankLineAfterPoint(EditPoint point)
        {
            if (point.AtEndOfDocument) return;

            point.LineDown(1);
            point.StartOfLine();
            string text = point.GetLines(point.Line, point.Line + 1);
            if (Regex.IsMatch(text, @"^\s*[^\s\}]"))
            {
                point.Insert(Environment.NewLine);
            }
        }

        /// <summary>
        /// Substitutes all occurrences in the specified text document of
        /// the specified pattern string with the specified replacement string.
        /// </summary>
        /// <param name="textDocument">The text document.</param>
        /// <param name="patternString">The pattern string.</param>
        /// <param name="replacementString">The replacement string.</param>
        internal static void SubstituteAllStringMatches(TextDocument textDocument, string patternString, string replacementString)
        {
            TextRanges dummy = null;
            while (textDocument.ReplacePattern(patternString, replacementString, StandardFindOptions, ref dummy))
            {
            }
        }

        /// <summary>
        /// Substitutes all occurrences in the specified text selection of
        /// the specified pattern string with the specified replacement string.
        /// </summary>
        /// <param name="textSelection">The text selection.</param>
        /// <param name="patternString">The pattern string.</param>
        /// <param name="replacementString">The replacement string.</param>
        internal static void SubstituteAllStringMatches(TextSelection textSelection, string patternString, string replacementString)
        {
            TextRanges dummy = null;
            while (textSelection.ReplacePattern(patternString, replacementString, StandardFindOptions, ref dummy))
            {
            }
        }

        #endregion Internal Methods
    }
}