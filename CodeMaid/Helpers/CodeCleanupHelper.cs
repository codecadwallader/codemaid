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
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A helper class for cleaning up code.
    /// </summary>
    /// <remarks>
    /// Note:  All POSIXRegEx text replacements search against '\n' but insert/replace
    ///        with Environment.NewLine.  This handles line endings correctly.
    /// </remarks>
    internal class CodeCleanupHelper
    {
        #region Fields

        private UndoTransactionHelper _undoTransactionHelper;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// The singleton instance of the <see cref="CodeCleanupHelper"/> class.
        /// </summary>
        private static CodeCleanupHelper _instance;

        /// <summary>
        /// Gets an instance of the <see cref="CodeCleanupHelper"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="CodeCleanupHelper"/> class.</returns>
        internal static CodeCleanupHelper GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new CodeCleanupHelper(package));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeCleanupHelper"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private CodeCleanupHelper(CodeMaidPackage package)
        {
            Package = package;

            BlankLinePaddingHelper = BlankLinePaddingHelper.GetInstance(Package);
            CodeCleanupAvailabilityHelper = CodeCleanupAvailabilityHelper.GetInstance(Package);
            ExplicitAccessModifierHelper = ExplicitAccessModifierHelper.GetInstance(Package);
            UsingStatementCleanupHelper = UsingStatementCleanupHelper.GetInstance(Package);
        }

        #endregion Constructors

        #region Internal Methods

        /// <summary>
        /// Attempts to run code cleanup on the specified project item.
        /// </summary>
        /// <param name="projectItem">The project item for cleanup.</param>
        internal void Cleanup(ProjectItem projectItem)
        {
            if (!CodeCleanupAvailabilityHelper.ShouldCleanup(projectItem)) return;

            // Attempt to open the document if not already opened.
            bool wasOpen = projectItem.IsOpen[Constants.vsViewKindTextView];
            if (!wasOpen)
            {
                try
                {
                    projectItem.Open(Constants.vsViewKindTextView);
                }
                catch (Exception)
                {
                    // OK if file cannot be opened (ex: deleted from disk, non-text based type.)
                }
            }

            if (projectItem.Document != null)
            {
                Cleanup(projectItem.Document, false);

                // Close the document if it was opened for cleanup.
                if (Settings.Default.Cleaning_AutoSaveAndCloseIfOpenedByCleanup && !wasOpen)
                {
                    projectItem.Document.Close(vsSaveChanges.vsSaveChangesYes);
                }
            }
        }

        /// <summary>
        /// Attempts to run code cleanup on the specified document.
        /// </summary>
        /// <param name="document">The document for cleanup.</param>
        /// <param name="isAutoSave">A flag indicating if occurring due to auto-save.</param>
        internal void Cleanup(Document document, bool isAutoSave)
        {
            if (!CodeCleanupAvailabilityHelper.ShouldCleanup(document)) return;

            // Make sure the document to be cleaned up is active, required for some commands like format document.
            document.Activate();

            // Conditionally start cleanup with reorganization.
            if (Settings.Default.Reorganizing_RunAtStartOfCleanup)
            {
                CodeReorderHelper.GetInstance(Package).Reorganize(document);
            }

            UndoTransactionHelper.Run(
                () => !isAutoSave,
                delegate
                {
                    var cleanupMethod = FindCodeCleanupMethod(document);
                    if (cleanupMethod != null)
                    {
                        Package.IDE.StatusBar.Text = String.Format("CodeMaid is cleaning '{0}'...", document.Name);
                        // Perform the set of configured cleanups based on the language.
                        cleanupMethod(document, isAutoSave);

                        Package.IDE.StatusBar.Text = String.Format("CodeMaid cleaned '{0}'.", document.Name);
                    }
                },
                delegate(Exception ex)
                {
                    OutputWindowHelper.WriteLine(String.Format("CodeMaid stopped cleaning '{0}': {1}", document.Name, ex));
                    Package.IDE.StatusBar.Text = String.Format("CodeMaid stopped cleaning '{0}'.  See output window for more details.", document.Name);
                });
        }

        #endregion Internal Methods

        #region Private Language Methods

        /// <summary>
        /// Finds a code cleanup method appropriate for the specified document, otherwise null.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>The code cleanup method, otherwise null.</returns>
        private Action<Document, bool> FindCodeCleanupMethod(Document document)
        {
            switch (document.Language)
            {
                case "CSharp":
                    return RunCodeCleanupCSharp;

                case "C/C++":
                case "CSS":
                case "JScript":
                    return RunCodeCleanupC;

                case "HTML":
                case "XAML":
                case "XML":
                    return RunCodeCleanupGeneric;

                default:
                    System.Diagnostics.Trace.WriteLine(String.Format(
                        "CodeMaid does not support document language '{0}'.", document.Language));
                    return null;
            }
        }

        /// <summary>
        /// Attempts to run code cleanup on the specified CSharp document.
        /// </summary>
        /// <param name="document">The document for cleanup.</param>
        /// <param name="isAutoSave">A flag indicating if occurring due to auto-save.</param>
        private void RunCodeCleanupCSharp(Document document, bool isAutoSave)
        {
            var textDocument = (TextDocument)document.Object("TextDocument");

            // Perform any actions that can modify the file code model first.
            RunVSFormatting(textDocument);
            UsingStatementCleanupHelper.RemoveUnusedUsingStatements(isAutoSave);
            UsingStatementCleanupHelper.SortUsingStatements();

            // Interpret the document into a collection of elements.
            var codeItems = CodeModelHelper.RetrieveCodeItemsExcludingRegions(document);

            var usingStatements = codeItems.OfType<CodeItemUsingStatement>().ToList();
            var namespaces = codeItems.OfType<CodeItemNamespace>().ToList();
            var classes = codeItems.OfType<CodeItemClass>().ToList();
            var delegates = codeItems.OfType<CodeItemDelegate>().ToList();
            var enumerations = codeItems.OfType<CodeItemEnum>().ToList();
            var events = codeItems.OfType<CodeItemEvent>().ToList();
            var fields = codeItems.OfType<CodeItemField>().ToList();
            var interfaces = codeItems.OfType<CodeItemInterface>().ToList();
            var methods = codeItems.OfType<CodeItemMethod>().ToList();
            var properties = codeItems.OfType<CodeItemProperty>().ToList();
            var structs = codeItems.OfType<CodeItemStruct>().ToList();

            // Build up more complicated collections.
            var usingStatementBlocks = GetCodeItemBlocks(usingStatements).ToList();
            var usingStatementsThatStartBlocks = (from IEnumerable<CodeItemUsingStatement> block in usingStatementBlocks select block.First()).ToList();
            var usingStatementsThatEndBlocks = (from IEnumerable<CodeItemUsingStatement> block in usingStatementBlocks select block.Last()).ToList();
            var fieldsWithComments = fields.Where(x => x.StartPoint.Line < x.EndPoint.Line).ToList();

            // Perform removal cleanup.
            RemoveEOLWhitespace(textDocument);
            RemoveBlankLinesAtTop(textDocument);
            RemoveBlankLinesAtBottom(textDocument);
            RemoveBlankLinesAfterAttributes(textDocument);
            RemoveBlankLinesAfterOpeningBrace(textDocument);
            RemoveBlankLinesBeforeClosingBrace(textDocument);
            RemoveMultipleConsecutiveBlankLines(textDocument);

            // Perform insertion of blank line padding cleanup.
            BlankLinePaddingHelper.InsertPaddingBeforeRegionTags(textDocument);
            BlankLinePaddingHelper.InsertPaddingAfterRegionTags(textDocument);

            BlankLinePaddingHelper.InsertPaddingBeforeEndRegionTags(textDocument);
            BlankLinePaddingHelper.InsertPaddingAfterEndRegionTags(textDocument);

            BlankLinePaddingHelper.InsertPaddingBeforeCodeElements(usingStatementsThatStartBlocks);
            BlankLinePaddingHelper.InsertPaddingAfterCodeElements(usingStatementsThatEndBlocks);

            BlankLinePaddingHelper.InsertPaddingBeforeCodeElements(namespaces);
            BlankLinePaddingHelper.InsertPaddingAfterCodeElements(namespaces);

            BlankLinePaddingHelper.InsertPaddingBeforeCodeElements(classes);
            BlankLinePaddingHelper.InsertPaddingAfterCodeElements(classes);

            BlankLinePaddingHelper.InsertPaddingBeforeCodeElements(delegates);
            BlankLinePaddingHelper.InsertPaddingAfterCodeElements(delegates);

            BlankLinePaddingHelper.InsertPaddingBeforeCodeElements(enumerations);
            BlankLinePaddingHelper.InsertPaddingAfterCodeElements(enumerations);

            BlankLinePaddingHelper.InsertPaddingBeforeCodeElements(events);
            BlankLinePaddingHelper.InsertPaddingAfterCodeElements(events);

            BlankLinePaddingHelper.InsertPaddingBeforeCodeElements(fieldsWithComments);
            BlankLinePaddingHelper.InsertPaddingAfterCodeElements(fieldsWithComments);

            BlankLinePaddingHelper.InsertPaddingBeforeCodeElements(interfaces);
            BlankLinePaddingHelper.InsertPaddingAfterCodeElements(interfaces);

            BlankLinePaddingHelper.InsertPaddingBeforeCodeElements(methods);
            BlankLinePaddingHelper.InsertPaddingAfterCodeElements(methods);

            BlankLinePaddingHelper.InsertPaddingBeforeCodeElements(properties);
            BlankLinePaddingHelper.InsertPaddingBetweenMultiLinePropertyAccessors(properties);
            BlankLinePaddingHelper.InsertPaddingAfterCodeElements(properties);

            BlankLinePaddingHelper.InsertPaddingBeforeCodeElements(structs);
            BlankLinePaddingHelper.InsertPaddingAfterCodeElements(structs);

            // Perform insertion of explicit access modifier cleanup.
            ExplicitAccessModifierHelper.InsertExplicitAccessModifiersOnClasses(classes);
            ExplicitAccessModifierHelper.InsertExplicitAccessModifiersOnDelegates(delegates);
            ExplicitAccessModifierHelper.InsertExplicitAccessModifiersOnEnumerations(enumerations);
            ExplicitAccessModifierHelper.InsertExplicitAccessModifiersOnEvents(events);
            ExplicitAccessModifierHelper.InsertExplicitAccessModifiersOnFields(fields);
            ExplicitAccessModifierHelper.InsertExplicitAccessModifiersOnInterfaces(interfaces);
            ExplicitAccessModifierHelper.InsertExplicitAccessModifiersOnMethods(methods);
            ExplicitAccessModifierHelper.InsertExplicitAccessModifiersOnProperties(properties);
            ExplicitAccessModifierHelper.InsertExplicitAccessModifiersOnStructs(structs);

            // Perform update cleanup.
            UpdateEndRegionDirectives(textDocument);
        }

        /// <summary>
        /// Attempts to run code cleanup on the specified C/C++ document.
        /// </summary>
        /// <param name="document">The document for cleanup.</param>
        /// <param name="isAutoSave">A flag indicating if occurring due to auto-save.</param>
        private void RunCodeCleanupC(Document document, bool isAutoSave)
        {
            var textDocument = (TextDocument)document.Object("TextDocument");

            RunVSFormatting(textDocument);

            RemoveEOLWhitespace(textDocument);
            RemoveBlankLinesAtTop(textDocument);
            RemoveBlankLinesAtBottom(textDocument);
            RemoveBlankLinesAfterOpeningBrace(textDocument);
            RemoveBlankLinesBeforeClosingBrace(textDocument);
            RemoveMultipleConsecutiveBlankLines(textDocument);
        }

        /// <summary>
        /// Attempts to run code cleanup on the specified generic document.
        /// </summary>
        /// <param name="document">The document for cleanup.</param>
        /// <param name="isAutoSave">A flag indicating if occurring due to auto-save.</param>
        private void RunCodeCleanupGeneric(Document document, bool isAutoSave)
        {
            var textDocument = (TextDocument)document.Object("TextDocument");

            RunVSFormatting(textDocument);

            RemoveEOLWhitespace(textDocument);
            RemoveBlankLinesAtTop(textDocument);
            RemoveBlankLinesAtBottom(textDocument);
            RemoveMultipleConsecutiveBlankLines(textDocument);
        }

        #endregion Private Language Methods

        #region Private Cleanup Methods

        /// <summary>
        /// Run the visual studio built-in format document command.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        private void RunVSFormatting(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RunVisualStudioFormatDocumentCommand) return;

            try
            {
                using (new CursorPositionRestorer(textDocument))
                {
                    // Run the command.
                    Package.IDE.ExecuteCommand("Edit.FormatDocument", String.Empty);
                }
            }
            catch
            {
                // OK if fails, not available for some file types.
            }
        }

        /// <summary>
        /// Removes blank lines from the bottom of the specified text document.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        private void RemoveBlankLinesAtBottom(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RemoveBlankLinesAtBottom) return;

            EditPoint cursor = textDocument.EndPoint.CreateEditPoint();
            cursor.DeleteWhitespace(vsWhitespaceOptions.vsWhitespaceOptionsVertical);

            // The last blank line may not have been removed, perform the delete more explicitly.
            if (cursor.AtEndOfDocument && cursor.AtStartOfLine && cursor.AtEndOfLine)
            {
                var backCursor = cursor.CreateEditPoint();
                backCursor.CharLeft();
                backCursor.Delete(cursor);
            }
        }

        /// <summary>
        /// Removes blank lines from the top of the specified text document.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        private void RemoveBlankLinesAtTop(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RemoveBlankLinesAtTop) return;

            EditPoint cursor = textDocument.StartPoint.CreateEditPoint();
            cursor.DeleteWhitespace(vsWhitespaceOptions.vsWhitespaceOptionsVertical);
        }

        /// <summary>
        /// Removes blank lines after attributes.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        private void RemoveBlankLinesAfterAttributes(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RemoveBlankLinesAfterAttributes) return;

            string pattern = Package.UsePOSIXRegEx
                                 ? @"\]{:b*(//.*)*}\n\n"
                                 : @"\]([^\r\n]*)(\r?\n){2,}";
            string replacement = Package.UsePOSIXRegEx
                                     ? @"\]\1" + Environment.NewLine
                                     : @"]$1" + Environment.NewLine;
            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replacement);
        }

        /// <summary>
        /// Removes blank lines after an opening brace.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        private void RemoveBlankLinesAfterOpeningBrace(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RemoveBlankLinesAfterOpeningBrace) return;

            string pattern = Package.UsePOSIXRegEx
                                 ? @"\{{:b*(//.*)*}\n\n"
                                 : @"\{([ \t]*(//[^\r\n]*)*)(\r?\n){2,}";
            string replacement = Package.UsePOSIXRegEx
                                     ? @"\{\1" + Environment.NewLine
                                     : @"{$1" + Environment.NewLine;
            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replacement);
        }

        /// <summary>
        /// Removes blank lines before a closing brace.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        private void RemoveBlankLinesBeforeClosingBrace(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RemoveBlankLinesBeforeClosingBrace) return;

            string pattern = Package.UsePOSIXRegEx
                                 ? @"\n\n{:b*}\}"
                                 : @"(\r?\n){2,}([ \t]*)\}";
            string replacement = Package.UsePOSIXRegEx
                                     ? Environment.NewLine + @"\1\}"
                                     : Environment.NewLine + @"$2}";

            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replacement);
        }

        /// <summary>
        /// Removes all end of line whitespace from the specified text document.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        private void RemoveEOLWhitespace(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RemoveEndOfLineWhitespace) return;

            string pattern = Package.UsePOSIXRegEx ? @":b+\n" : @"[ \t]+\r?\n";
            string replacement = Environment.NewLine;

            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replacement);
        }

        /// <summary>
        /// Removes multiple consecutive blank lines from the specified text document.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        private void RemoveMultipleConsecutiveBlankLines(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RemoveMultipleConsecutiveBlankLines) return;

            string pattern = Package.UsePOSIXRegEx ? @"\n\n\n+" : @"(\r?\n){3,}";
            string replacement = Environment.NewLine + Environment.NewLine;

            TextDocumentHelper.SubstituteAllStringMatches(textDocument, pattern, replacement);
        }

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
        private void UpdateEndRegionDirectives(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_UpdateEndRegionDirectives) return;

            var regionStack = new Stack<string>();
            EditPoint cursor = textDocument.StartPoint.CreateEditPoint();
            TextRanges subGroupMatches = null; // Not used - required for FindPattern.
            string pattern = Package.UsePOSIXRegEx ? @"^:b*\#" : @"^[ \t]*#";
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

        #endregion Private Cleanup Methods

        #region Private Helper Methods

        /// <summary>
        /// Gets the specified code items as unique blocks by consecutive line positioning.
        /// </summary>
        /// <typeparam name="T">The type of the code item.</typeparam>
        /// <param name="codeItems">The code items.</param>
        /// <returns>An enumerable collection of blocks of code items.</returns>
        private static IEnumerable<IList<T>> GetCodeItemBlocks<T>(IEnumerable<T> codeItems)
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

        #endregion Private Helper Methods

        #region Private Properties

        /// <summary>
        /// Gets or sets the blank line padding helper.
        /// </summary>
        private BlankLinePaddingHelper BlankLinePaddingHelper { get; set; }

        /// <summary>
        /// Gets or sets the code cleanup availability helper.
        /// </summary>
        private CodeCleanupAvailabilityHelper CodeCleanupAvailabilityHelper { get; set; }

        /// <summary>
        /// Gets or sets the explicit access modifier helper.
        /// </summary>
        private ExplicitAccessModifierHelper ExplicitAccessModifierHelper { get; set; }

        /// <summary>
        /// Gets or sets the hosting package.
        /// </summary>
        private CodeMaidPackage Package { get; set; }

        /// <summary>
        /// Gets the lazy-initialized undo transaction helper.
        /// </summary>
        private UndoTransactionHelper UndoTransactionHelper
        {
            get { return _undoTransactionHelper ?? (_undoTransactionHelper = new UndoTransactionHelper(Package, "CodeMaid Cleanup")); }
        }

        /// <summary>
        /// Gets or sets the using statement cleanup helper.
        /// </summary>
        private UsingStatementCleanupHelper UsingStatementCleanupHelper { get; set; }

        #endregion Private Properties
    }
}