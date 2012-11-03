#region CodeMaid is Copyright 2007-2012 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2012 Steve Cadwallader.

using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.Digging
{
    /// <summary>
    /// The view model for digging options.
    /// </summary>
    public class DiggingViewModel : OptionsPageViewModel
    {
        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header
        {
            get { return "Digging (Spade)"; }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            CacheFiles = Settings.Default.Digging_CacheFiles;
            CenterOnWhole = Settings.Default.Digging_CenterOnWhole;
            ComplexityAlertThreshold = Settings.Default.Digging_ComplexityAlertThreshold;
            ComplexityWarningThreshold = Settings.Default.Digging_ComplexityWarningThreshold;
            IndentationMargin = Settings.Default.Digging_IndentationMargin;
            SecondarySortTypeByName = Settings.Default.Digging_SecondarySortTypeByName;
            ShowItemComplexity = Settings.Default.Digging_ShowItemComplexity;
            ShowItemMetadata = Settings.Default.Digging_ShowItemMetadata;
            ShowMethodParameters = Settings.Default.Digging_ShowMethodParameters;
            SynchronizeOutlining = Settings.Default.Digging_SynchronizeOutlining;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public override void SaveSettings()
        {
            Settings.Default.Digging_CacheFiles = CacheFiles;
            Settings.Default.Digging_CenterOnWhole = CenterOnWhole;
            Settings.Default.Digging_ComplexityAlertThreshold = ComplexityAlertThreshold;
            Settings.Default.Digging_ComplexityWarningThreshold = ComplexityWarningThreshold;
            Settings.Default.Digging_IndentationMargin = IndentationMargin;
            Settings.Default.Digging_SecondarySortTypeByName = SecondarySortTypeByName;
            Settings.Default.Digging_ShowItemComplexity = ShowItemComplexity;
            Settings.Default.Digging_ShowItemMetadata = ShowItemMetadata;
            Settings.Default.Digging_ShowMethodParameters = ShowMethodParameters;
            Settings.Default.Digging_SynchronizeOutlining = SynchronizeOutlining;
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

        private bool _centerOnWhole;

        /// <summary>
        /// Gets or sets the flag indicating if the view should center on the whole item upon navigation.
        /// </summary>
        public bool CenterOnWhole
        {
            get { return _centerOnWhole; }
            set
            {
                if (_centerOnWhole != value)
                {
                    _centerOnWhole = value;
                    NotifyPropertyChanged("CenterOnWhole");
                }
            }
        }

        private int _complexityAlertThreshold;

        /// <summary>
        /// Gets or sets the complexity alert threshold.
        /// </summary>
        public int ComplexityAlertThreshold
        {
            get { return _complexityAlertThreshold; }
            set
            {
                if (_complexityAlertThreshold != value)
                {
                    _complexityAlertThreshold = value;
                    NotifyPropertyChanged("ComplexityAlertThreshold");
                }
            }
        }

        private int _complexityWarningThreshold;

        /// <summary>
        /// Gets or sets the complexity warning threshold.
        /// </summary>
        public int ComplexityWarningThreshold
        {
            get { return _complexityWarningThreshold; }
            set
            {
                if (_complexityWarningThreshold != value)
                {
                    _complexityWarningThreshold = value;
                    NotifyPropertyChanged("ComplexityWarningThreshold");
                }
            }
        }

        private int _indentationMargin;

        /// <summary>
        /// Gets or sets the indentation margin.
        /// </summary>
        public int IndentationMargin
        {
            get { return _indentationMargin; }
            set
            {
                if (_indentationMargin != value)
                {
                    _indentationMargin = value;
                    NotifyPropertyChanged("IndentationMargin");
                }
            }
        }

        private bool _secondarySortTypeByName;

        /// <summary>
        /// Gets or sets the flag indicating if secondary sorting during type layout should be on name.
        /// </summary>
        public bool SecondarySortTypeByName
        {
            get { return _secondarySortTypeByName; }
            set
            {
                if (_secondarySortTypeByName != value)
                {
                    _secondarySortTypeByName = value;
                    NotifyPropertyChanged("SecondarySortTypeByName");
                }
            }
        }

        private bool _showItemComplexity;

        /// <summary>
        /// Gets or sets the flag indicating if item complexity should be shown.
        /// </summary>
        public bool ShowItemComplexity
        {
            get { return _showItemComplexity; }
            set
            {
                if (_showItemComplexity != value)
                {
                    _showItemComplexity = value;
                    NotifyPropertyChanged("ShowItemComplexity");
                }
            }
        }

        private bool _showItemMetadata;

        /// <summary>
        /// Gets or sets the flag indicating if item metadata should be shown.
        /// </summary>
        public bool ShowItemMetadata
        {
            get { return _showItemMetadata; }
            set
            {
                if (_showItemMetadata != value)
                {
                    _showItemMetadata = value;
                    NotifyPropertyChanged("ShowItemMetadata");
                }
            }
        }

        private bool _showMethodParameters;

        /// <summary>
        /// Gets or sets the flag indicating if method parameters should be shown.
        /// </summary>
        public bool ShowMethodParameters
        {
            get { return _showMethodParameters; }
            set
            {
                if (_showMethodParameters != value)
                {
                    _showMethodParameters = value;
                    NotifyPropertyChanged("ShowMethodParameters");
                }
            }
        }

        private bool _synchronizeOutlining;

        /// <summary>
        /// Gets or sets the flag indicating if outlining should be synchronized with the code file.
        /// </summary>
        public bool SynchronizeOutlining
        {
            get { return _synchronizeOutlining; }
            set
            {
                if (_synchronizeOutlining != value)
                {
                    _synchronizeOutlining = value;
                    NotifyPropertyChanged("SynchronizeOutlining");
                }
            }
        }

        #endregion Options
    }
}