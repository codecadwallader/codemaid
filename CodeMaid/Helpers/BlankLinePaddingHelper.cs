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
using SteveCadwallader.CodeMaid.CodeItems;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A class for encapsulating blank line padding behavior.
    /// </summary>
    internal class BlankLinePaddingHelper
    {
        #region Constructors

        /// <summary>
        /// The singleton instance of the <see cref="BlankLinePaddingHelper"/> class.
        /// </summary>
        private static BlankLinePaddingHelper _instance;

        /// <summary>
        /// Gets an instance of the <see cref="BlankLinePaddingHelper"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="BlankLinePaddingHelper"/> class.</returns>
        internal static BlankLinePaddingHelper GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new BlankLinePaddingHelper(package));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlankLinePaddingHelper"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private BlankLinePaddingHelper(CodeMaidPackage package)
        {
            Package = package;
        }

        #endregion Constructors

        #region Calculation Methods

        /// <summary>
        /// Determines if the specified code item instance should be preceded by a blank line.
        /// Defaults to false for unknown kinds or null objects.
        /// </summary>
        /// <param name="codeItem">The code item.</param>
        /// <returns>True if code item should be preceded by a blank line, otherwise false.</returns>
        internal bool ShouldInstanceBePrecededByBlankLine(BaseCodeItemElement codeItem)
        {
            if (codeItem == null || codeItem.CodeElement == null)
            {
                return false;
            }

            bool shouldKindBePrecededByBlankLine = ShouldKindBePrecededByBlankLine(codeItem.Kind);

            if (shouldKindBePrecededByBlankLine)
            {
                if (codeItem.Kind == KindCodeItem.Field && codeItem.StartPoint.Line == codeItem.EndPoint.Line)
                {
                    return false;
                }
            }

            return shouldKindBePrecededByBlankLine;
        }

        /// <summary>
        /// Determines if the specified code item instance should be followed by a blank line.
        /// Defaults to false for unknown kinds or null objects.
        /// </summary>
        /// <param name="codeItem">The code item.</param>
        /// <returns>True if code item should be followed by a blank line, otherwise false.</returns>
        internal bool ShouldInstanceBeFollowedByBlankLine(BaseCodeItemElement codeItem)
        {
            if (codeItem == null || codeItem.CodeElement == null)
            {
                return false;
            }

            bool shouldKindBeFollowedByBlankLine = ShouldKindBeFollowedByBlankLine(codeItem.Kind);

            if (shouldKindBeFollowedByBlankLine)
            {
                if (codeItem.Kind == KindCodeItem.Field && codeItem.StartPoint.Line == codeItem.EndPoint.Line)
                {
                    return false;
                }
            }

            return shouldKindBeFollowedByBlankLine;
        }

        /// <summary>
        /// Determines if the specified kind of code item should generally be preceded by a blank line.
        /// Exceptions may apply at the instance level.  Defaults to false for unknown kinds.
        /// </summary>
        /// <param name="kindCodeItem">The kind of code item.</param>
        /// <returns>True if kind of code item should be preceded by a blank line, otherwise false.</returns>
        internal bool ShouldKindBePrecededByBlankLine(KindCodeItem kindCodeItem)
        {
            switch (kindCodeItem)
            {
                case KindCodeItem.Class:
                    return Package.Options.CleanupInsert.InsertBlankLinePaddingBeforeClasses;

                case KindCodeItem.Enum:
                    return Package.Options.CleanupInsert.InsertBlankLinePaddingBeforeEnumerations;

                case KindCodeItem.Event:
                    return Package.Options.CleanupInsert.InsertBlankLinePaddingBeforeEvents;

                case KindCodeItem.Constant:
                case KindCodeItem.Field:
                    return Package.Options.CleanupInsert.InsertBlankLinePaddingBeforeFieldsWithComments;

                case KindCodeItem.Interface:
                    return Package.Options.CleanupInsert.InsertBlankLinePaddingBeforeInterfaces;

                case KindCodeItem.Namespace:
                    return Package.Options.CleanupInsert.InsertBlankLinePaddingBeforeNamespaces;

                case KindCodeItem.Constructor:
                case KindCodeItem.Destructor:
                case KindCodeItem.Method:
                    return Package.Options.CleanupInsert.InsertBlankLinePaddingBeforeMethods;

                case KindCodeItem.Property:
                    return Package.Options.CleanupInsert.InsertBlankLinePaddingBeforeProperties;

                case KindCodeItem.Struct:
                    return Package.Options.CleanupInsert.InsertBlankLinePaddingBeforeStructs;

                case KindCodeItem.Using:
                    return Package.Options.CleanupInsert.InsertBlankLinePaddingBeforeUsingStatementBlocks;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines if the specified kind of code item should generally be followed by a blank line.
        /// Exceptions may apply at the instance level.  Defaults to false for unknown kinds.
        /// </summary>
        /// <param name="kindCodeItem">The kind of code item.</param>
        /// <returns>True if kind of code item should be followed by a blank line, otherwise false.</returns>
        internal bool ShouldKindBeFollowedByBlankLine(KindCodeItem kindCodeItem)
        {
            switch (kindCodeItem)
            {
                case KindCodeItem.Class:
                    return Package.Options.CleanupInsert.InsertBlankLinePaddingAfterClasses;

                case KindCodeItem.Enum:
                    return Package.Options.CleanupInsert.InsertBlankLinePaddingAfterEnumerations;

                case KindCodeItem.Event:
                    return Package.Options.CleanupInsert.InsertBlankLinePaddingAfterEvents;

                case KindCodeItem.Constant:
                case KindCodeItem.Field:
                    return Package.Options.CleanupInsert.InsertBlankLinePaddingAfterFieldsWithComments;

                case KindCodeItem.Interface:
                    return Package.Options.CleanupInsert.InsertBlankLinePaddingAfterInterfaces;

                case KindCodeItem.Namespace:
                    return Package.Options.CleanupInsert.InsertBlankLinePaddingAfterNamespaces;

                case KindCodeItem.Constructor:
                case KindCodeItem.Destructor:
                case KindCodeItem.Method:
                    return Package.Options.CleanupInsert.InsertBlankLinePaddingAfterMethods;

                case KindCodeItem.Property:
                    return Package.Options.CleanupInsert.InsertBlankLinePaddingAfterProperties;

                case KindCodeItem.Struct:
                    return Package.Options.CleanupInsert.InsertBlankLinePaddingAfterStructs;

                case KindCodeItem.Using:
                    return Package.Options.CleanupInsert.InsertBlankLinePaddingAfterUsingStatementBlocks;

                default:
                    return false;
            }
        }

        #endregion Calculation Methods

        #region Insertion Methods

        /// <summary>
        /// Inserts a blank line before #region tags except where adjacent to a brace
        /// for the specified text document.
        /// </summary>
        /// <param name="textDocument">The text document.</param>
        internal void InsertPaddingBeforeRegionTags(TextDocument textDocument)
        {
            if (!Package.Options.CleanupInsert.InsertBlankLinePaddingBeforeRegionTags) return;

            string pattern = Package.UsePOSIXRegEx
                                 ? @"{[^\n\{]}\n{:b*}\#region"
                                 : @"([^\r\n\{])\r?\n([ \t]*)#region";

            string replacement = Package.UsePOSIXRegEx
                                     ? @"\1" + Environment.NewLine + Environment.NewLine + @"\2\#region"
                                     : @"$1" + Environment.NewLine + Environment.NewLine + @"$2#region";

            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replacement);
        }

        /// <summary>
        /// Inserts a blank line after #region tags for the specified text document.
        /// </summary>
        /// <param name="textDocument">The text document.</param>
        internal void InsertPaddingAfterRegionTags(TextDocument textDocument)
        {
            if (!Package.Options.CleanupInsert.InsertBlankLinePaddingAfterRegionTags) return;

            string pattern = Package.UsePOSIXRegEx
                                 ? @"^{:b*}\#region{.*}\n{.}"
                                 : @"^([ \t]*)#region([^\r\n]*)\r?\n([^\r\n])";

            string replacement = Package.UsePOSIXRegEx
                                     ? @"\1\#region\2" + Environment.NewLine + Environment.NewLine + @"\3"
                                     : @"$1#region$2" + Environment.NewLine + Environment.NewLine + @"$3";

            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replacement);
        }

        /// <summary>
        /// Inserts a blank line before #endregion tags for the specified text document.
        /// </summary>
        /// <param name="textDocument">The text document.</param>
        internal void InsertPaddingBeforeEndRegionTags(TextDocument textDocument)
        {
            if (!Package.Options.CleanupInsert.InsertBlankLinePaddingBeforeEndRegionTags) return;

            string pattern = Package.UsePOSIXRegEx
                                 ? @"{.}\n{:b*}\#endregion"
                                 : @"([^\r\n])\r?\n([ \t]*)#endregion";

            string replacement = Package.UsePOSIXRegEx
                                     ? @"\1" + Environment.NewLine + Environment.NewLine + @"\2\#endregion"
                                     : @"$1" + Environment.NewLine + Environment.NewLine + @"$2#endregion";

            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replacement);
        }

        /// <summary>
        /// Inserts a blank line after #endregion tags except where adjacent to a brace
        /// for the specified text document.
        /// </summary>
        /// <param name="textDocument">The text document.</param>
        internal void InsertPaddingAfterEndRegionTags(TextDocument textDocument)
        {
            if (!Package.Options.CleanupInsert.InsertBlankLinePaddingAfterEndRegionTags) return;

            string pattern = Package.UsePOSIXRegEx
                                 ? @"^{:b*}\#endregion{.*}\n{:b*[^:b\}]}"
                                 : @"^([ \t]*)#endregion([^\r\n]*)\r?\n([ \t]*[^ \t\r\n\}])";

            string replacement = Package.UsePOSIXRegEx
                                     ? @"\1\#endregion\2" + Environment.NewLine + Environment.NewLine + @"\3"
                                     : @"$1#endregion$2" + Environment.NewLine + Environment.NewLine + @"$3";

            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replacement);
        }

        /// <summary>
        /// Inserts a blank line before the specified code elements except where adjacent to a brace.
        /// </summary>
        /// <typeparam name="T">The type of the code element.</typeparam>
        /// <param name="codeElements">The code elements to pad.</param>
        internal void InsertPaddingBeforeCodeElements<T>(IEnumerable<T> codeElements)
            where T : BaseCodeItemElement
        {
            foreach (T codeElement in codeElements.Where(ShouldInstanceBePrecededByBlankLine))
            {
                TextDocumentHelper.InsertBlankLineBeforePoint(codeElement.StartPoint);
            }
        }

        /// <summary>
        /// Inserts a blank line after the specified code elements except where adjacent to a brace.
        /// </summary>
        /// <typeparam name="T">The type of the code element.</typeparam>
        /// <param name="codeElements">The code elements to pad.</param>
        internal void InsertPaddingAfterCodeElements<T>(IEnumerable<T> codeElements)
            where T : BaseCodeItemElement
        {
            foreach (T codeElement in codeElements.Where(ShouldInstanceBeFollowedByBlankLine))
            {
                TextDocumentHelper.InsertBlankLineAfterPoint(codeElement.EndPoint);
            }
        }

        #endregion Insertion Methods

        #region Private Properties

        /// <summary>
        /// Gets or sets the hosting package.
        /// </summary>
        private CodeMaidPackage Package { get; set; }

        #endregion Private Properties
    }
}