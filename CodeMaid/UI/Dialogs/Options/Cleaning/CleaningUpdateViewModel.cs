using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.Cleaning
{
    /// <summary>
    /// The view model for cleaning update options.
    /// </summary>
    public class CleaningUpdateViewModel : OptionsPageViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CleaningUpdateViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <param name="activeSettings">The active settings.</param>
        public CleaningUpdateViewModel(CodeMaidPackage package, Settings activeSettings)
            : base(package, activeSettings)
        {
            Mappings = new SettingsToOptionsList(ActiveSettings, this)
            {
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_UpdateAccessorsToBothBeSingleLineOrMultiLine, x => UpdateAccessorsToBothBeSingleLineOrMultiLine),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_UpdateEndRegionDirectives, x => UpdateEndRegionDirectives),
                new SettingToOptionMapping<string, string>(x => ActiveSettings.Cleaning_UpdateFileHeaderCPlusPlus, x => UpdateFileHeaderCPlusPlus),
                new SettingToOptionMapping<string, string>(x => ActiveSettings.Cleaning_UpdateFileHeaderCSharp, x => UpdateFileHeaderCSharp),
                new SettingToOptionMapping<string, string>(x => ActiveSettings.Cleaning_UpdateFileHeaderCSS, x => UpdateFileHeaderCSS),
                new SettingToOptionMapping<string, string>(x => ActiveSettings.Cleaning_UpdateFileHeaderFSharp, x => UpdateFileHeaderFSharp),
                new SettingToOptionMapping<string, string>(x => ActiveSettings.Cleaning_UpdateFileHeaderHTML, x => UpdateFileHeaderHTML),
                new SettingToOptionMapping<string, string>(x => ActiveSettings.Cleaning_UpdateFileHeaderJavaScript, x => UpdateFileHeaderJavaScript),
                new SettingToOptionMapping<string, string>(x => ActiveSettings.Cleaning_UpdateFileHeaderJSON, x => UpdateFileHeaderJSON),
                new SettingToOptionMapping<string, string>(x => ActiveSettings.Cleaning_UpdateFileHeaderLESS, x => UpdateFileHeaderLESS),
                new SettingToOptionMapping<string, string>(x => ActiveSettings.Cleaning_UpdateFileHeaderPHP, x => UpdateFileHeaderPHP),
                new SettingToOptionMapping<string, string>(x => ActiveSettings.Cleaning_UpdateFileHeaderPowerShell, x => UpdateFileHeaderPowerShell),
                new SettingToOptionMapping<string, string>(x => ActiveSettings.Cleaning_UpdateFileHeaderR, x => UpdateFileHeaderR),
                new SettingToOptionMapping<string, string>(x => ActiveSettings.Cleaning_UpdateFileHeaderSCSS, x => UpdateFileHeaderSCSS),
                new SettingToOptionMapping<string, string>(x => ActiveSettings.Cleaning_UpdateFileHeaderTypeScript, x => UpdateFileHeaderTypeScript),
                new SettingToOptionMapping<string, string>(x => ActiveSettings.Cleaning_UpdateFileHeaderVB, x => UpdateFileHeaderVisualBasic),
                new SettingToOptionMapping<string, string>(x => ActiveSettings.Cleaning_UpdateFileHeaderXAML, x => UpdateFileHeaderXAML),
                new SettingToOptionMapping<string, string>(x => ActiveSettings.Cleaning_UpdateFileHeaderXML, x => UpdateFileHeaderXML),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_UpdateSingleLineMethods, x => UpdateSingleLineMethods)
            };
        }

        #endregion Constructors

        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header => Resources.CleaningUpdateViewModel_Update;

        #endregion Overrides of OptionsPageViewModel

        #region Options

        /// <summary>
        /// Gets or sets the flag indicating if accessors should be updated to both be single line
        /// or multi line.
        /// </summary>
        public bool UpdateAccessorsToBothBeSingleLineOrMultiLine
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if end region directives should be updated.
        /// </summary>
        public bool UpdateEndRegionDirectives
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the file header that should be at the top of C++ files.
        /// </summary>
        public string UpdateFileHeaderCPlusPlus
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the file header that should be at the top of C# files.
        /// </summary>
        public string UpdateFileHeaderCSharp
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the file header that should be at the top of CSS files.
        /// </summary>
        public string UpdateFileHeaderCSS
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the file header that should be at the top of F# files.
        /// </summary>
        public string UpdateFileHeaderFSharp
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the file header that should be at the top of HTML files.
        /// </summary>
        public string UpdateFileHeaderHTML
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the file header that should be at the top of JavaScript files.
        /// </summary>
        public string UpdateFileHeaderJavaScript
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the file header that should be at the top of JSON files.
        /// </summary>
        public string UpdateFileHeaderJSON
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the file header that should be at the top of LESS files.
        /// </summary>
        public string UpdateFileHeaderLESS
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the file header that should be at the top of PHP files.
        /// </summary>
        public string UpdateFileHeaderPHP
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the file header that should be at the top of PowerShell files.
        /// </summary>
        public string UpdateFileHeaderPowerShell
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the file header that should be at the top of R files.
        /// </summary>
        public string UpdateFileHeaderR
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the file header that should be at the top of SCSS files.
        /// </summary>
        public string UpdateFileHeaderSCSS
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the file header that should be at the top of TypeScript files.
        /// </summary>
        public string UpdateFileHeaderTypeScript
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the file header that should be at the top of VB files.
        /// </summary>
        public string UpdateFileHeaderVisualBasic
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the file header that should be at the top of XAML files.
        /// </summary>
        public string UpdateFileHeaderXAML
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the file header that should be at the top of XML files.
        /// </summary>
        public string UpdateFileHeaderXML
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if single line methods should be updated.
        /// </summary>
        public bool UpdateSingleLineMethods
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        #endregion Options
    }
}