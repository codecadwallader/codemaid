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

using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using EnvDTE80;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Properties;

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
        /// The singleton instance of the <see cref="UpdateLogic"/> class.
        /// </summary>
        private static UpdateLogic _instance;

        /// <summary>
        /// Gets an instance of the <see cref="UpdateLogic"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="UpdateLogic"/> class.</returns>
        internal static UpdateLogic GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new UpdateLogic(package));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateLogic"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private UpdateLogic(CodeMaidPackage package)
        {
            _package = package;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Updates the #endregion directives to match the names of the matching
        /// #region directive and cleans up any unnecessary white space.
        /// </summary>
        /// <remarks>
        /// This code is very similar to the Common region retrieval function, but
        /// since it manipulates the cursors during processing the logic is different
        /// enough to warrant a separate copy of the code.
        /// </remarks>
        /// <param name="textDocument">The text document to cleanup.</param>
        internal void UpdateEndRegionDirectives(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_UpdateEndRegionDirectives) return;

            var regionStack = new Stack<string>();
            EditPoint cursor = textDocument.StartPoint.CreateEditPoint();
            TextRanges subGroupMatches = null; // Not used - required for FindPattern.
            string pattern = _package.UsePOSIXRegEx ? @"^:b*\#" : @"^[ \t]*#";

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
        /// Updates single line methods by placing braces on separate lines.
        /// </summary>
        /// <param name="methods">The methods to update.</param>
        internal void UpdateSingleLineMethods(IEnumerable<CodeItemMethod> methods)
        {
            if (!Settings.Default.Cleaning_UpdateSingleLineMethods) return;

            var singleLineMethods = methods.Where(x => x.StartLine == x.EndLine && x.OverrideKind != vsCMOverrideKind.vsCMOverrideKindAbstract);
            foreach (var singleLineMethod in singleLineMethods)
            {
                var start = singleLineMethod.CodeFunction.GetStartPoint(vsCMPart.vsCMPartBody).CreateEditPoint();
                var end = singleLineMethod.CodeFunction.GetEndPoint(vsCMPart.vsCMPartBody).CreateEditPoint();

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
                singleLineMethod.StartPoint.SmartFormat(singleLineMethod.EndPoint);
            }
        }

        #endregion Methods
    }
}