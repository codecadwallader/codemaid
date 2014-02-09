#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using SteveCadwallader.CodeMaid.Properties;

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
            Settings.Default.General_Multithread = Multithread;
            Settings.Default.General_ShowStartPageOnSolutionClose = ShowStartPageOnSolutionClose;
            Settings.Default.General_SkipUndoTransactionsDuringAutoCleanupOnSave = SkipUndoTransactionsDuringAutoCleanupOnSave;
            Settings.Default.General_Theme = (int)ThemeMode;
            Settings.Default.General_UseUndoTransactions = UseUndoTransactions;
        }

        #endregion Overrides of OptionsPageViewModel

        #region Options

        private bool _cacheFiles;

        /// <summary>
        /// Gets or sets the flag indicating if files should be cached.
        /// </summary>
        public bool CacheFiles
        {
            get { return _cacheFiles; }
            set
            {
                if (_cacheFiles != value)
                {
                    _cacheFiles = value;
                    NotifyPropertyChanged("CacheFiles");
                }
            }
        }

        private bool _diagnosticsMode;

        /// <summary>
        /// Gets or sets the flag indicating if diagnostics mode should be enabled.
        /// </summary>
        public bool DiagnosticsMode
        {
            get { return _diagnosticsMode; }
            set
            {
                if (_diagnosticsMode != value)
                {
                    _diagnosticsMode = value;
                    NotifyPropertyChanged("DiagnosticsMode");
                }
            }
        }

        private string _font;

        /// <summary>
        /// Gets or sets the string representing the font.
        /// </summary>
        public string Font
        {
            get { return _font; }
            set
            {
                if (_font != value)
                {
                    _font = value;
                    NotifyPropertyChanged("Font");
                }
            }
        }

        private IconSetMode _iconSetMode;

        /// <summary>
        /// Gets or sets which icon set should be utilized.
        /// </summary>
        public IconSetMode IconSetMode
        {
            get { return _iconSetMode; }
            set
            {
                if (_iconSetMode != value)
                {
                    _iconSetMode = value;
                    NotifyPropertyChanged("IconSetMode");
                }
            }
        }

        private bool _multithread;

        /// <summary>
        /// Gets or sets the flag indicating if multithreading should be utilized.
        /// </summary>
        public bool Multithread
        {
            get { return _multithread; }
            set
            {
                if (_multithread != value)
                {
                    _multithread = value;
                    NotifyPropertyChanged("Multithread");
                }
            }
        }

        private bool _showStartPageOnSolutionClose;

        /// <summary>
        /// Gets or sets the flag indicating if the start page should be shown when the solution is closed.
        /// </summary>
        public bool ShowStartPageOnSolutionClose
        {
            get { return _showStartPageOnSolutionClose; }
            set
            {
                if (_showStartPageOnSolutionClose != value)
                {
                    _showStartPageOnSolutionClose = value;
                    NotifyPropertyChanged("ShowStartPageOnSolutionClose");
                }
            }
        }

        private bool _skipUndoTransactionsDuringAutoCleanupOnSave;

        /// <summary>
        /// Gets or sets the flag indicating if undo transactions should not be used during auto
        /// cleanup on save.
        /// </summary>
        public bool SkipUndoTransactionsDuringAutoCleanupOnSave
        {
            get { return _skipUndoTransactionsDuringAutoCleanupOnSave; }
            set
            {
                if (_skipUndoTransactionsDuringAutoCleanupOnSave != value)
                {
                    _skipUndoTransactionsDuringAutoCleanupOnSave = value;
                    NotifyPropertyChanged("SkipUndoTransactionsDuringAutoCleanupOnSave");
                }
            }
        }

        private ThemeMode _themeMode;

        /// <summary>
        /// Gets or sets the current theme.
        /// </summary>
        public ThemeMode ThemeMode
        {
            get { return _themeMode; }
            set
            {
                if (_themeMode != value)
                {
                    _themeMode = value;
                    NotifyPropertyChanged("ThemeMode");
                }
            }
        }

        private bool _useUndoTransactions;

        /// <summary>
        /// Gets or sets a flag indicating if undo transactions should be utilized.
        /// </summary>
        public bool UseUndoTransactions
        {
            get { return _useUndoTransactions; }
            set
            {
                if (_useUndoTransactions != value)
                {
                    _useUndoTransactions = value;
                    NotifyPropertyChanged("UseUndoTransactions");
                }
            }
        }

        #endregion Options
    }
}