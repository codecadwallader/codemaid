using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.Cleaning
{
    /// <summary>
    /// The view model for cleaning file types options.
    /// </summary>
    public class CleaningFileTypesViewModel : OptionsPageViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CleaningFileTypesViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <param name="activeSettings">The active settings.</param>
        public CleaningFileTypesViewModel(CodeMaidPackage package, Settings activeSettings)
            : base(package, activeSettings)
        {
            Mappings = new SettingsToOptionsList(ActiveSettings, this)
            {
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_ExcludeT4GeneratedCode, x => ExcludeT4GeneratedCode),
                new SettingToOptionMapping<string, string>(x => ActiveSettings.Cleaning_ExclusionExpression, x => ExclusionExpression),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_IncludeCPlusPlus, x => IncludeCPlusPlus),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_IncludeCSharp, x => IncludeCSharp),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_IncludeCSS, x => IncludeCSS),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_IncludeEverythingElse, x => IncludeEverythingElse),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_IncludeFSharp, x => IncludeFSharp),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_IncludeHTML, x => IncludeHTML),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_IncludeJavaScript, x => IncludeJavaScript),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_IncludeJSON, x => IncludeJSON),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_IncludeLESS, x => IncludeLESS),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_IncludePHP, x => IncludePHP),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_IncludePowerShell, x => IncludePowerShell),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_IncludeR, x => IncludeR),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_IncludeSCSS, x => IncludeSCSS),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_IncludeTypeScript, x => IncludeTypeScript),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_IncludeVB, x => IncludeVisualBasic),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_IncludeXAML, x => IncludeXAML),
                new SettingToOptionMapping<bool, bool>(x => ActiveSettings.Cleaning_IncludeXML, x => IncludeXML)
            };
        }

        #endregion Constructors

        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header => Resources.CleaningFileTypesViewModel_FileTypes;

        #endregion Overrides of OptionsPageViewModel

        #region Options

        /// <summary>
        /// Gets or sets the flag indicating if T4 generated code files should be excluded.
        /// </summary>
        public bool ExcludeT4GeneratedCode
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the expression for files to exclude.
        /// </summary>
        public string ExclusionExpression
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if C++ files should be included.
        /// </summary>
        public bool IncludeCPlusPlus
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if C# files should be included.
        /// </summary>
        public bool IncludeCSharp
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if CSS files should be included.
        /// </summary>
        public bool IncludeCSS
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if everything else (ex: .txt, README) should be included.
        /// </summary>
        public bool IncludeEverythingElse
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if F# files should be included.
        /// </summary>
        public bool IncludeFSharp
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if HTML files should be included.
        /// </summary>
        public bool IncludeHTML
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if JavaScript files should be included.
        /// </summary>
        public bool IncludeJavaScript
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if JSON files should be included.
        /// </summary>
        public bool IncludeJSON
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if LESS files should be included.
        /// </summary>
        public bool IncludeLESS
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if PHP files should be included.
        /// </summary>
        public bool IncludePHP
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if PowerShell files should be included.
        /// </summary>
        public bool IncludePowerShell
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if R files should be included.
        /// </summary>
        public bool IncludeR
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if SCSS files should be included.
        /// </summary>
        public bool IncludeSCSS
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if TypeScript files should be included.
        /// </summary>
        public bool IncludeTypeScript
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if Visual Basic files should be included.
        /// </summary>
        public bool IncludeVisualBasic
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if XAML files should be included.
        /// </summary>
        public bool IncludeXAML
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if XML files should be included.
        /// </summary>
        public bool IncludeXML
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        #endregion Options
    }
}