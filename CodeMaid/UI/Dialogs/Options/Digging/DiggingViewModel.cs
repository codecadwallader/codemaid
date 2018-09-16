using SteveCadwallader.CodeMaid.Model.CodeTree;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.Digging
{
    /// <summary>
    /// The view model for digging options.
    /// </summary>
    public class DiggingViewModel : OptionsPageViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DiggingViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <param name="activeSettings">The active settings.</param>
        public DiggingViewModel(CodeMaidPackage package, Settings activeSettings)
            : base(package, activeSettings)
        {
            Mappings = new SettingsToOptionsList(ActiveSettings, this)
            {
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Digging_CenterOnWhole, x => CenterOnWhole),
                new SettingToOptionMapping<int, int>(x => ActiveSettings.Digging_ComplexityAlertThreshold, x => ComplexityAlertThreshold),
                new SettingToOptionMapping<int, int>(x => ActiveSettings.Digging_ComplexityWarningThreshold, x => ComplexityWarningThreshold),
                new SettingToOptionMapping<int, int>(x => ActiveSettings.Digging_IndentationMargin, x => IndentationMargin),
                new SettingToOptionMapping<int, CodeSortOrder>(x => ActiveSettings.Digging_PrimarySortOrder, x => PrimarySortOrder),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Digging_SecondarySortTypeByName, x => SecondarySortTypeByName),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Digging_ShowItemComplexity, x => ShowItemComplexity),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Digging_ShowItemMetadata, x => ShowItemMetadata),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Digging_ShowItemTypes, x => ShowItemTypes),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Digging_ShowMethodParameters, x => ShowMethodParameters),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Digging_SynchronizeOutlining, x => SynchronizeOutlining)
            };
        }

        #endregion Constructors

        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header => Resources.DiggingViewModel_DiggingSpade;

        #endregion Overrides of OptionsPageViewModel

        #region Options

        /// <summary>
        /// Gets or sets the flag indicating if the view should center on the whole item upon navigation.
        /// </summary>
        public bool CenterOnWhole
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the complexity alert threshold.
        /// </summary>
        public int ComplexityAlertThreshold
        {
            get { return GetPropertyValue<int>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the complexity warning threshold.
        /// </summary>
        public int ComplexityWarningThreshold
        {
            get { return GetPropertyValue<int>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the indentation margin.
        /// </summary>
        public int IndentationMargin
        {
            get { return GetPropertyValue<int>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the primary sort order.
        /// </summary>
        public CodeSortOrder PrimarySortOrder
        {
            get { return GetPropertyValue<CodeSortOrder>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if secondary sorting during type sort should be on name.
        /// </summary>
        public bool SecondarySortTypeByName
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if item complexity should be shown.
        /// </summary>
        public bool ShowItemComplexity
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if item metadata should be shown.
        /// </summary>
        public bool ShowItemMetadata
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if item types should be shown.
        /// </summary>
        public bool ShowItemTypes
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if method parameters should be shown.
        /// </summary>
        public bool ShowMethodParameters
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if outlining should be synchronized with the code file.
        /// </summary>
        public bool SynchronizeOutlining
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        #endregion Options
    }
}