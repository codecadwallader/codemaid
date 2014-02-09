#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using System;
using System.Linq;
using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.Logic.Cleaning
{
    /// <summary>
    /// A class for encapsulating using statement cleanup logic.
    /// </summary>
    internal class UsingStatementCleanupLogic
    {
        #region Fields

        private readonly CodeMaidPackage _package;

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
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Run the visual studio built-in remove unused using statements command.
        /// </summary>
        /// <param name="textDocument">The text document to update.</param>
        /// <param name="isAutoSave">A flag indicating if occurring due to auto-save.</param>
        public void RemoveUnusedUsingStatements(TextDocument textDocument, bool isAutoSave)
        {
            if (!Settings.Default.Cleaning_RunVisualStudioRemoveUnusedUsingStatements) return;
            if (isAutoSave && Settings.Default.Cleaning_SkipRemoveUnusedUsingStatementsDuringAutoCleanupOnSave) return;

            // Capture all existing using statements that should be re-inserted if removed.
            string patternFormat = _package.UsePOSIXRegEx
                                       ? @"^:b*{0}:b*\n"
                                       : @"^[ \t]*{0}[ \t]*\r?\n";

            var points = (from usingStatement in _usingStatementsToReinsertWhenRemoved.Value
                          from editPoint in TextDocumentHelper.FindMatches(textDocument, string.Format(patternFormat, usingStatement))
                          select new { editPoint, text = editPoint.GetLine() }).Reverse().ToList();

            _package.IDE.ExecuteCommand("Edit.RemoveUnusedUsings", String.Empty);

            // Check each using statement point and re-insert it if removed.
            foreach (var point in points)
            {
                string text = point.editPoint.GetLine();
                if (text != point.text)
                {
                    point.editPoint.StartOfLine();
                    point.editPoint.Insert(point.text);
                    point.editPoint.Insert(Environment.NewLine);
                }
            }
        }

        /// <summary>
        /// Run the visual studio built-in sort using statements command.
        /// </summary>
        /// <param name="isAutoSave">A flag indicating if occurring due to auto-save.</param>
        public void SortUsingStatements(bool isAutoSave)
        {
            if (!Settings.Default.Cleaning_RunVisualStudioSortUsingStatements) return;
            if (isAutoSave && Settings.Default.Cleaning_SkipSortUsingStatementsDuringAutoCleanupOnSave) return;

            _package.IDE.ExecuteCommand("Edit.SortUsings", String.Empty);
        }

        #endregion Methods
    }
}