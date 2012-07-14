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

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.Reorganizing
{
    /// <summary>
    /// The view model for reorganizing options.
    /// </summary>
    public class ReorganizingViewModel : OptionsPageViewModel
    {
        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header
        {
            get { return "Reorganizing"; }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public override void LoadSettings()
        {
            AlphabetizeMembersOfTheSameGroup = Settings.Default.Reorganizing_AlphabetizeMembersOfTheSameGroup;
            KeepMembersWithinRegions = Settings.Default.Reorganizing_KeepMembersWithinRegions;
            RunAtStartOfCleanup = Settings.Default.Reorganizing_RunAtStartOfCleanup;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public override void SaveSettings()
        {
            Settings.Default.Reorganizing_AlphabetizeMembersOfTheSameGroup = AlphabetizeMembersOfTheSameGroup;
            Settings.Default.Reorganizing_KeepMembersWithinRegions = KeepMembersWithinRegions;
            Settings.Default.Reorganizing_RunAtStartOfCleanup = RunAtStartOfCleanup;
        }

        #endregion Overrides of OptionsPageViewModel

        #region Options

        private bool _alphabetizeMembersOfTheSameGroup;

        /// <summary>
        /// Gets or sets the flag indicating if members of the same group should be alphabetized.
        /// </summary>
        public bool AlphabetizeMembersOfTheSameGroup
        {
            get { return _alphabetizeMembersOfTheSameGroup; }
            set
            {
                if (_alphabetizeMembersOfTheSameGroup != value)
                {
                    _alphabetizeMembersOfTheSameGroup = value;
                    NotifyPropertyChanged("AlphabetizeMembersOfTheSameGroup");
                }
            }
        }

        private bool _keepMembersWithinRegions;

        /// <summary>
        /// Gets or sets the flag indicating if members should be kept within regions.
        /// </summary>
        public bool KeepMembersWithinRegions
        {
            get { return _keepMembersWithinRegions; }
            set
            {
                if (_keepMembersWithinRegions != value)
                {
                    _keepMembersWithinRegions = value;
                    NotifyPropertyChanged("KeepMembersWithinRegions");
                }
            }
        }

        private bool _runAtStartOfCleanup;

        /// <summary>
        /// Gets or sets the flag indicating if reorganizing should be run at the start of cleanup.
        /// </summary>
        public bool RunAtStartOfCleanup
        {
            get { return _runAtStartOfCleanup; }
            set
            {
                if (_runAtStartOfCleanup != value)
                {
                    _runAtStartOfCleanup = value;
                    NotifyPropertyChanged("RunAtStartOfCleanup");
                }
            }
        }

        #endregion Options
    }
}