#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

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
            Mappings = new SettingsToOptionsList(this)
            {
                new SettingToOptionMapping<bool, bool>(x => Settings.Default.Reorganizing_RegionsIncludeAccessLevel, x => IncludeAccessLevel),
                new SettingToOptionMapping<bool, bool>(x => Settings.Default.Reorganizing_RegionsInsertKeepEvenIfEmpty, x => InsertKeepEvenIfEmpty),
                new SettingToOptionMapping<bool, bool>(x => Settings.Default.Reorganizing_RegionsInsertNewRegions, x => InsertNewRegions),
                new SettingToOptionMapping<bool, bool>(x => Settings.Default.Reorganizing_RegionsRemoveExistingRegions, x => RemoveExistingRegions)
            };
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

        #endregion Overrides of OptionsPageViewModel

        #region Options

        /// <summary>
        /// Gets or sets the flag indicating if the access level should be included in the regions.
        /// </summary>
        public bool IncludeAccessLevel
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if regions should be inserted or kept even if they would be empty.
        /// </summary>
        public bool InsertKeepEvenIfEmpty
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if new regions should be inserted.
        /// </summary>
        public bool InsertNewRegions
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        /// <summary>
        /// Gets or sets the flag indicating if the existing regions should be removed.
        /// </summary>
        public bool RemoveExistingRegions
        {
            get { return GetPropertyValue<bool>(); }
            set { SetPropertyValue(value); }
        }

        #endregion Options
    }
}