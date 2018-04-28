using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Logic.Formatting;
using SteveCadwallader.CodeMaid.Logic.Reorganizing;
using SteveCadwallader.CodeMaid.Model;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SteveCadwallader.CodeMaid.Logic.Cleaning
{
    /// <summary>
    /// A manager class for cleaning up code.
    /// </summary>
    /// <remarks>
    ///
    /// Note: All POSIXRegEx text replacements search against '\n' but insert/replace with
    ///       Environment.NewLine. This handles line endings correctly.
    /// </remarks>
    internal class CodeCleanupManager
    {
        #region Fields

        private readonly CodeMaidPackage _package;

        private readonly CodeModelManager _codeModelManager;
        private readonly CodeReorganizationManager _codeReorganizationManager;
        private readonly CommandHelper _commandHelper;

        private readonly CodeCleanupAvailabilityLogic _codeCleanupAvailabilityLogic;
        private readonly CommentFormatLogic _commentFormatLogic;
        private readonly InsertBlankLinePaddingLogic _insertBlankLinePaddingLogic;
        private readonly InsertExplicitAccessModifierLogic _insertExplicitAccessModifierLogic;
        private readonly InsertWhitespaceLogic _insertWhitespaceLogic;
        private readonly FileHeaderLogic _fileHeaderLogic;
        private readonly RemoveRegionLogic _removeRegionLogic;
        private readonly RemoveWhitespaceLogic _removeWhitespaceLogic;
        private readonly UpdateLogic _updateLogic;
        private readonly UsingStatementCleanupLogic _usingStatementCleanupLogic;

        private readonly CachedSettingSet<string> _otherCleaningCommands =
            new CachedSettingSet<string>(() => Settings.Default.ThirdParty_OtherCleaningCommandsExpression,
                                         expression =>
                                         expression.Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries)
                                                   .Select(x => x.Trim())
                                                   .Where(y => !string.IsNullOrEmpty(y))
                                                   .ToList());

        #endregion Fields

        #region Constructors

        /// <summary>
        /// The singleton instance of the <see cref="CodeCleanupManager" /> class.
        /// </summary>
        private static CodeCleanupManager _instance;

        /// <summary>
        /// Gets an instance of the <see cref="CodeCleanupManager" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="CodeCleanupManager" /> class.</returns>
        internal static CodeCleanupManager GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new CodeCleanupManager(package));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeCleanupManager" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private CodeCleanupManager(CodeMaidPackage package)
        {
            _package = package;

            _codeModelManager = CodeModelManager.GetInstance(_package);
            _codeReorganizationManager = CodeReorganizationManager.GetInstance(_package);
            _commandHelper = CommandHelper.GetInstance(_package);

            _codeCleanupAvailabilityLogic = CodeCleanupAvailabilityLogic.GetInstance(_package);
            _commentFormatLogic = CommentFormatLogic.GetInstance(_package);
            _insertBlankLinePaddingLogic = InsertBlankLinePaddingLogic.GetInstance(_package);
            _insertExplicitAccessModifierLogic = InsertExplicitAccessModifierLogic.GetInstance();
            _insertWhitespaceLogic = InsertWhitespaceLogic.GetInstance(_package);
            _fileHeaderLogic = FileHeaderLogic.GetInstance(_package);
            _removeRegionLogic = RemoveRegionLogic.GetInstance(_package);
            _removeWhitespaceLogic = RemoveWhitespaceLogic.GetInstance(_package);
            _updateLogic = UpdateLogic.GetInstance(_package);
            _usingStatementCleanupLogic = UsingStatementCleanupLogic.GetInstance(_package);
        }

        #endregion Constructors

        #region Internal Methods

        /// <summary>
        /// Attempts to run code cleanup on the specified project item.
        /// </summary>
        /// <param name="projectItem">The project item for cleanup.</param>
        internal void Cleanup(ProjectItem projectItem)
        {
            if (!_codeCleanupAvailabilityLogic.CanCleanupProjectItem(projectItem)) return;

            // Attempt to open the document if not already opened.
            bool wasOpen = projectItem.IsOpen[Constants.vsViewKindTextView] || projectItem.IsOpen[Constants.vsViewKindCode];
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
                Cleanup(projectItem.Document);

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
        internal void Cleanup(Document document)
        {
            if (!_codeCleanupAvailabilityLogic.CanCleanupDocument(document, true)) return;

            // Make sure the document to be cleaned up is active, required for some commands like format document.
            document.Activate();

            // Check for designer windows being active, which should not proceed with cleanup as the code isn't truly active.
            if (document.ActiveWindow.Caption.EndsWith(" [Design]"))
            {
                return;
            }

            if (_package.ActiveDocument != document)
            {
                OutputWindowHelper.WarningWriteLine($"Activation was not completed before cleaning began for '{document.Name}'");
            }

            // Conditionally start cleanup with reorganization.
            if (Settings.Default.Reorganizing_RunAtStartOfCleanup)
            {
                _codeReorganizationManager.Reorganize(document);
            }

            new UndoTransactionHelper(_package, string.Format(Resources.CodeMaidCleanupFor0, document.Name)).Run(
                delegate
                {
                    var cleanupMethod = FindCodeCleanupMethod(document);
                    if (cleanupMethod != null)
                    {
                        OutputWindowHelper.DiagnosticWriteLine($"CodeCleanupManager.Cleanup started for '{document.FullName}'");
                        _package.IDE.StatusBar.Text = string.Format(Resources.CodeMaidIsCleaning0, document.Name);

                        // Perform the set of configured cleanups based on the language.
                        cleanupMethod(document);

                        _package.IDE.StatusBar.Text = string.Format(Resources.CodeMaidCleaned0, document.Name);
                        OutputWindowHelper.DiagnosticWriteLine($"CodeCleanupManager.Cleanup completed for '{document.FullName}'");
                    }
                });
        }

        #endregion Internal Methods

        #region Private Language Methods

        /// <summary>
        /// Finds a code cleanup method appropriate for the specified document, otherwise null.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>The code cleanup method, otherwise null.</returns>
        private Action<Document> FindCodeCleanupMethod(Document document)
        {
            switch (document.GetCodeLanguage())
            {
                case CodeLanguage.CSharp:
                    return RunCodeCleanupCSharp;

                case CodeLanguage.VisualBasic:
                    return RunCodeCleanupVB;

                case CodeLanguage.CPlusPlus:
                case CodeLanguage.CSS:
                case CodeLanguage.JavaScript:
                case CodeLanguage.JSON:
                case CodeLanguage.LESS:
                case CodeLanguage.PHP:
                case CodeLanguage.PowerShell:
                case CodeLanguage.R:
                case CodeLanguage.SCSS:
                case CodeLanguage.TypeScript:
                    return RunCodeCleanupC;

                case CodeLanguage.HTML:
                case CodeLanguage.XAML:
                case CodeLanguage.XML:
                    return RunCodeCleanupMarkup;

                case CodeLanguage.FSharp:
                case CodeLanguage.Unknown:
                    return RunCodeCleanupGeneric;

                default:
                    OutputWindowHelper.WarningWriteLine($"FindCodeCleanupMethod does not recognize document language '{document.Language}'");
                    return null;
            }
        }

        /// <summary>
        /// Attempts to run code cleanup on the specified CSharp document.
        /// </summary>
        /// <param name="document">The document for cleanup.</param>
        private void RunCodeCleanupCSharp(Document document)
        {
            var textDocument = document.GetTextDocument();

            // Perform any actions that can modify the file code model first.
            RunExternalFormatting(textDocument);
            if (!document.IsExternal())
            {
                _usingStatementCleanupLogic.RemoveAndSortUsingStatements(textDocument);
            }

            // Interpret the document into a collection of elements.
            var codeItems = _codeModelManager.RetrieveAllCodeItems(document);

            var regions = codeItems.OfType<CodeItemRegion>().ToList();
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
            var usingStatementBlocks = CodeModelHelper.GetCodeItemBlocks(usingStatements).ToList();
            var usingStatementsThatStartBlocks = (from IEnumerable<CodeItemUsingStatement> block in usingStatementBlocks select block.First()).ToList();
            var usingStatementsThatEndBlocks = (from IEnumerable<CodeItemUsingStatement> block in usingStatementBlocks select block.Last()).ToList();

            // Perform file header cleanup.
            _fileHeaderLogic.UpdateFileHeader(textDocument);

            // Perform removal cleanup.
            _removeRegionLogic.RemoveRegionsPerSettings(regions);
            _removeWhitespaceLogic.RemoveEOLWhitespace(textDocument);
            _removeWhitespaceLogic.RemoveBlankLinesAtTop(textDocument);
            _removeWhitespaceLogic.RemoveBlankLinesAtBottom(textDocument);
            _removeWhitespaceLogic.RemoveEOFTrailingNewLine(textDocument);
            _removeWhitespaceLogic.RemoveBlankLinesAfterAttributes(textDocument);
            _removeWhitespaceLogic.RemoveBlankLinesAfterOpeningBrace(textDocument);
            _removeWhitespaceLogic.RemoveBlankLinesBeforeClosingBrace(textDocument);
            _removeWhitespaceLogic.RemoveBlankLinesBetweenChainedStatements(textDocument);
            _removeWhitespaceLogic.RemoveMultipleConsecutiveBlankLines(textDocument);

            // Perform insertion of blank line padding cleanup.
            _insertBlankLinePaddingLogic.InsertPaddingBeforeRegionTags(regions);
            _insertBlankLinePaddingLogic.InsertPaddingAfterRegionTags(regions);

            _insertBlankLinePaddingLogic.InsertPaddingBeforeEndRegionTags(regions);
            _insertBlankLinePaddingLogic.InsertPaddingAfterEndRegionTags(regions);

            _insertBlankLinePaddingLogic.InsertPaddingBeforeCodeElements(usingStatementsThatStartBlocks);
            _insertBlankLinePaddingLogic.InsertPaddingAfterCodeElements(usingStatementsThatEndBlocks);

            _insertBlankLinePaddingLogic.InsertPaddingBeforeCodeElements(namespaces);
            _insertBlankLinePaddingLogic.InsertPaddingAfterCodeElements(namespaces);

            _insertBlankLinePaddingLogic.InsertPaddingBeforeCodeElements(classes);
            _insertBlankLinePaddingLogic.InsertPaddingAfterCodeElements(classes);

            _insertBlankLinePaddingLogic.InsertPaddingBeforeCodeElements(delegates);
            _insertBlankLinePaddingLogic.InsertPaddingAfterCodeElements(delegates);

            _insertBlankLinePaddingLogic.InsertPaddingBeforeCodeElements(enumerations);
            _insertBlankLinePaddingLogic.InsertPaddingAfterCodeElements(enumerations);

            _insertBlankLinePaddingLogic.InsertPaddingBeforeCodeElements(events);
            _insertBlankLinePaddingLogic.InsertPaddingAfterCodeElements(events);

            _insertBlankLinePaddingLogic.InsertPaddingBeforeCodeElements(fields);
            _insertBlankLinePaddingLogic.InsertPaddingAfterCodeElements(fields);

            _insertBlankLinePaddingLogic.InsertPaddingBeforeCodeElements(interfaces);
            _insertBlankLinePaddingLogic.InsertPaddingAfterCodeElements(interfaces);

            _insertBlankLinePaddingLogic.InsertPaddingBeforeCodeElements(methods);
            _insertBlankLinePaddingLogic.InsertPaddingAfterCodeElements(methods);

            _insertBlankLinePaddingLogic.InsertPaddingBeforeCodeElements(properties);
            _insertBlankLinePaddingLogic.InsertPaddingBetweenMultiLinePropertyAccessors(properties);
            _insertBlankLinePaddingLogic.InsertPaddingAfterCodeElements(properties);

            _insertBlankLinePaddingLogic.InsertPaddingBeforeCodeElements(structs);
            _insertBlankLinePaddingLogic.InsertPaddingAfterCodeElements(structs);

            _insertBlankLinePaddingLogic.InsertPaddingBeforeCaseStatements(textDocument);
            _insertBlankLinePaddingLogic.InsertPaddingBeforeSingleLineComments(textDocument);

            // Perform insertion of explicit access modifier cleanup.
            _insertExplicitAccessModifierLogic.InsertExplicitAccessModifiersOnClasses(classes);
            _insertExplicitAccessModifierLogic.InsertExplicitAccessModifiersOnDelegates(delegates);
            _insertExplicitAccessModifierLogic.InsertExplicitAccessModifiersOnEnumerations(enumerations);
            _insertExplicitAccessModifierLogic.InsertExplicitAccessModifiersOnEvents(events);
            _insertExplicitAccessModifierLogic.InsertExplicitAccessModifiersOnFields(fields);
            _insertExplicitAccessModifierLogic.InsertExplicitAccessModifiersOnInterfaces(interfaces);
            _insertExplicitAccessModifierLogic.InsertExplicitAccessModifiersOnMethods(methods);
            _insertExplicitAccessModifierLogic.InsertExplicitAccessModifiersOnProperties(properties);
            _insertExplicitAccessModifierLogic.InsertExplicitAccessModifiersOnStructs(structs);

            // Perform insertion of whitespace cleanup.
            _insertWhitespaceLogic.InsertEOFTrailingNewLine(textDocument);

            // Perform update cleanup.
            _updateLogic.UpdateEndRegionDirectives(textDocument);
            _updateLogic.UpdateEventAccessorsToBothBeSingleLineOrMultiLine(events);
            _updateLogic.UpdatePropertyAccessorsToBothBeSingleLineOrMultiLine(properties);
            _updateLogic.UpdateSingleLineMethods(methods);

            // Perform comment cleaning.
            _commentFormatLogic.FormatComments(textDocument);
        }

        /// <summary>
        /// Attempts to run code cleanup on the specified VB.Net document.
        /// </summary>
        /// <param name="document">The document for cleanup.</param>
        private void RunCodeCleanupVB(Document document)
        {
            var textDocument = document.GetTextDocument();

            // Perform any actions that can modify the file code model first.
            RunExternalFormatting(textDocument);
            if (!document.IsExternal())
            {
                _usingStatementCleanupLogic.RemoveAndSortUsingStatements(textDocument);
            }

            // Interpret the document into a collection of elements.
            var codeItems = _codeModelManager.RetrieveAllCodeItems(document);

            var regions = codeItems.OfType<CodeItemRegion>().ToList();
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
            var usingStatementBlocks = CodeModelHelper.GetCodeItemBlocks(usingStatements).ToList();
            var usingStatementsThatStartBlocks = (from IEnumerable<CodeItemUsingStatement> block in usingStatementBlocks select block.First()).ToList();
            var usingStatementsThatEndBlocks = (from IEnumerable<CodeItemUsingStatement> block in usingStatementBlocks select block.Last()).ToList();

            // Perform file header cleanup.
            _fileHeaderLogic.UpdateFileHeader(textDocument);

            // Perform removal cleanup.
            _removeRegionLogic.RemoveRegionsPerSettings(regions);
            _removeWhitespaceLogic.RemoveEOLWhitespace(textDocument);
            _removeWhitespaceLogic.RemoveBlankLinesAtTop(textDocument);
            _removeWhitespaceLogic.RemoveBlankLinesAtBottom(textDocument);
            _removeWhitespaceLogic.RemoveEOFTrailingNewLine(textDocument);
            _removeWhitespaceLogic.RemoveBlankLinesAfterAttributes(textDocument);
            _removeWhitespaceLogic.RemoveBlankLinesBetweenChainedStatements(textDocument);
            _removeWhitespaceLogic.RemoveMultipleConsecutiveBlankLines(textDocument);

            // Perform insertion of blank line padding cleanup.
            _insertBlankLinePaddingLogic.InsertPaddingBeforeRegionTags(regions);
            _insertBlankLinePaddingLogic.InsertPaddingAfterRegionTags(regions);

            _insertBlankLinePaddingLogic.InsertPaddingBeforeEndRegionTags(regions);
            _insertBlankLinePaddingLogic.InsertPaddingAfterEndRegionTags(regions);

            _insertBlankLinePaddingLogic.InsertPaddingBeforeCodeElements(usingStatementsThatStartBlocks);
            _insertBlankLinePaddingLogic.InsertPaddingAfterCodeElements(usingStatementsThatEndBlocks);

            _insertBlankLinePaddingLogic.InsertPaddingBeforeCodeElements(namespaces);
            _insertBlankLinePaddingLogic.InsertPaddingAfterCodeElements(namespaces);

            _insertBlankLinePaddingLogic.InsertPaddingBeforeCodeElements(classes);
            _insertBlankLinePaddingLogic.InsertPaddingAfterCodeElements(classes);

            _insertBlankLinePaddingLogic.InsertPaddingBeforeCodeElements(delegates);
            _insertBlankLinePaddingLogic.InsertPaddingAfterCodeElements(delegates);

            _insertBlankLinePaddingLogic.InsertPaddingBeforeCodeElements(enumerations);
            _insertBlankLinePaddingLogic.InsertPaddingAfterCodeElements(enumerations);

            _insertBlankLinePaddingLogic.InsertPaddingBeforeCodeElements(events);
            _insertBlankLinePaddingLogic.InsertPaddingAfterCodeElements(events);

            _insertBlankLinePaddingLogic.InsertPaddingBeforeCodeElements(fields);
            _insertBlankLinePaddingLogic.InsertPaddingAfterCodeElements(fields);

            _insertBlankLinePaddingLogic.InsertPaddingBeforeCodeElements(interfaces);
            _insertBlankLinePaddingLogic.InsertPaddingAfterCodeElements(interfaces);

            _insertBlankLinePaddingLogic.InsertPaddingBeforeCodeElements(methods);
            _insertBlankLinePaddingLogic.InsertPaddingAfterCodeElements(methods);

            _insertBlankLinePaddingLogic.InsertPaddingBeforeCodeElements(properties);
            _insertBlankLinePaddingLogic.InsertPaddingBetweenMultiLinePropertyAccessors(properties);
            _insertBlankLinePaddingLogic.InsertPaddingAfterCodeElements(properties);

            _insertBlankLinePaddingLogic.InsertPaddingBeforeCodeElements(structs);
            _insertBlankLinePaddingLogic.InsertPaddingAfterCodeElements(structs);

            _insertBlankLinePaddingLogic.InsertPaddingBeforeCaseStatements(textDocument);
            _insertBlankLinePaddingLogic.InsertPaddingBeforeSingleLineComments(textDocument);

            // Perform insertion of whitespace cleanup.
            _insertWhitespaceLogic.InsertEOFTrailingNewLine(textDocument);

            // Perform comment cleaning.
            _commentFormatLogic.FormatComments(textDocument);
        }

        /// <summary>
        /// Attempts to run code cleanup on the specified C/C++ document.
        /// </summary>
        /// <param name="document">The document for cleanup.</param>
        private void RunCodeCleanupC(Document document)
        {
            var textDocument = document.GetTextDocument();

            RunExternalFormatting(textDocument);

            // Perform file header cleanup.
            _fileHeaderLogic.UpdateFileHeader(textDocument);

            // Perform removal cleanup.
            _removeWhitespaceLogic.RemoveEOLWhitespace(textDocument);
            _removeWhitespaceLogic.RemoveBlankLinesAtTop(textDocument);
            _removeWhitespaceLogic.RemoveBlankLinesAtBottom(textDocument);
            _removeWhitespaceLogic.RemoveEOFTrailingNewLine(textDocument);
            _removeWhitespaceLogic.RemoveBlankLinesAfterOpeningBrace(textDocument);
            _removeWhitespaceLogic.RemoveBlankLinesBeforeClosingBrace(textDocument);
            _removeWhitespaceLogic.RemoveMultipleConsecutiveBlankLines(textDocument);

            // Perform insertion of blank line padding cleanup.
            _insertBlankLinePaddingLogic.InsertPaddingBeforeSingleLineComments(textDocument);

            // Perform insertion of whitespace cleanup.
            _insertWhitespaceLogic.InsertEOFTrailingNewLine(textDocument);
        }

        /// <summary>
        /// Attempts to run code cleanup on the specified markup document.
        /// </summary>
        /// <param name="document">The document for cleanup.</param>
        private void RunCodeCleanupMarkup(Document document)
        {
            var textDocument = document.GetTextDocument();

            RunExternalFormatting(textDocument);

            // Perform file header cleanup.
            _fileHeaderLogic.UpdateFileHeader(textDocument);

            // Perform removal cleanup.
            _removeWhitespaceLogic.RemoveEOLWhitespace(textDocument);
            _removeWhitespaceLogic.RemoveBlankLinesAtTop(textDocument);
            _removeWhitespaceLogic.RemoveBlankLinesAtBottom(textDocument);
            _removeWhitespaceLogic.RemoveEOFTrailingNewLine(textDocument);
            _removeWhitespaceLogic.RemoveBlankLinesBeforeClosingTag(textDocument);
            _removeWhitespaceLogic.RemoveBlankSpacesBeforeClosingAngleBracket(textDocument);
            _removeWhitespaceLogic.RemoveMultipleConsecutiveBlankLines(textDocument);

            // Perform insertion cleanup.
            _insertWhitespaceLogic.InsertBlankSpaceBeforeSelfClosingAngleBracket(textDocument);
            _insertWhitespaceLogic.InsertEOFTrailingNewLine(textDocument);
        }

        /// <summary>
        /// Attempts to run code cleanup on the specified generic document.
        /// </summary>
        /// <param name="document">The document for cleanup.</param>
        private void RunCodeCleanupGeneric(Document document)
        {
            var textDocument = document.GetTextDocument();

            RunExternalFormatting(textDocument);

            // Perform file header cleanup.
            _fileHeaderLogic.UpdateFileHeader(textDocument);

            // Perform removal cleanup.
            _removeWhitespaceLogic.RemoveEOLWhitespace(textDocument);
            _removeWhitespaceLogic.RemoveBlankLinesAtTop(textDocument);
            _removeWhitespaceLogic.RemoveBlankLinesAtBottom(textDocument);
            _removeWhitespaceLogic.RemoveEOFTrailingNewLine(textDocument);
            _removeWhitespaceLogic.RemoveMultipleConsecutiveBlankLines(textDocument);

            // Perform insertion cleanup.
            _insertWhitespaceLogic.InsertEOFTrailingNewLine(textDocument);
        }

        #endregion Private Language Methods

        #region Private Cleanup Methods

        /// <summary>
        /// Runs external formatting tools (e.g. Visual Studio, JetBrains ReSharper, Telerik JustCode).
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        private void RunExternalFormatting(TextDocument textDocument)
        {
            RunVisualStudioFormatDocument(textDocument);
            RunJetBrainsReSharperCleanup(textDocument);
            RunTelerikJustCodeCleanup(textDocument);
            RunXAMLStylerCleanup(textDocument);
            RunOtherCleanupCommands(textDocument);
        }

        /// <summary>
        /// Runs the Visual Studio built-in format document command.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        private void RunVisualStudioFormatDocument(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RunVisualStudioFormatDocumentCommand) return;

            _commandHelper.ExecuteCommand(textDocument, "Edit.FormatDocument");
        }

        /// <summary>
        /// Runs the JetBrains ReSharper cleanup command.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        private void RunJetBrainsReSharperCleanup(TextDocument textDocument)
        {
            if (!Settings.Default.ThirdParty_UseJetBrainsReSharperCleanup) return;

            // This command changed to include the leading 'ReSharper.' in version 2016.1.
            // Execute both commands for backwards compatibility.
            _commandHelper.ExecuteCommand(textDocument, "ReSharper_SilentCleanupCode", "ReSharper.ReSharper_SilentCleanupCode");
        }

        /// <summary>
        /// Runs the Telerik JustCode cleanup command.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        private void RunTelerikJustCodeCleanup(TextDocument textDocument)
        {
            if (!Settings.Default.ThirdParty_UseTelerikJustCodeCleanup) return;

            _commandHelper.ExecuteCommand(textDocument, "JustCode.JustCode_CleanCodeWithDefaultProfile");
        }

        /// <summary>
        /// Runs the XAML Styler cleanup command.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        private void RunXAMLStylerCleanup(TextDocument textDocument)
        {
            if (!Settings.Default.ThirdParty_UseXAMLStylerCleanup) return;

            _commandHelper.ExecuteCommand(textDocument, "EditorContextMenus.XAMLEditor.BeautifyXaml", "EditorContextMenus.XAMLEditor.FormatXAML", "EditorContextMenus.CodeWindow.FormatXAML");
        }

        /// <summary>
        /// Runs the other cleanup commands.
        /// </summary>
        /// <param name="textDocument">The text document to cleanup.</param>
        private void RunOtherCleanupCommands(TextDocument textDocument)
        {
            if (!_otherCleaningCommands.Value.Any()) return;

            foreach (var commandName in _otherCleaningCommands.Value)
            {
                _commandHelper.ExecuteCommand(textDocument, commandName);
            }
        }

        #endregion Private Cleanup Methods
    }
}