#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using SteveCadwallader.CodeMaid.Properties;
using SteveCadwallader.CodeMaid.UI.Enumerations;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.Cleaning
{
    /// <summary>
    /// The view model for cleaning general options.
    /// </summary>
    public class CleaningGeneralViewModel : OptionsPageViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CleaningGeneralViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        public CleaningGeneralViewModel(CodeMaidPackage package)
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
            get { return "General"; }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            AutoCleanupOnFileSave = Settings.Default.Cleaning_AutoCleanupOnFileSave;
            AutoSaveAndCloseIfOpenedByCleanup = Settings.Default.Cleaning_AutoSaveAndCloseIfOpenedByCleanup;
            PerformPartialCleanupOnExternal = (AskYesNo)Settings.Default.Cleaning_PerformPartialCleanupOnExternal;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public override void SaveSettings()
        {
            Settings.Default.Cleaning_AutoCleanupOnFileSave = AutoCleanupOnFileSave;
            Settings.Default.Cleaning_AutoSaveAndCloseIfOpenedByCleanup = AutoSaveAndCloseIfOpenedByCleanup;
            Settings.Default.Cleaning_PerformPartialCleanupOnExternal = (int)PerformPartialCleanupOnExternal;
        }

        #endregion Overrides of OptionsPageViewModel

        #region Options

        /// <summary>
        /// Gets or sets the flag indicating if cleanup should run automatically on file save.
        /// </summary>
        public bool AutoCleanupOnFileSave
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if files should be automatically saved and closed if
        /// opened by cleanup.
        /// </summary>
        public bool AutoSaveAndCloseIfOpenedByCleanup
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the options for performing partial cleanup on external files.
        /// </summary>
        public AskYesNo PerformPartialCleanupOnExternal
        {
            get { return GetPropertyValue<AskYesNo>(); }
            set { SetPropertyValue(value); }
        }

        #endregion Options
    }
}