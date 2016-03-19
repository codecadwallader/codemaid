using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

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
        /// The singleton instance of the <see cref="InsertBlankLinePaddingLogic" /> class.
        /// </summary>
        private static InsertBlankLinePaddingLogic _instance;

        /// <summary>
        /// Gets an instance of the <see cref="InsertBlankLinePaddingLogic" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="InsertBlankLinePaddingLogic" /> class.</returns>
        internal static InsertBlankLinePaddingLogic GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new InsertBlankLinePaddingLogic(package));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertBlankLinePaddingLogic" /> class.
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
        internal bool ShouldBePrecededByBlankLine(BaseCodeItem codeItem)
        {
            if (codeItem == null)
            {
                return false;
            }

            switch (codeItem.Kind)
            {
                case KindCodeItem.Class:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingBeforeClasses;

                case KindCodeItem.Delegate:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingBeforeDelegates;

                case KindCodeItem.Enum:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingBeforeEnumerations;

                case KindCodeItem.Event:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingBeforeEvents;

                case KindCodeItem.Field:
                    return codeItem.IsMultiLine && Settings.Default.Cleaning_InsertBlankLinePaddingBeforeFieldsMultiLine;

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
                    return codeItem.IsMultiLine
                        ? Settings.Default.Cleaning_InsertBlankLinePaddingBeforePropertiesMultiLine
                        : Settings.Default.Cleaning_InsertBlankLinePaddingBeforePropertiesSingleLine;

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
        /// Determines if the specified code item instance should be followed by a blank line.
        /// Defaults to false for unknown kinds or null objects.
        /// </summary>
        /// <param name="codeItem">The code item.</param>
        /// <returns>True if code item should be followed by a blank line, otherwise false.</returns>
        internal bool ShouldBeFollowedByBlankLine(BaseCodeItem codeItem)
        {
            if (codeItem == null)
            {
                return false;
            }

            switch (codeItem.Kind)
            {
                case KindCodeItem.Class:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingAfterClasses;

                case KindCodeItem.Delegate:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingAfterDelegates;

                case KindCodeItem.Enum:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingAfterEnumerations;

                case KindCodeItem.Event:
                    return Settings.Default.Cleaning_InsertBlankLinePaddingAfterEvents;

                case KindCodeItem.Field:
                    return codeItem.IsMultiLine && Settings.Default.Cleaning_InsertBlankLinePaddingAfterFieldsMultiLine;

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
                    return codeItem.IsMultiLine
                        ? Settings.Default.Cleaning_InsertBlankLinePaddingAfterPropertiesMultiLine
                        : Settings.Default.Cleaning_InsertBlankLinePaddingAfterPropertiesSingleLine;

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
        /// Inserts a blank line before #region tags except where adjacent to a brace.
        /// </summary>
        /// <param name="regions">The regions to pad.</param>
        internal void InsertPaddingBeforeRegionTags(IEnumerable<CodeItemRegion> regions)
        {
            if (!Settings.Default.Cleaning_InsertBlankLinePaddingBeforeRegionTags) return;

            foreach (var region in regions.Where(x => !x.IsInvalidated))
            {
                var startPoint = region.StartPoint.CreateEditPoint();

                TextDocumentHelper.InsertBlankLineBeforePoint(startPoint);
            }
        }

        /// <summary>
        /// Inserts a blank line after #region tags except where adjacent to a brace.
        /// </summary>
        /// <param name="regions">The regions to pad.</param>
        internal void InsertPaddingAfterRegionTags(IEnumerable<CodeItemRegion> regions)
        {
            if (!Settings.Default.Cleaning_InsertBlankLinePaddingAfterRegionTags) return;

            foreach (var region in regions.Where(x => !x.IsInvalidated))
            {
                var startPoint = region.StartPoint.CreateEditPoint();

                TextDocumentHelper.InsertBlankLineAfterPoint(startPoint);
            }
        }

        /// <summary>
        /// Inserts a blank line before #endregion tags except where adjacent to a brace.
        /// </summary>
        /// <param name="regions">The regions to pad.</param>
        internal void InsertPaddingBeforeEndRegionTags(IEnumerable<CodeItemRegion> regions)
        {
            if (!Settings.Default.Cleaning_InsertBlankLinePaddingBeforeEndRegionTags) return;

            foreach (var region in regions.Where(x => !x.IsInvalidated))
            {
                var endPoint = region.EndPoint.CreateEditPoint();

                TextDocumentHelper.InsertBlankLineBeforePoint(endPoint);
            }
        }

        /// <summary>
        /// Inserts a blank line after #endregion tags except where adjacent to a brace.
        /// </summary>
        /// <param name="regions">The regions to pad.</param>
        internal void InsertPaddingAfterEndRegionTags(IEnumerable<CodeItemRegion> regions)
        {
            if (!Settings.Default.Cleaning_InsertBlankLinePaddingAfterEndRegionTags) return;

            foreach (var region in regions.Where(x => !x.IsInvalidated))
            {
                var endPoint = region.EndPoint.CreateEditPoint();

                TextDocumentHelper.InsertBlankLineAfterPoint(endPoint);
            }
        }

        /// <summary>
        /// Inserts a blank line before the specified code elements except where adjacent to a brace.
        /// </summary>
        /// <typeparam name="T">The type of the code element.</typeparam>
        /// <param name="codeElements">The code elements to pad.</param>
        internal void InsertPaddingBeforeCodeElements<T>(IEnumerable<T> codeElements)
            where T : BaseCodeItemElement
        {
            foreach (T codeElement in codeElements.Where(ShouldBePrecededByBlankLine))
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
            foreach (T codeElement in codeElements.Where(ShouldBeFollowedByBlankLine))
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

            const string pattern = @"(^[ \t]*)(break;|return([ \t][^;]*)?;)\r?\n([ \t]*)(case|default)";
            string replacement = @"$1$2" + Environment.NewLine + Environment.NewLine + @"$4$5";

            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replacement);
        }

        /// <summary>
        /// Inserts a blank line before single line comments except where adjacent to a brace,
        /// another single line comment line or a quadruple slash comment.
        /// </summary>
        /// <param name="textDocument">The text document.</param>
        internal void InsertPaddingBeforeSingleLineComments(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_InsertBlankLinePaddingBeforeSingleLineComments) return;

            const string pattern = @"(^[ \t]*(?!//)[^ \t\r\n\{].*\r?\n)([ \t]*//)(?!//)";
            string replacement = @"$1" + Environment.NewLine + @"$2";

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