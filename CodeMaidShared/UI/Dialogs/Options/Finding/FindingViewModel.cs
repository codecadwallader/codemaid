using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.Finding
{
    /// <summary>
    /// The view model for finding options.
    /// </summary>
    public class FindingViewModel : OptionsPageViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FindingViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <param name="activeSettings">The active settings.</param>
        public FindingViewModel(CodeMaidPackage package, Settings activeSettings)
            : base(package, activeSettings)
        {
            Mappings = new SettingsToOptionsList(ActiveSettings, this)
            {
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Finding_ClearSolutionExplorerSearch, x => ClearSolutionExplorerSearch),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Finding_TemporarilyOpenSolutionFolders, x => TemporarilyOpenSolutionFolders)
            };
        }

        #endregion Constructors

        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header => Resources.FindingViewModel_Finding;

        #endregion Overrides of OptionsPageViewModel

        #region Options

        /// <summary>
        /// Gets or sets a flag indicating if Solution Explorer search should be cleared.
        /// </summary>
        public bool ClearSolutionExplorerSearch
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets a flag indicating if solution folders should be temporarily opened.
        /// </summary>
        public bool TemporarilyOpenSolutionFolders
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        #endregion Options
    }
}