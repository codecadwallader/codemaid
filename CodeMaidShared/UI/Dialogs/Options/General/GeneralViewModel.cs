using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.General
{
    /// <summary>
    /// The view model for general options.
    /// </summary>
    public class GeneralViewModel : OptionsPageViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <param name="activeSettings">The active settings.</param>
        public GeneralViewModel(CodeMaidPackage package, Settings activeSettings)
            : base(package, activeSettings)
        {
            Mappings = new SettingsToOptionsList(ActiveSettings, this)
            {
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.General_CacheFiles, x => CacheFiles),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.General_DiagnosticsMode, x => DiagnosticsMode),
                new SettingToOptionMapping<string, string>(x => ActiveSettings.General_Font, x => Font),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.General_LoadModelsAsynchronously, x => LoadModelsAsynchronously),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.General_ShowStartPageOnSolutionClose, x => ShowStartPageOnSolutionClose),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.General_SkipUndoTransactionsDuringAutoCleanupOnSave, x => SkipUndoTransactionsDuringAutoCleanupOnSave),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.General_UseUndoTransactions, x => UseUndoTransactions)
            };
        }

        #endregion Constructors

        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header => Resources.General;

        #endregion Overrides of OptionsPageViewModel

        #region Options

        /// <summary>
        /// Gets or sets the flag indicating if files should be cached.
        /// </summary>
        public bool CacheFiles
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if diagnostics mode should be enabled.
        /// </summary>
        public bool DiagnosticsMode
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the string representing the font.
        /// </summary>
        public string Font
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if models can be loaded asynchronously.
        /// </summary>
        public bool LoadModelsAsynchronously
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if the start page should be shown when the solution is closed.
        /// </summary>
        public bool ShowStartPageOnSolutionClose
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if undo transactions should not be used during auto
        /// cleanup on save.
        /// </summary>
        public bool SkipUndoTransactionsDuringAutoCleanupOnSave
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets a flag indicating if undo transactions should be utilized.
        /// </summary>
        public bool UseUndoTransactions
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        #endregion Options
    }
}