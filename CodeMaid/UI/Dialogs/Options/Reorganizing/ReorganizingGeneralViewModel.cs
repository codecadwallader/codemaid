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
    /// The view model for reorganizing general options.
    /// </summary>
    public class ReorganizingGeneralViewModel : OptionsPageViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReorganizingGeneralViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        public ReorganizingGeneralViewModel(CodeMaidPackage package)
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
            AlphabetizeMembersOfTheSameGroup = Settings.Default.Reorganizing_AlphabetizeMembersOfTheSameGroup;
            KeepMembersWithinRegions = Settings.Default.Reorganizing_KeepMembersWithinRegions;
            PrimaryOrderByAccessLevel = Settings.Default.Reorganizing_PrimaryOrderByAccessLevel;
            RunAtStartOfCleanup = Settings.Default.Reorganizing_RunAtStartOfCleanup;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public override void SaveSettings()
        {
            Settings.Default.Reorganizing_AlphabetizeMembersOfTheSameGroup = AlphabetizeMembersOfTheSameGroup;
            Settings.Default.Reorganizing_KeepMembersWithinRegions = KeepMembersWithinRegions;
            Settings.Default.Reorganizing_PrimaryOrderByAccessLevel = PrimaryOrderByAccessLevel;
            Settings.Default.Reorganizing_RunAtStartOfCleanup = RunAtStartOfCleanup;
        }

        #endregion Overrides of OptionsPageViewModel

        #region Options

        /// <summary>
        /// Gets or sets the flag indicating if members of the same group should be alphabetized.
        /// </summary>
        public bool AlphabetizeMembersOfTheSameGroup
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if members should be kept within regions.
        /// </summary>
        public bool KeepMembersWithinRegions
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if primary ordering should be by access level.
        /// </summary>
        public bool PrimaryOrderByAccessLevel
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if reorganizing should be run at the start of cleanup.
        /// </summary>
        public bool RunAtStartOfCleanup
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        #endregion Options
    }
}