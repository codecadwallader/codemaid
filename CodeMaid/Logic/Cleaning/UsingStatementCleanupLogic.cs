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

        /// <summary>
        /// A cached setting set container for accessing the using statements to reinsert when removed.
        /// </summary>
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
        /// The singleton instance of the <see cref="UsingStatementCleanupLogic"/> class.
        /// </summary>
        private static UsingStatementCleanupLogic _instance;

        /// <summary>
        /// Gets an instance of the <see cref="UsingStatementCleanupLogic"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="UsingStatementCleanupLogic"/> class.</returns>
        internal static UsingStatementCleanupLogic GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new UsingStatementCleanupLogic(package));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsingStatementCleanupLogic"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private UsingStatementCleanupLogic(CodeMaidPackage package)
        {
            Package = package;
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
            string patternFormat = Package.UsePOSIXRegEx
                                       ? @"^:b*{0}:b*\n"
                                       : @"^[ \t]*{0}[ \t]*\r?\n";

            var points = (from usingStatement in _usingStatementsToReinsertWhenRemoved.Value
                          from editPoint in TextDocumentHelper.FindMatches(textDocument, string.Format(patternFormat, usingStatement))
                          select new { editPoint, text = editPoint.GetLine() }).ToList();

            Package.IDE.ExecuteCommand("Edit.RemoveUnusedUsings", String.Empty);

            // Check each using statement point and re-insert it if removed.
            foreach (var point in points)
            {
                string text = point.editPoint.GetLine();
                if (text != point.text)
                {
                    point.editPoint.Insert(point.text);
                    point.editPoint.Insert(Environment.NewLine);
                }
            }
        }

        /// <summary>
        /// Run the visual studio built-in sort using statements command.
        /// </summary>
        public void SortUsingStatements()
        {
            if (!Settings.Default.Cleaning_RunVisualStudioSortUsingStatements) return;

            Package.IDE.ExecuteCommand("Edit.SortUsings", String.Empty);
        }

        #endregion Methods

        #region Private Properties

        /// <summary>
        /// Gets or sets the hosting package.
        /// </summary>
        private CodeMaidPackage Package { get; set; }

        #endregion Private Properties
    }
}