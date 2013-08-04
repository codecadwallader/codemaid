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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EnvDTE;
using SteveCadwallader.CodeMaid.Model.CodeItems;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A static helper class for working with the code model.
    /// </summary>
    /// <remarks>
    /// Note:  All POSIXRegEx text replacements search against '\n' but insert/replace
    ///        with Environment.NewLine.  This handles line endings correctly.
    /// </remarks>
    internal static class CodeModelHelper
    {
        #region Internal Methods

        /// <summary>
        /// Gets the specified code items as unique blocks by consecutive line positioning.
        /// </summary>
        /// <typeparam name="T">The type of the code item.</typeparam>
        /// <param name="codeItems">The code items.</param>
        /// <returns>An enumerable collection of blocks of code items.</returns>
        internal static IEnumerable<IList<T>> GetCodeItemBlocks<T>(IEnumerable<T> codeItems)
            where T : BaseCodeItem
        {
            var codeItemBlocks = new List<IList<T>>();
            IList<T> currentBlock = null;

            var orderedCodeItems = codeItems.OrderBy(x => x.StartLine);
            foreach (T codeItem in orderedCodeItems)
            {
                if (currentBlock != null &&
                    (codeItem.StartLine <= currentBlock.Last().EndLine + 1))
                {
                    // This item belongs in the current block, add it.
                    currentBlock.Add(codeItem);
                }
                else
                {
                    // This item starts a new block, create one.
                    currentBlock = new List<T> { codeItem };
                    codeItemBlocks.Add(currentBlock);
                }
            }

            return codeItemBlocks;
        }

        /// <summary>
        /// Walks the given document and constructs a <see cref="SetCodeItems"/> of CodeItems within it excluding regions.
        /// </summary>
        /// <param name="document">The document to walk.</param>
        /// <returns>The set of code items within the document, excluding regions.</returns>
        internal static SetCodeItems RetrieveCodeItemsExcludingRegions(Document document)
        {
            var codeItems = new SetCodeItems();

            RetrieveCodeItems(codeItems, document.ProjectItem.FileCodeModel);

            return codeItems;
        }

        /// <summary>
        /// Walks the given document and constructs a <see cref="SetCodeItems"/> of CodeItems within it including regions.
        /// </summary>
        /// <param name="document">The document to walk.</param>
        /// <returns>The set of code items within the document, including regions.</returns>
        internal static SetCodeItems RetrieveCodeItemsIncludingRegions(Document document)
        {
            var codeItems = RetrieveCodeItemsExcludingRegions(document);

            RetrieveCodeRegions(codeItems, document);

            return codeItems;
        }

        #endregion Internal Methods

        #region Private Methods

        /// <summary>
        /// Walks the given FileCodeModel, turning CodeElements into code items within the specified code items set.
        /// </summary>
        /// <param name="codeItems">The code items set for accumulation.</param>
        /// <param name="fcm">The FileCodeModel to walk.</param>
        private static void RetrieveCodeItems(SetCodeItems codeItems, FileCodeModel fcm)
        {
            if (fcm != null)
            {
                foreach (CodeElement codeElement in fcm.CodeElements)
                {
                    RetrieveCodeItemsRecursively(codeItems, codeElement);
                }
            }
        }

        /// <summary>
        /// Recursive method for creating a code item for the specified code element,
        /// adding it to the specified code items set and recursing into all of the code element's children.
        /// </summary>
        /// <param name="codeItems">The code items set for accumulation.</param>
        /// <param name="codeElement">The CodeElement to walk (add and recurse).</param>
        private static void RetrieveCodeItemsRecursively(SetCodeItems codeItems, CodeElement codeElement)
        {
            var parentCodeItem = FactoryCodeItems.CreateCodeItemElement(codeElement);
            if (parentCodeItem != null)
            {
                codeItems.Add(parentCodeItem);
            }

            if (codeElement.Children != null)
            {
                foreach (CodeElement child in codeElement.Children)
                {
                    RetrieveCodeItemsRecursively(codeItems, child);
                }
            }
        }

        /// <summary>
        /// Retrieves code regions from the specified document into the specifed code items set.
        /// </summary>
        /// <param name="codeItems">The code items set for accumulation.</param>
        /// <param name="document">The document to walk.</param>
        private static void RetrieveCodeRegions(SetCodeItems codeItems, Document document)
        {
            var textDocument = (TextDocument)document.Object("TextDocument");

            var regionStack = new Stack<CodeItemRegion>();                   // Nested working hierarchy.
            EditPoint cursor = textDocument.StartPoint.CreateEditPoint();    // The document cursor.
            TextRanges subGroupMatches = null;                               // Not used - required for FindPattern.
            string pattern = UsePOSIXRegEx(document) ? @"^:b*\#" : @"^[ \t]*#";

            // Keep pushing cursor forwards (FindPattern uses cursor as ref parameter) until finished.
            while (cursor != null &&
                   cursor.FindPattern(pattern, TextDocumentHelper.StandardFindOptions, ref cursor, ref subGroupMatches))
            {
                // Move the cursor back one character to pick up the '#' symbol.
                cursor.CharLeft(1);

                // Create a pointer to capture the text for this line.
                EditPoint eolCursor = cursor.CreateEditPoint();
                eolCursor.EndOfLine();
                string regionText = cursor.GetText(eolCursor);

                // Move the cursor back to the start of the line.
                cursor.StartOfLine();

                if (regionText.StartsWith("#region ")) // Space required by compiler.
                {
                    // Get the region name.
                    string regionName = regionText.Substring(8).Trim();

                    // Push the parsed region info onto the top of the stack.
                    regionStack.Push(new CodeItemRegion
                                         {
                                             Name = regionName,
                                             StartLine = cursor.Line,
                                             StartOffset = cursor.AbsoluteCharOffset,
                                             StartPoint = cursor.CreateEditPoint()
                                         });
                }
                else if (regionText.StartsWith("#endregion"))
                {
                    if (regionStack.Count > 0)
                    {
                        CodeItemRegion region = regionStack.Pop();
                        region.EndLine = eolCursor.Line;
                        region.EndOffset = eolCursor.AbsoluteCharOffset;
                        region.EndPoint = eolCursor.CreateEditPoint();

                        codeItems.Add(region);
                    }
                    else
                    {
                        // This document is improperly formatted, abort.
                        return;
                    }
                }

                // Move the cursor to the end of the line to continue searching.
                cursor.EndOfLine();
            }
        }

        /// <summary>
        /// Gets a flag indicating if POSIX regular expressions should be used for TextDocument Find/Replace actions.
        /// Applies to pre-Visual Studio 11 versions.
        /// Determines the IDE version through the specified document's DTE object.
        /// </summary>
        /// <param name="document">The document which can access the DTE object.</param>
        /// <returns>True is POSIX regular expressions should be used, otherwise false.</returns>
        private static bool UsePOSIXRegEx(Document document)
        {
            var dte = document.DTE;
            double ideVersion = Convert.ToDouble(dte.Version, CultureInfo.InvariantCulture);

            return ideVersion < 11;
        }

        #endregion Private Methods
    }
}