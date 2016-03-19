using EnvDTE;
using EnvDTE80;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SteveCadwallader.CodeMaid.Logic.Cleaning
{
    /// <summary>
    /// A class for encapsulating the logic of general updates.
    /// </summary>
    internal class UpdateLogic
    {
        #region Fields

        private readonly CodeMaidPackage _package;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// The singleton instance of the <see cref="UpdateLogic" /> class.
        /// </summary>
        private static UpdateLogic _instance;

        /// <summary>
        /// Gets an instance of the <see cref="UpdateLogic" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="UpdateLogic" /> class.</returns>
        internal static UpdateLogic GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new UpdateLogic(package));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateLogic" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private UpdateLogic(CodeMaidPackage package)
        {
            _package = package;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Updates the #endregion directives to match the names of the matching #region directive
        /// and cleans up any unnecessary white space.
        /// </summary>
        /// <remarks>
        /// This code is very similar to the Common region retrieval function, but since it
        /// manipulates the cursors during processing the logic is different enough to warrant a
        /// separate copy of the code.
        /// </remarks>
        /// <param name="textDocument">The text document to cleanup.</param>
        internal void UpdateEndRegionDirectives(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_UpdateEndRegionDirectives) return;

            var regionStack = new Stack<string>();
            EditPoint cursor = textDocument.StartPoint.CreateEditPoint();
            TextRanges subGroupMatches = null; // Not used - required for FindPattern.
            const string pattern = @"^[ \t]*#";

            // Keep pushing cursor forwards (note ref cursor parameter) until finished.
            while (cursor != null &&
                   cursor.FindPattern(pattern, TextDocumentHelper.StandardFindOptions, ref cursor, ref subGroupMatches))
            {
                // Create a pointer to capture the text for this line.
                EditPoint eolCursor = cursor.CreateEditPoint();
                eolCursor.EndOfLine();
                string regionText = cursor.GetText(eolCursor);

                if (regionText.StartsWith("region ")) // Space required by compiler.
                {
                    // Cleanup any whitespace in the region name.
                    string regionName = regionText.Substring(7);
                    string regionNameTrimmed = regionName.Trim();
                    if (regionName != regionNameTrimmed)
                    {
                        cursor.CharRight(7);
                        cursor.Delete(eolCursor);
                        cursor.Insert(regionNameTrimmed);
                    }

                    // Push the parsed region name onto the top of the stack.
                    regionStack.Push(regionNameTrimmed);
                }
                else if (regionText.StartsWith("endregion")) // Space may or may not be present.
                {
                    if (regionStack.Count > 0)
                    {
                        // Do not trim the endRegionName in order to catch whitespace differences.
                        string endRegionName = regionText.Length > 9 ?
                            regionText.Substring(10) : String.Empty;
                        string matchingRegion = regionStack.Pop();

                        // Update if the strings do not match.
                        if (matchingRegion != endRegionName)
                        {
                            cursor.CharRight(9);
                            cursor.Delete(eolCursor);
                            cursor.Insert(" " + matchingRegion);
                        }
                    }
                    else
                    {
                        // This document is improperly formatted, abort.
                        return;
                    }
                }

                // Note: eolCursor may be outdated now if changes have been made.
                cursor.EndOfLine();
            }
        }

        /// <summary>
        /// Updates the event accessors to either both be single-line or multi-line.
        /// </summary>
        /// <param name="events">The events to update.</param>
        internal void UpdateEventAccessorsToBothBeSingleLineOrMultiLine(IEnumerable<CodeItemEvent> events)
        {
            if (!Settings.Default.Cleaning_UpdateAccessorsToBothBeSingleLineOrMultiLine) return;

            foreach (var item in events)
            {
                UpdateAccessorsToBothBeSingleLineOrMultiLine(item.CodeEvent.Adder, item.CodeEvent.Remover);
            }
        }

        /// <summary>
        /// Updates the property accessors to either both be single-line or multi-line.
        /// </summary>
        /// <param name="properties">The properties to update.</param>
        internal void UpdatePropertyAccessorsToBothBeSingleLineOrMultiLine(IEnumerable<CodeItemProperty> properties)
        {
            if (!Settings.Default.Cleaning_UpdateAccessorsToBothBeSingleLineOrMultiLine) return;

            foreach (var item in properties)
            {
                UpdateAccessorsToBothBeSingleLineOrMultiLine(item.CodeProperty.Getter, item.CodeProperty.Setter);
            }
        }

        /// <summary>
        /// Updates single line methods by placing braces on separate lines.
        /// </summary>
        /// <param name="methods">The methods to update.</param>
        internal void UpdateSingleLineMethods(IEnumerable<CodeItemMethod> methods)
        {
            if (!Settings.Default.Cleaning_UpdateSingleLineMethods) return;

            var singleLineMethods = methods.Where(x => x.StartPoint.Line == x.EndPoint.Line && x.OverrideKind != vsCMOverrideKind.vsCMOverrideKindAbstract && !(x.CodeFunction.Parent is CodeInterface));
            foreach (var singleLineMethod in singleLineMethods)
            {
                SpreadSingleLineMethodOntoMultipleLines(singleLineMethod.CodeFunction);
            }
        }

        /// <summary>
        /// Joins the specified multi-line method onto a single line.
        /// </summary>
        /// <param name="method">The method to update.</param>
        private void JoinMultiLineMethodOntoSingleLine(CodeFunction method)
        {
            var start = method.StartPoint.CreateEditPoint();
            var end = method.EndPoint.CreateEditPoint();

            const string pattern = @"[ \t]*\r?\n[ \t]*";
            const string replacement = @" ";

            // Substitute all new lines (and optional surrounding whitespace) with a single space.
            TextDocumentHelper.SubstituteAllStringMatches(start, end, pattern, replacement);
        }

        /// <summary>
        /// Spreads the specified single line method onto multiple lines.
        /// </summary>
        /// <param name="method">The method to update.</param>
        private void SpreadSingleLineMethodOntoMultipleLines(CodeFunction method)
        {
            try
            {
                var start = method.GetStartPoint(vsCMPart.vsCMPartBody).CreateEditPoint();
                var end = method.GetEndPoint(vsCMPart.vsCMPartBody).CreateEditPoint();

                // Insert a new-line before and after the opening brace.
                start.CharLeft();
                start.Insert(Environment.NewLine);
                start.CharRight();
                start.Insert(Environment.NewLine);

                // Insert a new-line before the closing brace, unless the method is empty.
                end.DeleteWhitespace();
                if (end.DisplayColumn > 1)
                {
                    end.Insert(Environment.NewLine);
                }

                // Update the formatting of the method.
                method.StartPoint.CreateEditPoint().SmartFormat(method.EndPoint);
            }
            catch (Exception)
            {
                // Methods may not have a body (ex: partial).
            }
        }

        /// <summary>
        /// Updates the specified accessors to both be single-line or multi-line.
        /// </summary>
        /// <param name="first">The first accessor.</param>
        /// <param name="second">The second accessor.</param>
        private void UpdateAccessorsToBothBeSingleLineOrMultiLine(CodeFunction first, CodeFunction second)
        {
            if (first == null || second == null) return;

            bool isFirstSingleLine = first.StartPoint.Line == first.EndPoint.Line;
            bool isSecondSingleLine = second.StartPoint.Line == second.EndPoint.Line;

            if (isFirstSingleLine == isSecondSingleLine) return;

            var multiLineMethod = isFirstSingleLine ? second : first;
            var singleLineMethod = isFirstSingleLine ? first : second;

            try
            {
                var multiLineBodyStart = multiLineMethod.GetStartPoint(vsCMPart.vsCMPartBody).CreateEditPoint();
                var multiLineBodyEnd = multiLineMethod.GetEndPoint(vsCMPart.vsCMPartBody).CreateEditPoint();

                // Move the body end back one character to account for new-lines.
                multiLineBodyEnd.CharLeft();

                bool multiLineHasSingleLineBody = multiLineBodyStart.Line == multiLineBodyEnd.Line;

                if (multiLineHasSingleLineBody)
                {
                    JoinMultiLineMethodOntoSingleLine(multiLineMethod);
                }
                else
                {
                    SpreadSingleLineMethodOntoMultipleLines(singleLineMethod);
                }
            }
            catch (Exception)
            {
                // Accessor without a body can be ignored.
            }
        }

        #endregion Methods
    }
}