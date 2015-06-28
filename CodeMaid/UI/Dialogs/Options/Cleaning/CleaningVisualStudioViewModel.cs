#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

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
        /// <param name="activeSettings">The active settings.</param>
        public CleaningVisualStudioViewModel(CodeMaidPackage package, Settings activeSettings)
            : base(package, activeSettings)
        {
            Mappings = new SettingsToOptionsList(ActiveSettings, this)
            {
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_RunVisualStudioFormatDocumentCommand, x => RunVisualStudioFormatDocument),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_RunVisualStudioRemoveUnusedUsingStatements, x => RunVisualStudioRemoveUnusedUsingStatements),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_RunVisualStudioSortUsingStatements, x => RunVisualStudioSortUsingStatements),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_SkipRemoveUnusedUsingStatementsDuringAutoCleanupOnSave, x => SkipRemoveUnusedUsingStatementsDuringAutoCleanupOnSave),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_SkipSortUsingStatementsDuringAutoCleanupOnSave, x => SkipSortUsingStatementsDuringAutoCleanupOnSave),
                new SettingToOptionMapping<string, string>(x => ActiveSettings.Cleaning_UsingStatementsToReinsertWhenRemovedExpression, x => UsingStatementsToReinsertWhenRemovedExpression)
            };
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