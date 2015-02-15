#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

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
        public CleaningFileTypesViewModel(CodeMaidPackage package)
            : base(package)
        {
        }

        #endregion Constructors

        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header
        {
            get { return "File Types"; }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            ExcludeT4GeneratedCode = Settings.Default.Cleaning_ExcludeT4GeneratedCode;
            ExclusionExpression = Settings.Default.Cleaning_ExclusionExpression;
            IncludeCPlusPlus = Settings.Default.Cleaning_IncludeCPlusPlus;
            IncludeCSharp = Settings.Default.Cleaning_IncludeCSharp;
            IncludeCSS = Settings.Default.Cleaning_IncludeCSS;
            IncludeFSharp = Settings.Default.Cleaning_IncludeFSharp;
            IncludeHTML = Settings.Default.Cleaning_IncludeHTML;
            IncludeJavaScript = Settings.Default.Cleaning_IncludeJavaScript;
            IncludeJSON = Settings.Default.Cleaning_IncludeJSON;
            IncludeLESS = Settings.Default.Cleaning_IncludeLESS;
            IncludePHP = Settings.Default.Cleaning_IncludePHP;
            IncludeSCSS = Settings.Default.Cleaning_IncludeSCSS;
            IncludeTypeScript = Settings.Default.Cleaning_IncludeTypeScript;
            IncludeVisualBasic = Settings.Default.Cleaning_IncludeVB;
            IncludeXAML = Settings.Default.Cleaning_IncludeXAML;
            IncludeXML = Settings.Default.Cleaning_IncludeXML;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public override void SaveSettings()
        {
            Settings.Default.Cleaning_ExcludeT4GeneratedCode = ExcludeT4GeneratedCode;
            Settings.Default.Cleaning_ExclusionExpression = ExclusionExpression;
            Settings.Default.Cleaning_IncludeCPlusPlus = IncludeCPlusPlus;
            Settings.Default.Cleaning_IncludeCSharp = IncludeCSharp;
            Settings.Default.Cleaning_IncludeCSS = IncludeCSS;
            Settings.Default.Cleaning_IncludeFSharp = IncludeFSharp;
            Settings.Default.Cleaning_IncludeHTML = IncludeHTML;
            Settings.Default.Cleaning_IncludeJavaScript = IncludeJavaScript;
            Settings.Default.Cleaning_IncludeJSON = IncludeJSON;
            Settings.Default.Cleaning_IncludeLESS = IncludeLESS;
            Settings.Default.Cleaning_IncludePHP = IncludePHP;
            Settings.Default.Cleaning_IncludeSCSS = IncludeSCSS;
            Settings.Default.Cleaning_IncludeTypeScript = IncludeTypeScript;
            Settings.Default.Cleaning_IncludeVB = IncludeVisualBasic;
            Settings.Default.Cleaning_IncludeXAML = IncludeXAML;
            Settings.Default.Cleaning_IncludeXML = IncludeXML;
        }

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