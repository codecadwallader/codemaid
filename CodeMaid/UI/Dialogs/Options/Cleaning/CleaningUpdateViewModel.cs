#region CodeMaid is Copyright 2007-2013 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2013 Steve Cadwallader.

using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.Cleaning
{
    /// <summary>
    /// The view model for cleaning update options.
    /// </summary>
    public class CleaningUpdateViewModel : OptionsPageViewModel
    {
        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header
        {
            get { return "Update"; }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            UpdateAccessorsToBothBeSingleLineOrMultiLine = Settings.Default.Cleaning_UpdateAccessorsToBothBeSingleLineOrMultiLine;
            UpdateEndRegionDirectives = Settings.Default.Cleaning_UpdateEndRegionDirectives;
            UpdateSingleLineMethods = Settings.Default.Cleaning_UpdateSingleLineMethods;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public override void SaveSettings()
        {
            Settings.Default.Cleaning_UpdateAccessorsToBothBeSingleLineOrMultiLine = UpdateAccessorsToBothBeSingleLineOrMultiLine;
            Settings.Default.Cleaning_UpdateEndRegionDirectives = UpdateEndRegionDirectives;
            Settings.Default.Cleaning_UpdateSingleLineMethods = UpdateSingleLineMethods;
        }

        #endregion Overrides of OptionsPageViewModel

        #region Options

        private bool _updateAccessorsToBothBeSingleLineOrMultiLine;

        /// <summary>
        /// Gets or sets the flag indicating if accessors should be updated to both be single line or multi line.
        /// </summary>
        public bool UpdateAccessorsToBothBeSingleLineOrMultiLine
        {
            get { return _updateAccessorsToBothBeSingleLineOrMultiLine; }
            set
            {
                if (_updateAccessorsToBothBeSingleLineOrMultiLine != value)
                {
                    _updateAccessorsToBothBeSingleLineOrMultiLine = value;
                    NotifyPropertyChanged("UpdateAccessorsToBothBeSingleLineOrMultiLine");
                }
            }
        }

        private bool _updateEndRegionDirectives;

        /// <summary>
        /// Gets or sets the flag indicating if end region directives should be updated.
        /// </summary>
        public bool UpdateEndRegionDirectives
        {
            get { return _updateEndRegionDirectives; }
            set
            {
                if (_updateEndRegionDirectives != value)
                {
                    _updateEndRegionDirectives = value;
                    NotifyPropertyChanged("UpdateEndRegionDirectives");
                }
            }
        }

        private bool _updateSingleLineMethods;

        /// <summary>
        /// Gets or sets the flag indicating if single line methods should be updated.
        /// </summary>
        public bool UpdateSingleLineMethods
        {
            get { return _updateSingleLineMethods; }
            set
            {
                if (_updateSingleLineMethods != value)
                {
                    _updateSingleLineMethods = value;
                    NotifyPropertyChanged("UpdateSingleLineMethods");
                }
            }
        }

        #endregion Options
    }
}