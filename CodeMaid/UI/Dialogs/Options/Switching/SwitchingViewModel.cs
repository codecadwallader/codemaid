using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.Switching
{
    /// <summary>
    /// The view model for switching options.
    /// </summary>
    public class SwitchingViewModel : OptionsPageViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchingViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <param name="activeSettings">The active settings.</param>
        public SwitchingViewModel(CodeMaidPackage package, Settings activeSettings)
            : base(package, activeSettings)
        {
            Mappings = new SettingsToOptionsList(ActiveSettings, this)
            {
                new SettingToOptionMapping<string, string>(x => ActiveSettings.Switching_RelatedFileExtensionsExpression, x => RelatedFileExtensionsExpression)
            };
        }

        #endregion Constructors

        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header => "Switching";

        #endregion Overrides of OptionsPageViewModel

        #region Options

        /// <summary>
        /// Gets or sets the expression for related file extensions.
        /// </summary>
        public string RelatedFileExtensionsExpression
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        #endregion Options
    }
}