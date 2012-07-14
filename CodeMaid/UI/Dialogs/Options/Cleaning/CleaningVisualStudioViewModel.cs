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

using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.Cleaning
{
    /// <summary>
    /// The view model for cleaning visual studio options.
    /// </summary>
    public class CleaningVisualStudioViewModel : OptionsPageViewModel
    {
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
            Settings.Default.Cleaning_UsingStatementsToReinsertWhenRemovedExpression = UsingStatementsToReinsertWhenRemovedExpression;
        }

        #endregion Overrides of OptionsPageViewModel

        #region Options

        private bool _runVisualStudioFormatDocument;

        /// <summary>
        /// Gets or sets the flag indicating if the Visual Studio format document command should be run.
        /// </summary>
        public bool RunVisualStudioFormatDocument
        {
            get { return _runVisualStudioFormatDocument; }
            set
            {
                if (_runVisualStudioFormatDocument != value)
                {
                    _runVisualStudioFormatDocument = value;
                    NotifyPropertyChanged("RunVisualStudioFormatDocument");
                }
            }
        }

        private bool _runVisualStudioRemoveUnusedUsingStatements;

        /// <summary>
        /// Gets or sets the flag indicating if unused using statements should be removed.
        /// </summary>
        public bool RunVisualStudioRemoveUnusedUsingStatements
        {
            get { return _runVisualStudioRemoveUnusedUsingStatements; }
            set
            {
                if (_runVisualStudioRemoveUnusedUsingStatements != value)
                {
                    _runVisualStudioRemoveUnusedUsingStatements = value;
                    NotifyPropertyChanged("RunVisualStudioRemoveUnusedUsingStatements");
                }
            }
        }

        private bool _runVisualStudioSortUsingStatements;

        /// <summary>
        /// Gets or sets the flag indicating if using statements should be sorted.
        /// </summary>
        public bool RunVisualStudioSortUsingStatements
        {
            get { return _runVisualStudioSortUsingStatements; }
            set
            {
                if (_runVisualStudioSortUsingStatements != value)
                {
                    _runVisualStudioSortUsingStatements = value;
                    NotifyPropertyChanged("RunVisualStudioSortUsingStatements");
                }
            }
        }

        private bool _skipRemoveUnusedUsingStatementsDuringAutoCleanupOnSave;

        /// <summary>
        /// Gets or sets the flag indicating if unused using statements should be removed except during auto cleanup on save.
        /// </summary>
        public bool SkipRemoveUnusedUsingStatementsDuringAutoCleanupOnSave
        {
            get { return _skipRemoveUnusedUsingStatementsDuringAutoCleanupOnSave; }
            set
            {
                if (_skipRemoveUnusedUsingStatementsDuringAutoCleanupOnSave != value)
                {
                    _skipRemoveUnusedUsingStatementsDuringAutoCleanupOnSave = value;
                    NotifyPropertyChanged("SkipRemoveUnusedUsingStatementsDuringAutoCleanupOnSave");
                }
            }
        }

        private string _usingStatementsToReinsertWhenRemovedExpression;

        /// <summary>
        /// Gets or sets the expression for using statements that should be re-inserted when removed.
        /// </summary>
        public string UsingStatementsToReinsertWhenRemovedExpression
        {
            get { return _usingStatementsToReinsertWhenRemovedExpression; }
            set
            {
                if (_usingStatementsToReinsertWhenRemovedExpression != value)
                {
                    _usingStatementsToReinsertWhenRemovedExpression = value;
                    NotifyPropertyChanged("UsingStatementsToReinsertWhenRemovedExpression");
                }
            }
        }

        #endregion Options
    }
}