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
using System.Globalization;
using System.Text.RegularExpressions;
using EnvDTE;
using SteveCadwallader.CodeMaid.CodeItems;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A static helper class for working with the code model.
    /// </summary>
    /// <remarks>
    /// Note:  All text replacements search against '\n' but insert/replace
    ///        with Environment.NewLine.  This handles line endings correctly.
    /// </remarks>
    internal static class CodeModelHelper
    {
        #region Internal Methods

        /// <summary>
        /// Calculates the complexity of the given element.
        /// </summary>
        /// <param name="element">The code element to examine.</param>
        /// <returns>The calculated complexity.</returns>
        internal static int CalculateComplexity(CodeElement element)
        {
            EditPoint startPoint = element.StartPoint.CreateEditPoint();
            string functionText = startPoint.GetText(element.EndPoint);

            // Rip out single line comments.
            functionText = Regex.Replace(functionText, @"//.*" + Environment.NewLine, Environment.NewLine);

            // Rip out multi-line comments.
            functionText = Regex.Replace(functionText, @"/\*.*\*/", String.Empty, RegexOptions.Singleline);

            // Rip out strings.
            functionText = Regex.Replace(functionText, @"""[^""]*""", String.Empty);

            // Rip out characters.
            functionText = Regex.Replace(functionText, @"'[^']*'", String.Empty);

            int ifCount = Regex.Matches(functionText, @"\sif[\s\(]").Count;
            int elseCount = Regex.Matches(functionText, @"\selse\s").Count;
            int elseIfCount = Regex.Matches(functionText, @"\selse if[\s\(]").Count;
            int whileCount = Regex.Matches(functionText, @"\swhile[\s\(]").Count;
            int forCount = Regex.Matches(functionText, @"\sfor[\s\(]").Count;
            int forEachCount = Regex.Matches(functionText, @"\sforeach[\s\(]").Count;
            int switchCount = Regex.Matches(functionText, @"\sswitch[\s\(]").Count;
            int caseCount = Regex.Matches(functionText, @"\scase\s[^;]*;").Count;
            int catchCount = Regex.Matches(functionText, @"\scatch[\s\(]").Count;
            int tertiaryCount = Regex.Matches(functionText, @"\s\?\s").Count;
            int andCount = Regex.Matches(functionText, @"\&\&").Count;
            int orCount = Regex.Matches(functionText, @"\|\|").Count;

            int complexity = 1 +
                ifCount + elseCount - elseIfCount + // else if will have been counted twice already by 'if' and 'else'
                whileCount + forCount + forEachCount + switchCount + caseCount +
                catchCount + tertiaryCount + andCount + orCount;

            return complexity;
        }

        /// <summary>
        /// Gets the keyword for the specified access modifier.
        /// </summary>
        /// <param name="accessModifier">The access modifier.</param>
        /// <returns>The matching keyword, otherwise null.</returns>
        internal static string GetAccessModifierKeyword(vsCMAccess accessModifier)
        {
            switch (accessModifier)
            {
                case vsCMAccess.vsCMAccessPublic: return "public";
                case vsCMAccess.vsCMAccessProtected: return "protected";
                case vsCMAccess.vsCMAccessPrivate: return "private";
                case vsCMAccess.vsCMAccessProject: return "internal";
                default: return null;
            }
        }

        /// <summary>
        /// Gets the declaration of the specified code class as a string.
        /// </summary>
        /// <param name="codeClass">The code class.</param>
        /// <returns>The string declaration.</returns>
        internal static string GetClassDeclaration(CodeClass codeClass)
        {
            // Get the start point after the attributes.
            var startPoint = codeClass.GetStartPoint(vsCMPart.vsCMPartHeader);

            return TextDocumentHelper.GetTextToFirstMatch(startPoint, @"\{");
        }

        /// <summary>
        /// Gets the declaration of the specified code enum as a string.
        /// </summary>
        /// <param name="codeEnum">The code enum.</param>
        /// <returns>The string declaration.</returns>
        internal static string GetEnumerationDeclaration(CodeEnum codeEnum)
        {
            // Get the start point after the attributes.
            var startPoint = codeEnum.GetStartPoint(vsCMPart.vsCMPartHeader);

            return TextDocumentHelper.GetTextToFirstMatch(startPoint, @"\{");
        }

        /// <summary>
        /// Gets the declaration of the specified code method as a string.
        /// </summary>
        /// <param name="codeFunction">The code method.</param>
        /// <returns>The string declaration.</returns>
        internal static string GetMethodDeclaration(CodeFunction codeFunction)
        {
            // Get the start point after the attributes.
            var startPoint = codeFunction.GetStartPoint(vsCMPart.vsCMPartHeader);

            return TextDocumentHelper.GetTextToFirstMatch(startPoint, @"[\{;]");
        }

        /// <summary>
        /// Gets the declaration of the specified code property as a string.
        /// </summary>
        /// <param name="codeProperty">The code property.</param>
        /// <returns>The string declaration.</returns>
        internal static string GetPropertyDeclaration(CodeProperty codeProperty)
        {
            // Get the start point at the end of the attributes if there are any (vsCMPartHeader is not available for properties).
            var startPoint = codeProperty.Attributes.Count > 0
                                 ? codeProperty.GetEndPoint(vsCMPart.vsCMPartAttributesWithDelimiter)
                                 : codeProperty.StartPoint;

            return TextDocumentHelper.GetTextToFirstMatch(startPoint, @"\{");
        }

        /// <summary>
        /// Gets the declaration of the specified code struct as a string.
        /// </summary>
        /// <param name="codeStruct">The code struct.</param>
        /// <returns>The string declaration.</returns>
        internal static string GetStructDeclaration(CodeStruct codeStruct)
        {
            // Get the start point after the attributes.
            var startPoint = codeStruct.GetStartPoint(vsCMPart.vsCMPartHeader);

            return TextDocumentHelper.GetTextToFirstMatch(startPoint, @"\{");
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
            TextDocument textDocument = (TextDocument)document.Object("TextDocument");

            Stack<CodeItemRegion> regionStack = new Stack<CodeItemRegion>(); // Nested working hierarchy.
            EditPoint cursor = textDocument.StartPoint.CreateEditPoint();    // The document cursor.
            TextRanges subGroupMatches = null;                               // Not used - required for FindPattern.
            string pattern = UsePOSIXRegEx(document) ? @"^:b*\#" : @"^[ \t]*#";

            // Keep pushing cursor forwards (FindPattern uses cursor as ref parameter) until finished.
            while (cursor != null &&
                   cursor.FindPattern(pattern, TextDocumentHelper.StandardFindOptions, ref cursor, ref subGroupMatches))
            {
                // Create a pointer to capture the text for this line.
                EditPoint eolCursor = cursor.CreateEditPoint();
                eolCursor.EndOfLine();
                string regionText = cursor.GetText(eolCursor);

                if (regionText.StartsWith("region ")) // Space required by compiler.
                {
                    // Get the region name.
                    string regionName = regionText.Substring(7).Trim();

                    // Push the parsed region info onto the top of the stack.
                    regionStack.Push(new CodeItemRegion { Name = regionName, StartLine = cursor.Line, StartOffset = cursor.AbsoluteCharOffset - 1 });
                }
                else if (regionText.StartsWith("endregion"))
                {
                    if (regionStack.Count > 0)
                    {
                        CodeItemRegion region = regionStack.Pop();
                        region.EndLine = cursor.Line;
                        region.EndOffset = eolCursor.AbsoluteCharOffset;

                        codeItems.Add(region);
                    }
                    else
                    {
                        // This document is improperly formatted, abort.
                        return;
                    }
                }

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