#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.Reorganizing
{
    /// <summary>
    /// The view model for reorganizing regions options.
    /// </summary>
    public class ReorganizingRegionsViewModel : OptionsPageViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReorganizingRegionsViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        public ReorganizingRegionsViewModel(CodeMaidPackage package)
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
            get { return "Regions"; }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            AutoGenerate = Settings.Default.Reorganizing_RegionsAutoGenerate;
            IncludeAccessLevel = Settings.Default.Reorganizing_RegionsIncludeAccessLevel;
            InsertKeepEvenIfEmpty = Settings.Default.Reorganizing_RegionsInsertKeepEvenIfEmpty;
            RemoveExistingRegions = Settings.Default.Reorganizing_RegionsRemoveExistingRegions;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public override void SaveSettings()
        {
            Settings.Default.Reorganizing_RegionsAutoGenerate = AutoGenerate;
            Settings.Default.Reorganizing_RegionsIncludeAccessLevel = IncludeAccessLevel;
            Settings.Default.Reorganizing_RegionsInsertKeepEvenIfEmpty = InsertKeepEvenIfEmpty;
            Settings.Default.Reorganizing_RegionsRemoveExistingRegions = RemoveExistingRegions;
        }

        #endregion Overrides of OptionsPageViewModel

        #region Options

        private bool _autoGenerate;

        /// <summary>
        /// Gets or sets the flag indicating if regions should be automatically generated.
        /// </summary>
        public bool AutoGenerate
        {
            get { return _autoGenerate; }
            set
            {
                if (_autoGenerate != value)
                {
                    _autoGenerate = value;
                    NotifyPropertyChanged("AutoGenerate");
                }
            }
        }

        private bool _includeAccessLevel;

        /// <summary>
        /// Gets or sets the flag indicating if the access level should be included in the regions.
        /// </summary>
        public bool IncludeAccessLevel
        {
            get { return _includeAccessLevel; }
            set
            {
                if (_includeAccessLevel != value)
                {
                    _includeAccessLevel = value;
                    NotifyPropertyChanged("IncludeAccessLevel");
                }
            }
        }

        private bool _insertKeepEvenIfEmpty;

        /// <summary>
        /// Gets or sets the flag indicating if regions should be inserted or kept even if they would be empty.
        /// </summary>
        public bool InsertKeepEvenIfEmpty
        {
            get { return _insertKeepEvenIfEmpty; }
            set
            {
                if (_insertKeepEvenIfEmpty != value)
                {
                    _insertKeepEvenIfEmpty = value;
                    NotifyPropertyChanged("InsertKeepEvenIfEmpty");
                }
            }
        }

        private bool _removeExistingRegions;

        /// <summary>
        /// Gets or sets the flag indicating if the existing regions should be removed.
        /// </summary>
        public bool RemoveExistingRegions
        {
            get { return _removeExistingRegions; }
            set
            {
                if (_removeExistingRegions != value)
                {
                    _removeExistingRegions = value;
                    NotifyPropertyChanged("RemoveExistingRegions");
                }
            }
        }

        #endregion Options
    }
}