#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

using SteveCadwallader.CodeMaid.Properties;
using SteveCadwallader.CodeMaid.UI.Enumerations;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.General
{
    /// <summary>
    /// The view model for general options.
    /// </summary>
    public class GeneralViewModel : OptionsPageViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        public GeneralViewModel(CodeMaidPackage package)
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
            CacheFiles = Settings.Default.General_CacheFiles;
            DiagnosticsMode = Settings.Default.General_DiagnosticsMode;
            Font = Settings.Default.General_Font;
            IconSetMode = (IconSetMode)Settings.Default.General_IconSet;
            LoadModelsAsynchronously = Settings.Default.General_LoadModelsAsynchronously;
            Multithread = Settings.Default.General_Multithread;
            ShowStartPageOnSolutionClose = Settings.Default.General_ShowStartPageOnSolutionClose;
            SkipUndoTransactionsDuringAutoCleanupOnSave = Settings.Default.General_SkipUndoTransactionsDuringAutoCleanupOnSave;
            ThemeMode = (ThemeMode)Settings.Default.General_Theme;
            UseUndoTransactions = Settings.Default.General_UseUndoTransactions;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public override void SaveSettings()
        {
            Settings.Default.General_CacheFiles = CacheFiles;
            Settings.Default.General_DiagnosticsMode = DiagnosticsMode;
            Settings.Default.General_Font = Font;
            Settings.Default.General_IconSet = (int)IconSetMode;
            Settings.Default.General_LoadModelsAsynchronously = LoadModelsAsynchronously;
            Settings.Default.General_Multithread = Multithread;
            Settings.Default.General_ShowStartPageOnSolutionClose = ShowStartPageOnSolutionClose;
            Settings.Default.General_SkipUndoTransactionsDuringAutoCleanupOnSave = SkipUndoTransactionsDuringAutoCleanupOnSave;
            Settings.Default.General_Theme = (int)ThemeMode;
            Settings.Default.General_UseUndoTransactions = UseUndoTransactions;
        }

        #endregion Overrides of OptionsPageViewModel

        #region Options

        /// <summary>
        /// Gets or sets the flag indicating if files should be cached.
        /// </summary>
        public bool CacheFiles
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if diagnostics mode should be enabled.
        /// </summary>
        public bool DiagnosticsMode
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the string representing the font.
        /// </summary>
        public string Font
        {
            get { return GetPropertyValue<string>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets which icon set should be utilized.
        /// </summary>
        public IconSetMode IconSetMode
        {
            get { return GetPropertyValue<IconSetMode>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if models can be loaded asynchronously.
        /// </summary>
        public bool LoadModelsAsynchronously
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if multithreading should be utilized.
        /// </summary>
        public bool Multithread
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if the start page should be shown when the solution is closed.
        /// </summary>
        public bool ShowStartPageOnSolutionClose
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if undo transactions should not be used during auto
        /// cleanup on save.
        /// </summary>
        public bool SkipUndoTransactionsDuringAutoCleanupOnSave
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the current theme.
        /// </summary>
        public ThemeMode ThemeMode
        {
            get { return GetPropertyValue<ThemeMode>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets a flag indicating if undo transactions should be utilized.
        /// </summary>
        public bool UseUndoTransactions
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        #endregion Options
    }
}