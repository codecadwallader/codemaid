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
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.Logic.Cleaning
{
    /// <summary>
    /// A class for encapsulating insertion of blank line padding logic.
    /// </summary>
    internal class InsertBlankLinePaddingLogic
    {
        #region Fields

        private readonly CodeMaidPackage _package;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// The singleton instance of the <see cref="InsertBlankLinePaddingLogic"/> class.
        /// </summary>
        private static InsertBlankLinePaddingLogic _instance;

        /// <summary>
        /// Gets an instance of the <see cref="InsertBlankLinePaddingLogic"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="InsertBlankLinePaddingLogic"/> class.</returns>
        internal static InsertBlankLinePaddingLogic GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new InsertBlankLinePaddingLogic(package));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertBlankLinePaddingLogic"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private InsertBlankLinePaddingLogic(CodeMaidPackage package)
        {
            _package = package;
        }

        #endregion Constructors

        #region Calculation Methods

        /// <summary>
        /// Determines if the specified code item instance should be preceded by a blank line.
        /// Defaults to false for unknown kinds or null objects.
        /// </summary>
        /// <param name="codeItem">The code item.</param>
        /// <returns>True if code item should be preceded by a blank line, otherwise false.</returns>
        internal bool ShouldInstanceBePrecededByBlankLine(BaseCodeItem codeItem)
        {
            if (codeItem == null)
            {
                return false;
            }

            bool shouldKindBePrecededByBlankLine = ShouldKindBePrecededByBlankLine(codeItem.Kind);

            if (shouldKindBePrecededByBlankLine)
            {
                if ((codeItem.Kind == KindCodeItem.Constant ||
                     codeItem.Kind == KindCodeItem.Field) &&
                    codeItem.StartPoint.Line == codeItem.EndPoint.Line)
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
        internal bool ShouldInstanceBeFollowedByBlankLine(BaseCodeItem codeItem)
        {
            if (codeItem == null)
            {
                return false;
            }

            bool shouldKindBeFollowedByBlankLine = ShouldKindBeFollowedByBlankLine(codeItem.Kind);

            if (shouldKindBeFollowedByBlankLine)
            {
                if ((codeItem.Kind == KindCodeItem.Constant ||
                     codeItem.Kind == KindCodeItem.Field) &&
                    codeItem.StartPoint.Line == codeItem.EndPoint.Line)
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
                    return Settings.Default.Cleaning_InsertBlankLinePaddingBeforeClasses;

                case KindCodeItem.Delegate:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingBeforeDelegates;

                case KindCodeItem.Enum:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingBeforeEnumerations;

                case KindCodeItem.Event:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingBeforeEvents;

                case KindCodeItem.Constant:
                case KindCodeItem.Field:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingBeforeFieldsMultiLine;

                case KindCodeItem.Interface:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingBeforeInterfaces;

                case KindCodeItem.Namespace:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingBeforeNamespaces;

                case KindCodeItem.Constructor:
                case KindCodeItem.Destructor:
                case KindCodeItem.Method:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingBeforeMethods;

                case KindCodeItem.Indexer:
                case KindCodeItem.Property:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingBeforeProperties;

                case KindCodeItem.Region:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingBeforeRegionTags;

                case KindCodeItem.Struct:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingBeforeStructs;

                case KindCodeItem.Using:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingBeforeUsingStatementBlocks;

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
                    return Settings.Default.Cleaning_InsertBlankLinePaddingAfterClasses;

                case KindCodeItem.Delegate:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingAfterDelegates;

                case KindCodeItem.Enum:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingAfterEnumerations;

                case KindCodeItem.Event:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingAfterEvents;

                case KindCodeItem.Constant:
                case KindCodeItem.Field:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingAfterFieldsMultiLine;

                case KindCodeItem.Interface:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingAfterInterfaces;

                case KindCodeItem.Namespace:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingAfterNamespaces;

                case KindCodeItem.Constructor:
                case KindCodeItem.Destructor:
                case KindCodeItem.Method:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingAfterMethods;

                case KindCodeItem.Indexer:
                case KindCodeItem.Property:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingAfterProperties;

                case KindCodeItem.Region:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingAfterEndRegionTags;

                case KindCodeItem.Struct:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingAfterStructs;

                case KindCodeItem.Using:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingAfterUsingStatementBlocks;

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
            if (!Settings.Default.Cleaning_InsertBlankLinePaddingBeforeRegionTags) return;

            string pattern = _package.UsePOSIXRegEx
                                 ? @"{[^\n\{]}\n{:b*}\#region"
                                 : @"([^\r\n\{])\r?\n([ \t]*)#region";

            string replacement = _package.UsePOSIXRegEx
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
            if (!Settings.Default.Cleaning_InsertBlankLinePaddingAfterRegionTags) return;

            string pattern = _package.UsePOSIXRegEx
                                 ? @"^{:b*}\#region{.*}\n{.}"
                                 : @"^([ \t]*)#region([^\r\n]*)\r?\n([^\r\n])";

            string replacement = _package.UsePOSIXRegEx
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
            if (!Settings.Default.Cleaning_InsertBlankLinePaddingBeforeEndRegionTags) return;

            string pattern = _package.UsePOSIXRegEx
                                 ? @"{.}\n{:b*}\#endregion"
                                 : @"([^\r\n])\r?\n([ \t]*)#endregion";

            string replacement = _package.UsePOSIXRegEx
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
            if (!Settings.Default.Cleaning_InsertBlankLinePaddingAfterEndRegionTags) return;

            string pattern = _package.UsePOSIXRegEx
                                 ? @"^{:b*}\#endregion{.*}\n{:b*[^:b\}]}"
                                 : @"^([ \t]*)#endregion([^\r\n]*)\r?\n([ \t]*[^ \t\r\n\}])";

            string replacement = _package.UsePOSIXRegEx
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

        /// <summary>
        /// Inserts a blank line before case statements except for single-line case statements.
        /// </summary>
        /// <param name="textDocument">The text document.</param>
        internal void InsertPaddingBeforeCaseStatements(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_InsertBlankLinePaddingBeforeCaseStatements) return;

            string pattern = _package.UsePOSIXRegEx
                                 ? @"{^:b*}{break;|return;}\n{:b*}case"
                                 : @"(^[ \t]*)(break;|return;)\r?\n([ \t]*)case";

            string replacement = _package.UsePOSIXRegEx
                                     ? @"\1\2" + Environment.NewLine + Environment.NewLine + @"\3case"
                                     : @"$1$2" + Environment.NewLine + Environment.NewLine + @"$3case";

            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replacement);
        }

        /// <summary>
        /// Inserts a blank line between multi-line property accessors.
        /// </summary>
        /// <param name="properties">The properties.</param>
        internal void InsertPaddingBetweenMultiLinePropertyAccessors(IEnumerable<CodeItemProperty> properties)
        {
            if (!Settings.Default.Cleaning_InsertBlankLinePaddingBetweenPropertiesMultiLineAccessors) return;

            foreach (var property in properties)
            {
                var getter = property.CodeProperty.Getter;
                var setter = property.CodeProperty.Setter;

                if (getter != null && setter != null && (getter.StartPoint.Line < getter.EndPoint.Line ||
                                                         setter.StartPoint.Line < setter.EndPoint.Line))
                {
                    TextDocumentHelper.InsertBlankLineAfterPoint(setter.EndPoint.Line > getter.EndPoint.Line
                                                                     ? getter.EndPoint.CreateEditPoint()
                                                                     : setter.EndPoint.CreateEditPoint());
                }
            }
        }

        #endregion Insertion Methods
    }
}