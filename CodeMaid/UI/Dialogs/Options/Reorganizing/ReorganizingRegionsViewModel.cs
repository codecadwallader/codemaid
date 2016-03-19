using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.Reorganizing
{
    /// <summary>
    /// The view model for reorganizing regions options.
    /// </summary>
    public class ReorganizingRegionsViewModel : OptionsPageViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReorganizingRegionsViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <param name="activeSettings">The active settings.</param>
        public ReorganizingRegionsViewModel(CodeMaidPackage package, Settings activeSettings)
            : base(package, activeSettings)
        {
            Mappings = new SettingsToOptionsList(ActiveSettings, this)
            {
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Reorganizing_RegionsIncludeAccessLevel, x => IncludeAccessLevel),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Reorganizing_RegionsInsertKeepEvenIfEmpty, x => InsertKeepEvenIfEmpty),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Reorganizing_RegionsInsertNewRegions, x => InsertNewRegions),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Reorganizing_RegionsRemoveExistingRegions, x => RemoveExistingRegions)
            };
        }

        #endregion Constructors

        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header => "Regions";

        #endregion Overrides of OptionsPageViewModel

        #region Options

        /// <summary>
        /// Gets or sets the flag indicating if the access level should be included in the regions.
        /// </summary>
        public bool IncludeAccessLevel
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if regions should be inserted or kept even if they would be empty.
        /// </summary>
        public bool InsertKeepEvenIfEmpty
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if new regions should be inserted.
        /// </summary>
        public bool InsertNewRegions
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if the existing regions should be removed.
        /// </summary>
        public bool RemoveExistingRegions
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        #endregion Options
    }
}