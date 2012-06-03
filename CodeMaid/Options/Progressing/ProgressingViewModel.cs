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

namespace SteveCadwallader.CodeMaid.Options.Progressing
{
    /// <summary>
    /// The view model for progressing options.
    /// </summary>
    public class ProgressingViewModel : OptionsPageViewModel
    {
        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header
        {
            get { return "Progressing"; }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            HideBuildProgressOnBuildStop = Settings.Default.Progressing_HideBuildProgressOnBuildStop;
            ShowBuildProgressOnBuildStart = Settings.Default.Progressing_ShowBuildProgressOnBuildStart;
            ShowProgressOnWindowsTaskbar = Settings.Default.Progressing_ShowProgressOnWindowsTaskbar;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public override void SaveSettings()
        {
            Settings.Default.Progressing_HideBuildProgressOnBuildStop = HideBuildProgressOnBuildStop;
            Settings.Default.Progressing_ShowBuildProgressOnBuildStart = ShowBuildProgressOnBuildStart;
            Settings.Default.Progressing_ShowProgressOnWindowsTaskbar = ShowProgressOnWindowsTaskbar;
        }

        #endregion Overrides of OptionsPageViewModel

        #region Options

        private bool _hideBuildProgressOnBuildStop;

        /// <summary>
        /// Gets or sets a flag indicating if build progress should be hidden when a build stops.
        /// </summary>
        public bool HideBuildProgressOnBuildStop
        {
            get { return _hideBuildProgressOnBuildStop; }
            set
            {
                if (_hideBuildProgressOnBuildStop != value)
                {
                    _hideBuildProgressOnBuildStop = value;
                    NotifyPropertyChanged("HideBuildProgressOnBuildStop");
                }
            }
        }

        private bool _showBuildProgressOnBuildStart;

        /// <summary>
        /// Gets or sets a flag indicating if build progress should be shown when a build starts.
        /// </summary>
        public bool ShowBuildProgressOnBuildStart
        {
            get { return _showBuildProgressOnBuildStart; }
            set
            {
                if (_showBuildProgressOnBuildStart != value)
                {
                    _showBuildProgressOnBuildStart = value;
                    NotifyPropertyChanged("ShowBuildProgressOnBuildStart");
                }
            }
        }

        private bool _showProgressOnWindowsTaskbar;

        /// <summary>
        /// Gets or sets a flag indicating if build progress should be shown on the windows taskbar.
        /// </summary>
        public bool ShowProgressOnWindowsTaskbar
        {
            get { return _showProgressOnWindowsTaskbar; }
            set
            {
                if (_showProgressOnWindowsTaskbar != value)
                {
                    _showProgressOnWindowsTaskbar = value;
                    NotifyPropertyChanged("ShowProgressOnWindowsTaskbar");
                }
            }
        }

        #endregion Options
    }
}