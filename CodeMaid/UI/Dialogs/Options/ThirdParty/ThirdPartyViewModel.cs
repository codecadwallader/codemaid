using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.ThirdParty
{
    /// <summary>
    /// The view model for third party options.
    /// </summary>
    public class ThirdPartyViewModel : OptionsPageViewModel
    {
        #region Fields

        private readonly CommandHelper _commandHelper;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ThirdPartyViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <param name="activeSettings">The active settings.</param>
        public ThirdPartyViewModel(CodeMaidPackage package, Settings activeSettings)
            : base(package, activeSettings)
        {
            Mappings = new SettingsToOptionsList(ActiveSettings, this)
            {
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.ThirdParty_UseJetBrainsReSharperCleanup, x => UseJetBrainsReSharperCleanup),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.ThirdParty_UseTelerikJustCodeCleanup, x => UseTelerikJustCodeCleanup),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.ThirdParty_UseXAMLStylerCleanup, x => UseXAMLStylerCleanup),
                new SettingToOptionMapping<string, string>(x => ActiveSettings.ThirdParty_OtherCleaningCommandsExpression, x => OtherCleaningCommandsExpression)
            };

            _commandHelper = CommandHelper.GetInstance(package);
        }

        #endregion Constructors

        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header => "Third Party";

        #endregion Overrides of OptionsPageViewModel

        #region Options

        /// <summary>
        /// Gets or sets the flag indicating if JetBrains ReSharper cleanup should be utilized during cleanup.
        /// </summary>
        public bool UseJetBrainsReSharperCleanup
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if Telerik JustCode cleanup should be utilized during cleanup.
        /// </summary>
        public bool UseTelerikJustCodeCleanup
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if XAML Styler cleanup should be utilized during cleanup.
        /// </summary>
        public bool UseXAMLStylerCleanup
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the expression for other cleaning commands to be utilized during cleanup.
        /// </summary>
        public string OtherCleaningCommandsExpression
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        #endregion Options

        #region Enables

        /// <summary>
        /// Gets a flag indicating if the UseJetBrainsReSharperCleanup option should be enabled.
        /// </summary>
        public bool IsEnabledUseJetBrainsReSharperCleanup => _commandHelper.FindCommand("ReSharper_SilentCleanupCode") != null || _commandHelper.FindCommand("ReSharper.ReSharper_SilentCleanupCode") != null;

        /// <summary>
        /// Gets a flag indicating if the UseTelerikJustCodeCleanup option should be enabled.
        /// </summary>
        public bool IsEnabledUseTelerikJustCodeCleanup => _commandHelper.FindCommand("JustCode.JustCode_CleanCodeWithDefaultProfile") != null;

        /// <summary>
        /// Gets a flag indicating if the UseXAMLStylerCleanup option should be enabled.
        /// </summary>
        public bool IsEnabledUseXAMLStylerCleanup => _commandHelper.FindCommand("EditorContextMenus.XAMLEditor.BeautifyXaml", "EditorContextMenus.XAMLEditor.FormatXAML") != null;

        #endregion Enables
    }
}