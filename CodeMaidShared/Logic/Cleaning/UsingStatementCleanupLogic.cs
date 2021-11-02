using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using System;
using System.Linq;

namespace SteveCadwallader.CodeMaid.Logic.Cleaning
{
    /// <summary>
    /// A class for encapsulating using statement cleanup logic.
    /// </summary>
    internal class UsingStatementCleanupLogic
    {
        #region Fields

        private readonly CodeMaidPackage _package;
        private readonly CommandHelper _commandHelper;

        private readonly CachedSettingSet<string> _usingStatementsToReinsertWhenRemoved =
            new CachedSettingSet<string>(() => Settings.Default.Cleaning_UsingStatementsToReinsertWhenRemovedExpression,
                                         expression =>
                                         expression.Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries)
                                                   .Select(x => x.Trim())
                                                   .Where(y => !string.IsNullOrEmpty(y))
                                                   .ToList());

        #endregion Fields

        #region Constructors

        /// <summary>
        /// The singleton instance of the <see cref="UsingStatementCleanupLogic" /> class.
        /// </summary>
        private static UsingStatementCleanupLogic _instance;

        /// <summary>
        /// Gets an instance of the <see cref="UsingStatementCleanupLogic" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="UsingStatementCleanupLogic" /> class.</returns>
        internal static UsingStatementCleanupLogic GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new UsingStatementCleanupLogic(package));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsingStatementCleanupLogic" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private UsingStatementCleanupLogic(CodeMaidPackage package)
        {
            _package = package;

            _commandHelper = CommandHelper.GetInstance(_package);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Run the visual studio built-in remove and sort using statements command.
        /// </summary>
        /// <remarks>
        /// Before VS2017 these were two separate commands.  Starting in VS2017 they were merged into one.
        /// </remarks>
        /// <param name="textDocument">The text document to update.</param>
        public void RemoveAndSortUsingStatements(TextDocument textDocument)
        {
            if (!Settings.Default.Cleaning_RunVisualStudioRemoveAndSortUsingStatements) return;
            if (_package.IsAutoSaveContext && Settings.Default.Cleaning_SkipRemoveAndSortUsingStatementsDuringAutoCleanupOnSave) return;

            // Capture all existing using statements that should be re-inserted if removed.
            const string patternFormat = @"^[ \t]*{0}[ \t]*\r?\n";

            var usingStatementsToReinsert = _usingStatementsToReinsertWhenRemoved.Value
                .Where(usingStatement => TextDocumentHelper.FirstOrDefaultMatch(textDocument, string.Format(patternFormat, usingStatement)) != null)
                .ToList();

            _commandHelper.ExecuteCommand(textDocument, "EditorContextMenus.CodeWindow.RemoveAndSort");

            // Ignore any using statements that are still referenced
            usingStatementsToReinsert = usingStatementsToReinsert
                 .Where(usingStatement => TextDocumentHelper.FirstOrDefaultMatch(textDocument, string.Format(patternFormat, usingStatement)) == null)
                 .ToList();

            if (usingStatementsToReinsert.Count > 0)
            {
                var point = textDocument.StartPoint.CreateEditPoint();

                foreach (string usingStatement in usingStatementsToReinsert)
                {
                    point.StartOfLine();
                    point.Insert(usingStatement);
                    point.Insert(Environment.NewLine);
                }

                // Now sort without removing to ensure correct ordering.
                _commandHelper.ExecuteCommand(textDocument, "Edit.SortUsings");
            }
        }

        #endregion Methods
    }
}