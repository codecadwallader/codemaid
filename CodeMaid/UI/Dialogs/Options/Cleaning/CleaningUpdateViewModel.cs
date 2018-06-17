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

        #region Binding properties for variable header management

        /// <summary>
        /// the selection start index of the C# Textbox
        /// </summary>
        private int textSelectionStartCSharp;

        /// <summary>
        /// Get or sets the selection start index of the C# Textbox
        /// </summary>
        public int TextSelectionStartCSharp
        {
            get { return textSelectionStartCSharp; }
            set
            {
                if (value != textSelectionStartCSharp)
                {
                    textSelectionStartCSharp = value;
                    RaisePropertyChanged(nameof(TextSelectionStartCSharp));
                }
            }
        }

        /// <summary>
        /// the selection start index of the C++ Textbox
        /// </summary>
        private int textSelectionStartCPlusPlus;

        /// <summary>
        /// Get or sets the selection start index of the C++ Textbox
        /// </summary>
        public int TextSelectionStartCPlusPlus
        {
            get { return textSelectionStartCPlusPlus; }
            set
            {
                if (value != textSelectionStartCPlusPlus)
                {
                    textSelectionStartCPlusPlus = value;
                    RaisePropertyChanged(nameof(TextSelectionStartCPlusPlus));
                }
            }
        }

        /// <summary>
        /// the selection start index of the FSharp Textbox
        /// </summary>
        private int textSelectionStartFSharp;

        /// <summary>
        /// Get or sets the selection start index of the FSharp Textbox
        /// </summary>
        public int TextSelectionStartFSharp
        {
            get { return textSelectionStartFSharp; }
            set
            {
                if (value != textSelectionStartFSharp)
                {
                    textSelectionStartFSharp = value;
                    RaisePropertyChanged(nameof(TextSelectionStartFSharp));
                }
            }
        }

        /// <summary>
        /// the selection start index of the VB Textbox
        /// </summary>
        private int textSelectionStartVisualBasic;

        /// <summary>
        /// Get or sets the selection start index of the VB Textbox
        /// </summary>
        public int TextSelectionStartVisualBasic
        {
            get { return textSelectionStartVisualBasic; }
            set
            {
                if (value != textSelectionStartVisualBasic)
                {
                    textSelectionStartVisualBasic = value;
                    RaisePropertyChanged(nameof(TextSelectionStartVisualBasic));
                }
            }
        }

        /// <summary>
        /// the selection start index of the PHP Textbox
        /// </summary>
        private int textSelectionStartPHP;

        /// <summary>
        /// Get or sets the selection start index of the PHP Textbox
        /// </summary>
        public int TextSelectionStartPHP
        {
            get { return textSelectionStartPHP; }
            set
            {
                if (value != textSelectionStartPHP)
                {
                    textSelectionStartPHP = value;
                    RaisePropertyChanged(nameof(TextSelectionStartPHP));
                }
            }
        }

        /// <summary>
        /// The selection start index of the PowerShell Textbox
        /// </summary>
        private int textSelectionStartPowerShell;

        /// <summary>
        /// Get or sets the selection start index of the PowerShell Textbox
        /// </summary>
        public int TextSelectionStartPowerShell
        {
            get { return textSelectionStartPowerShell; }
            set
            {
                if (value != textSelectionStartPowerShell)
                {
                    textSelectionStartPowerShell = value;
                    RaisePropertyChanged(nameof(TextSelectionStartPowerShell));
                }
            }
        }

        /// <summary>
        /// The selection start index of the R Textbox
        /// </summary>
        private int textSelectionStartR;

        /// <summary>
        /// Get or sets the selection start index of the R Textbox
        /// </summary>
        public int TextSelectionStartR
        {
            get { return textSelectionStartR; }
            set
            {
                if (value != textSelectionStartR)
                {
                    textSelectionStartR = value;
                    RaisePropertyChanged(nameof(TextSelectionStartR));
                }
            }
        }

        /// <summary>
        /// the selection start index of the JSON Textbox
        /// </summary>
        private int textSelectionStartJSON;

        /// <summary>
        /// Get or sets the selection start index of the JSON Textbox
        /// </summary>
        public int TextSelectionStartJSON
        {
            get { return textSelectionStartJSON; }
            set
            {
                if (value != textSelectionStartJSON)
                {
                    textSelectionStartJSON = value;
                    RaisePropertyChanged(nameof(TextSelectionStartJSON));
                }
            }
        }

        /// <summary>
        /// The selection start index of the XAML Textbox
        /// </summary>
        private int textSelectionStartXAML;

        /// <summary>
        /// Get or sets the selection start index of the XAML Textbox
        /// </summary>
        public int TextSelectionStartXAML
        {
            get { return textSelectionStartXAML; }
            set
            {
                if (value != textSelectionStartXAML)
                {
                    textSelectionStartXAML = value;
                    RaisePropertyChanged(nameof(TextSelectionStartXAML));
                }
            }
        }

        /// <summary>
        /// the selection start index of the XML Textbox
        /// </summary>
        private int textSelectionStartXML;

        /// <summary>
        /// Get or sets the selection start index of the XML Textbox
        /// </summary>
        public int TextSelectionStartXML
        {
            get { return textSelectionStartXML; }
            set
            {
                if (value != textSelectionStartXML)
                {
                    textSelectionStartXML = value;
                    RaisePropertyChanged(nameof(TextSelectionStartXML));
                }
            }
        }

        /// <summary>
        /// The selection start index of the HTML Textbox
        /// </summary>
        private int textSelectionStartHTML;

        /// <summary>
        /// Get or sets the selection start index of the HTML Textbox
        /// </summary>
        public int TextSelectionStartHTML
        {
            get { return textSelectionStartHTML; }
            set
            {
                if (value != textSelectionStartHTML)
                {
                    textSelectionStartHTML = value;
                    RaisePropertyChanged(nameof(TextSelectionStartHTML));
                }
            }
        }

        /// <summary>
        /// the selection start index of the CSS Textbox
        /// </summary>
        private int textSelectionStartCSS;

        /// <summary>
        /// Get or sets the selection start index of the CSS Textbox
        /// </summary>
        public int TextSelectionStartCSS
        {
            get { return textSelectionStartCSS; }
            set
            {
                if (value != textSelectionStartCSS)
                {
                    textSelectionStartCSS = value;
                    RaisePropertyChanged(nameof(TextSelectionStartCSS));
                }
            }
        }

        /// <summary>
        /// The selection start index of the LESS Textbox
        /// </summary>
        private int textSelectionStartLESS;

        /// <summary>
        /// Get or sets the selection start index of the LESS Textbox
        /// </summary>
        public int TextSelectionStartLESS
        {
            get { return textSelectionStartLESS; }
            set
            {
                if (value != textSelectionStartLESS)
                {
                    textSelectionStartLESS = value;
                    RaisePropertyChanged(nameof(TextSelectionStartLESS));
                }
            }
        }

        /// <summary>
        /// the selection start index of the SCSS Textbox
        /// </summary>
        private int textSelectionStartSCSS;

        /// <summary>
        /// Get or sets the selection start index of the SCSS Textbox
        /// </summary>
        public int TextSelectionStartSCSS
        {
            get { return textSelectionStartSCSS; }
            set
            {
                if (value != textSelectionStartSCSS)
                {
                    textSelectionStartSCSS = value;
                    RaisePropertyChanged(nameof(TextSelectionStartSCSS));
                }
            }
        }

        /// <summary>
        /// the selection start index of the Javascript Textbox
        /// </summary>
        private int textSelectionStartJavaScript;

        /// <summary>
        /// Get or sets the selection start index of the Javascript Textbox
        /// </summary>
        public int TextSelectionStartJavaScript
        {
            get { return textSelectionStartJavaScript; }
            set
            {
                if (value != textSelectionStartJavaScript)
                {
                    textSelectionStartJavaScript = value;
                    RaisePropertyChanged(nameof(TextSelectionStartJavaScript));
                }
            }
        }

        /// <summary>
        /// the selection start index of the TypeScript Textbox
        /// </summary>
        private int textSelectionStartTypeScript;

        /// <summary>
        /// Get or sets the selection start index of the TypeScript Textbox
        /// </summary>
        public int TextSelectionStartTypeScript
        {
            get { return textSelectionStartTypeScript; }
            set
            {
                if (value != textSelectionStartTypeScript)
                {
                    textSelectionStartTypeScript = value;
                    RaisePropertyChanged(nameof(TextSelectionStartTypeScript));
                }
            }
        }

        /// <summary>
        /// the select tab index
        /// </summary>
        private int selectedTabIndex;

        /// <summary>
        /// Get or sets the selected tab index
        /// </summary>
        public int SelectedTabIndex
        {
            get { return selectedTabIndex; }
            set
            {
                if (value != selectedTabIndex)
                {
                    selectedTabIndex = value;
                    RaisePropertyChanged(nameof(SelectedTabIndex));
                }
            }
        }

        #endregion Binding properties for variable header management

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

        #region AddHeaderVariable Command

        private DelegateCommand _addHeaderVariableCommand;

        /// <summary>
        /// Gets the set dialog result command.
        /// </summary>
        public DelegateCommand AddHeaderVariableCommand => _addHeaderVariableCommand ?? (_addHeaderVariableCommand = new DelegateCommand(OnAddVaribleCommandExecuted));

        /// <summary>
        /// Called when the <see cref="AddHeaderVariableCommand" /> is executed.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void OnAddVaribleCommandExecuted(object parameter)
        {
            var selectedTab = selectedTabIndex;
            var currentSelectedHeaderText = GetSelectedHeaderText();
            var currentStartSelectionIndex = GetSelectionStartIndex();

            var variableText = parameter as string;

            if (variableText == null || currentSelectedHeaderText.Length < currentStartSelectionIndex)
            {
                return;
            }

            var previousSelectionStart = currentStartSelectionIndex;
            var newHeaderText = currentSelectedHeaderText.Insert(currentStartSelectionIndex, variableText);

            SetSelectedHeaderText(newHeaderText);
            SetSelectionStartIndex(previousSelectionStart + variableText.Length);
        }

        /// <summary>
        /// Get the selected text header
        /// </summary>
        /// <returns>The text header from the active tab</returns>
        private string GetSelectedHeaderText()
        {
            switch (selectedTabIndex)
            {
                case 0:
                    {
                        return UpdateFileHeaderCSharp;
                    }
                case 1:
                    {
                        return UpdateFileHeaderCPlusPlus;
                    }
                case 2:
                    {
                        return UpdateFileHeaderFSharp;
                    }
                case 3:
                    {
                        return UpdateFileHeaderVisualBasic;
                    }
                case 4:
                    {
                        return UpdateFileHeaderPHP;
                    }
                case 5:
                    {
                        return UpdateFileHeaderPowerShell;
                    }
                case 6:
                    {
                        return UpdateFileHeaderR;
                    }
                case 7:
                    {
                        return UpdateFileHeaderJSON;
                    }
                case 8:
                    {
                        return UpdateFileHeaderXAML;
                    }
                case 9:
                    {
                        return UpdateFileHeaderXML;
                    }
                case 10:
                    {
                        return UpdateFileHeaderHTML;
                    }
                case 11:
                    {
                        return UpdateFileHeaderCSS;
                    }
                case 12:
                    {
                        return UpdateFileHeaderLESS;
                    }
                case 13:
                    {
                        return UpdateFileHeaderSCSS;
                    }
                case 14:
                    {
                        return UpdateFileHeaderJavaScript;
                    }
                case 15:
                    {
                        return UpdateFileHeaderTypeScript;
                    }
                default:
                    {
                        return string.Empty;
                    }
            }
        }

        /// <summary>
        /// Set the text on the active tab text box
        /// </summary>
        /// <param name="headerText">the header text</param>
        private void SetSelectedHeaderText(string headerText)
        {
            switch (selectedTabIndex)
            {
                case 0:
                    {
                        UpdateFileHeaderCSharp = headerText;
                        break;
                    }
                case 1:
                    {
                        UpdateFileHeaderCPlusPlus = headerText;
                        break;
                    }
                case 2:
                    {
                        UpdateFileHeaderFSharp = headerText;
                        break;
                    }
                case 3:
                    {
                        UpdateFileHeaderVisualBasic = headerText;
                        break;
                    }
                case 4:
                    {
                        UpdateFileHeaderPHP = headerText;
                        break;
                    }
                case 5:
                    {
                        UpdateFileHeaderPowerShell = headerText;
                        break;
                    }
                case 6:
                    {
                        UpdateFileHeaderR = headerText;
                        break;
                    }
                case 7:
                    {
                        UpdateFileHeaderJSON = headerText;
                        break;
                    }
                case 8:
                    {
                        UpdateFileHeaderXAML = headerText;
                        break;
                    }
                case 9:
                    {
                        UpdateFileHeaderXML = headerText;
                        break;
                    }
                case 10:
                    {
                        UpdateFileHeaderHTML = headerText;
                        break;
                    }
                case 11:
                    {
                        UpdateFileHeaderCSS = headerText;
                        break;
                    }
                case 12:
                    {
                        UpdateFileHeaderLESS = headerText;
                        break;
                    }
                case 13:
                    {
                        UpdateFileHeaderSCSS = headerText;
                        break;
                    }
                case 14:
                    {
                        UpdateFileHeaderJavaScript = headerText;
                        break;
                    }
                case 15:
                    {
                        UpdateFileHeaderTypeScript = headerText;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        /// <summary>
        /// Get the selection start index of the active tab header textbox
        /// </summary>
        /// <returns>the selection start index</returns>
        private int GetSelectionStartIndex()
        {
            switch (selectedTabIndex)
            {
                case 0:
                    {
                        return TextSelectionStartCSharp;
                    }
                case 1:
                    {
                        return TextSelectionStartCPlusPlus;
                    }
                case 2:
                    {
                        return TextSelectionStartFSharp;
                    }
                case 3:
                    {
                        return TextSelectionStartVisualBasic;
                    }
                case 4:
                    {
                        return TextSelectionStartPHP;
                    }
                case 5:
                    {
                        return TextSelectionStartPowerShell;
                    }
                case 6:
                    {
                        return TextSelectionStartR;
                    }
                case 7:
                    {
                        return TextSelectionStartJSON;
                    }
                case 8:
                    {
                        return TextSelectionStartXAML;
                    }
                case 9:
                    {
                        return TextSelectionStartXML;
                    }
                case 10:
                    {
                        return TextSelectionStartHTML;
                    }
                case 11:
                    {
                        return TextSelectionStartCSS;
                    }
                case 12:
                    {
                        return TextSelectionStartLESS;
                    }
                case 13:
                    {
                        return TextSelectionStartSCSS;
                    }
                case 14:
                    {
                        return TextSelectionStartJavaScript;
                    }
                case 15:
                    {
                        return TextSelectionStartTypeScript;
                    }
                default:
                    {
                        return default(int);
                    }
            }
        }

        /// <summary>
        /// Set the selection start index for the active tab header textbox
        /// </summary>
        /// <param name="startIndex"></param>
        private void SetSelectionStartIndex(int startIndex)
        {
            switch (selectedTabIndex)
            {
                case 0:
                    {
                        TextSelectionStartCSharp = startIndex;
                        break;
                    }
                case 1:
                    {
                        TextSelectionStartCPlusPlus = startIndex;
                        break;
                    }
                case 2:
                    {
                        TextSelectionStartFSharp = startIndex;
                        break;
                    }
                case 3:
                    {
                        TextSelectionStartVisualBasic = startIndex;
                        break;
                    }
                case 4:
                    {
                        TextSelectionStartPHP = startIndex;
                        break;
                    }
                case 5:
                    {
                        TextSelectionStartPowerShell = startIndex;
                        break;
                    }
                case 6:
                    {
                        TextSelectionStartR = startIndex;
                        break;
                    }
                case 7:
                    {
                        TextSelectionStartJSON = startIndex;
                        break;
                    }
                case 8:
                    {
                        TextSelectionStartXAML = startIndex;
                        break;
                    }
                case 9:
                    {
                        TextSelectionStartXML = startIndex;
                        break;
                    }
                case 10:
                    {
                        TextSelectionStartHTML = startIndex;
                        break;
                    }
                case 11:
                    {
                        TextSelectionStartCSS = startIndex;
                        break;
                    }
                case 12:
                    {
                        TextSelectionStartLESS = startIndex;
                        break;
                    }
                case 13:
                    {
                        TextSelectionStartSCSS = startIndex;
                        break;
                    }
                case 14:
                    {
                        TextSelectionStartJavaScript = startIndex;
                        break;
                    }
                case 15:
                    {
                        TextSelectionStartTypeScript = startIndex;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        #endregion AddHeaderVariable Command
    }
}