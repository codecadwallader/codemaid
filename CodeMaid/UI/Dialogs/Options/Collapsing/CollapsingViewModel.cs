using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.Collapsing
{
    /// <summary>
    /// The view model for collapsing options.
    /// </summary>
    public class CollapsingViewModel : OptionsPageViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CollapsingViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <param name="activeSettings">The active settings.</param>
        public CollapsingViewModel(CodeMaidPackage package, Settings activeSettings)
            : base(package, activeSettings)
        {
            Mappings = new SettingsToOptionsList(ActiveSettings, this)
            {
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Collapsing_CollapseSolutionWhenOpened, x => CollapseSolutionWhenOpened),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Collapsing_KeepSoloProjectExpanded, x => KeepSoloProjectExpanded)
            };
        }

        #endregion Constructors

        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header => "Collapsing";

        #endregion Overrides of OptionsPageViewModel

        #region Options

        /// <summary>
        /// Gets or sets a flag indicating if the solution should be collapsed when it is opened.
        /// </summary>
        public bool CollapseSolutionWhenOpened
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets a flag indicating if a solo project should be kept expanded.
        /// </summary>
        public bool KeepSoloProjectExpanded
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        #endregion Options
    }
}