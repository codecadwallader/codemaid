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
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A class for encapsulating using statement cleanup behavior.
    /// </summary>
    internal class UsingStatementCleanupHelper
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
        /// The singleton instance of the <see cref="UsingStatementCleanupHelper"/> class.
        /// </summary>
        private static UsingStatementCleanupHelper _instance;

        /// <summary>
        /// Gets an instance of the <see cref="UsingStatementCleanupHelper"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="UsingStatementCleanupHelper"/> class.</returns>
        internal static UsingStatementCleanupHelper GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new UsingStatementCleanupHelper(package));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsingStatementCleanupHelper"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private UsingStatementCleanupHelper(CodeMaidPackage package)
        {
            Package = package;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Run the visual studio built-in remove unused using statements command.
        /// </summary>
        /// <param name="isAutoSave">A flag indicating if occurring due to auto-save.</param>
        public void RemoveUnusedUsingStatements(bool isAutoSave)
        {
            if (!Settings.Default.Cleaning_RunVisualStudioRemoveUnusedUsingStatements) return;
            if (isAutoSave && Settings.Default.Cleaning_SkipRemoveUnusedUsingStatementsDuringAutoCleanupOnSave) return;

            Package.IDE.ExecuteCommand("Edit.RemoveUnusedUsings", String.Empty);
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