#region CodeMaid is Copyright 2007-2013 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2013 Steve Cadwallader.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using EnvDTE;
using SteveCadwallader.CodeMaid.Model.CodeItems;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A static helper class for working with text documents.
    /// </summary>
    /// <remarks>
    ///
    /// Note: All POSIXRegEx text replacements search against '\n' but insert/replace with
    ///       Environment.NewLine. This handles line endings correctly.
    /// </remarks>
    internal static class TextDocumentHelper
    {
        #region Internal Constants

        /// <summary>
        /// The common set of options to be used for find and replace patterns.
        /// </summary>
        internal const int StandardFindOptions = (int)(vsFindOptions.vsFindOptionsRegularExpression |
                                                       vsFindOptions.vsFindOptionsMatchInHiddenText);

        #endregion Internal Constants

        #region Internal Methods

        /// <summary>
        /// Finds all matches of the specified pattern within the specified text document.
        /// </summary>
        /// <param name="textDocument">The text document.</param>
        /// <param name="patternString">The pattern string.</param>
        /// <returns>The set of matches.</returns>
        internal static IEnumerable<EditPoint> FindMatches(TextDocument textDocument, string patternString)
        {
            var matches = new List<EditPoint>();
            var cursor = textDocument.StartPoint.CreateEditPoint();
            EditPoint end = null;
            TextRanges dummy = null;

            while (cursor != null && cursor.FindPattern(patternString, StandardFindOptions, ref end, ref dummy))
            {
                matches.Add(cursor.CreateEditPoint());
                cursor = end;
            }

            return matches;
        }

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
        /// </summary>
        /// <param name="point">The point.</param>
        internal static void InsertBlankLineBeforePoint(EditPoint point)
        {
            if (point.Line <= 1) return;

            point.LineUp(1);
            point.StartOfLine();

            string text = point.GetLine();
            if (Regex.IsMatch(text, @"^\s*[^\s\{]")) // If it is not a scope boundary, insert newline.
            {
                point.EndOfLine();
                point.Insert(Environment.NewLine);
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

            string text = point.GetLine();
            if (Regex.IsMatch(text, @"^\s*[^\s\}]"))
            {
                point.Insert(Environment.NewLine);
            }
        }

        /// <summary>
        /// Attempts to move the cursor to the specified code item.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="codeItem">The code item.</param>
        /// <param name="centerOnWhole">True if the whole element should be used for centering.</param>
        internal static void MoveToCodeItem(Document document, BaseCodeItem codeItem, bool centerOnWhole)
        {
            var textDocument = document.Object("TextDocument") as TextDocument;
            if (textDocument == null) return;

            try
            {
                object viewRangeEnd = null;
                TextPoint navigatePoint = null;

                codeItem.RefreshCachedPositionAndName();
                textDocument.Selection.MoveToPoint(codeItem.StartPoint, false);

                if (centerOnWhole)
                {
                    viewRangeEnd = codeItem.EndPoint;
                }

                var codeItemElement = codeItem as BaseCodeItemElement;
                if (codeItemElement != null)
                {
                    navigatePoint = codeItemElement.CodeElement.GetStartPoint(vsCMPart.vsCMPartNavigate);
                }

                textDocument.Selection.AnchorPoint.TryToShow(vsPaneShowHow.vsPaneShowCentered, viewRangeEnd);

                if (navigatePoint != null)
                {
                    textDocument.Selection.MoveToPoint(navigatePoint, false);
                }
                else
                {
                    textDocument.Selection.FindText(codeItem.Name, (int)vsFindOptions.vsFindOptionsMatchInHiddenText);
                    textDocument.Selection.MoveToPoint(textDocument.Selection.AnchorPoint, false);
                }
            }
            catch (Exception)
            {
                // Move operation may fail if element is no longer available.
            }
            finally
            {
                // Always set focus within the code editor window.
                document.Activate();
            }
        }

        /// <summary>
        /// Attempts to select the text of the specified code item.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="codeItem">The code item.</param>
        internal static void SelectCodeItem(Document document, BaseCodeItem codeItem)
        {
            var textDocument = document.Object("TextDocument") as TextDocument;
            if (textDocument == null) return;

            try
            {
                codeItem.RefreshCachedPositionAndName();
                textDocument.Selection.MoveToPoint(codeItem.StartPoint, false);
                textDocument.Selection.MoveToPoint(codeItem.EndPoint, true);

                textDocument.Selection.SwapAnchor();
            }
            catch (Exception)
            {
                // Select operation may fail if element is no longer available.
            }
            finally
            {
                // Always set focus within the code editor window.
                document.Activate();
            }
        }

        /// <summary>
        /// Substitutes all occurrences in the specified text document of the specified pattern
        /// string with the specified replacement string.
        /// </summary>
        /// <param name="textDocument">The text document.</param>
        /// <param name="patternString">The pattern string.</param>
        /// <param name="replacementString">The replacement string.</param>
        internal static void SubstituteAllStringMatches(TextDocument textDocument, string patternString, string replacementString)
        {
            TextRanges dummy = null;
            int lastCount = -1;
            while (textDocument.ReplacePattern(patternString, replacementString, StandardFindOptions, ref dummy))
            {
                // it is possible that the replacements aren't actually being done. In such a case,
                // we can detect the situation by seeing if the count always remains the same, and
                // if so exiting early.
                if (lastCount == dummy.Count)
                {
                    OutputWindowHelper.WriteLine("CodeMaid had to force a break out of TextDocumentHelper's SubstituteAllStringMatches for a document.");
                    break;
                }
                lastCount = dummy.Count;
            }
        }

        /// <summary>
        /// Substitutes all occurrences in the specified text selection of the specified pattern
        /// string with the specified replacement string.
        /// </summary>
        /// <param name="textSelection">The text selection.</param>
        /// <param name="patternString">The pattern string.</param>
        /// <param name="replacementString">The replacement string.</param>
        internal static void SubstituteAllStringMatches(TextSelection textSelection, string patternString, string replacementString)
        {
            TextRanges dummy = null;
            int lastCount = -1;
            while (textSelection.ReplacePattern(patternString, replacementString, StandardFindOptions, ref dummy))
            {
                // it is possible that the replacements aren't actually being done. In such a case,
                // we can detect the situation by seeing if the count always remains the same, and
                // if so exiting early.
                if (lastCount == dummy.Count)
                {
                    OutputWindowHelper.WriteLine("CodeMaid had to force a break out of TextDocumentHelper's SubstituteAllStringMatches for a selection.");
                    break;
                }
                lastCount = dummy.Count;
            }
        }

        /// <summary>
        /// Substitutes all occurrences between the specified start and end points of the specified
        /// pattern string with the specified replacement string.
        /// </summary>
        /// <param name="startPoint">The start point.</param>
        /// <param name="endPoint">The end point.</param>
        /// <param name="patternString">The pattern string.</param>
        /// <param name="replacementString">The replacement string.</param>
        internal static void SubstituteAllStringMatches(EditPoint startPoint, EditPoint endPoint, string patternString, string replacementString)
        {
            TextRanges dummy = null;
            int lastCount = -1;
            while (startPoint.ReplacePattern(endPoint, patternString, replacementString, StandardFindOptions, ref dummy))
            {
                // it is possible that the replacements aren't actually being done. In such a case,
                // we can detect the situation by seeing if the count always remains the same, and
                // if so exiting early.
                if (lastCount == dummy.Count)
                {
                    OutputWindowHelper.WriteLine("CodeMaid had to force a break out of TextDocumentHelper's SubstituteAllStringMatches for a pair of points.");
                    break;
                }
                lastCount = dummy.Count;
            }
        }

        #endregion Internal Methods
    }
}