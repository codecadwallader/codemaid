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
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_RunVisualStudioRemoveAndSortUsingStatements, x => RunVisualStudioRemoveAndSortUsingStatements),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_SkipRemoveAndSortUsingStatementsDuringAutoCleanupOnSave, x => SkipRemoveAndSortUsingStatementsDuringAutoCleanupOnSave),
                new SettingToOptionMapping<string, string>(x => ActiveSettings.Cleaning_UsingStatementsToReinsertWhenRemovedExpression, x => UsingStatementsToReinsertWhenRemovedExpression)
            };
        }

        #endregion Constructors

        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header => "Visual Studio";

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
        /// Gets or sets the flag indicating if using statements should be removed and sorted.
        /// </summary>
        public bool RunVisualStudioRemoveAndSortUsingStatements
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if using statements should not be removed and sorted
        /// during auto cleanup on save.
        /// </summary>
        public bool SkipRemoveAndSortUsingStatementsDuringAutoCleanupOnSave
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