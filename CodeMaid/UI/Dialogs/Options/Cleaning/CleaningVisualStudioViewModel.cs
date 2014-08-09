#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.Cleaning
{
    /// <summary>
    /// The view model for cleaning visual studio options.
    /// </summary>
    public class CleaningVisualStudioViewModel : OptionsPageViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CleaningVisualStudioViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        public CleaningVisualStudioViewModel(CodeMaidPackage package)
            : base(package)
        {
        }

        #endregion Constructors

        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header
        {
            get { return "Visual Studio"; }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            RunVisualStudioFormatDocument = Settings.Default.Cleaning_RunVisualStudioFormatDocumentCommand;
            RunVisualStudioRemoveUnusedUsingStatements = Settings.Default.Cleaning_RunVisualStudioRemoveUnusedUsingStatements;
            RunVisualStudioSortUsingStatements = Settings.Default.Cleaning_RunVisualStudioSortUsingStatements;
            SkipRemoveUnusedUsingStatementsDuringAutoCleanupOnSave = Settings.Default.Cleaning_SkipRemoveUnusedUsingStatementsDuringAutoCleanupOnSave;
            SkipSortUsingStatementsDuringAutoCleanupOnSave = Settings.Default.Cleaning_SkipSortUsingStatementsDuringAutoCleanupOnSave;
            UsingStatementsToReinsertWhenRemovedExpression = Settings.Default.Cleaning_UsingStatementsToReinsertWhenRemovedExpression;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public override void SaveSettings()
        {
            Settings.Default.Cleaning_RunVisualStudioFormatDocumentCommand = RunVisualStudioFormatDocument;
            Settings.Default.Cleaning_RunVisualStudioRemoveUnusedUsingStatements = RunVisualStudioRemoveUnusedUsingStatements;
            Settings.Default.Cleaning_RunVisualStudioSortUsingStatements = RunVisualStudioSortUsingStatements;
            Settings.Default.Cleaning_SkipRemoveUnusedUsingStatementsDuringAutoCleanupOnSave = SkipRemoveUnusedUsingStatementsDuringAutoCleanupOnSave;
            Settings.Default.Cleaning_SkipSortUsingStatementsDuringAutoCleanupOnSave = SkipSortUsingStatementsDuringAutoCleanupOnSave;
            Settings.Default.Cleaning_UsingStatementsToReinsertWhenRemovedExpression = UsingStatementsToReinsertWhenRemovedExpression;
        }

        #endregion Overrides of OptionsPageViewModel

        #region Options

        /// <summary>
        /// Gets or sets the flag indicating if the Visual Studio format document command should be run.
        /// </summary>
        public bool RunVisualStudioFormatDocument
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if unused using statements should be removed.
        /// </summary>
        public bool RunVisualStudioRemoveUnusedUsingStatements
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if using statements should be sorted.
        /// </summary>
        public bool RunVisualStudioSortUsingStatements
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if unused using statements should not be removed during
        /// auto cleanup on save.
        /// </summary>
        public bool SkipRemoveUnusedUsingStatementsDuringAutoCleanupOnSave
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if using statements should not be sorted during auto
        /// cleanup on save.
        /// </summary>
        public bool SkipSortUsingStatementsDuringAutoCleanupOnSave
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the expression for using statements that should be re-inserted when removed.
        /// </summary>
        public string UsingStatementsToReinsertWhenRemovedExpression
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        #endregion Options
    }
}