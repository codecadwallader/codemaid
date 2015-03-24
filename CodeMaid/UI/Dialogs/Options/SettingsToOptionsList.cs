#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

using System.Collections.Generic;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options
{
    /// <summary>
    /// A specialized list class holding settings to options mappings.
    /// </summary>
    public class SettingsToOptionsList : List<ISettingToOptionMapping>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsToOptionsList"/> class.
        /// </summary>
        /// <param name="optionsPageViewModel">The options page view model.</param>
        public SettingsToOptionsList(OptionsPageViewModel optionsPageViewModel)
        {
            OptionsPageViewModel = optionsPageViewModel;
        }

        /// <summary>
        /// Gets or sets the <see cref="OptionsPageViewModel"/> that owns this list.
        /// </summary>
        public OptionsPageViewModel OptionsPageViewModel { get; set; }

        /// <summary>
        /// Iterates across all mappings, copying the setting values onto the options.
        /// </summary>
        public void CopySettingsToOptions()
        {
            foreach (var mapping in this)
            {
                mapping.CopySettingToOption(OptionsPageViewModel);
            }
        }

        /// <summary>
        /// Iterates across all mappings, copying the option values onto the settings.
        /// </summary>
        public void CopyOptionsToSettings()
        {
            foreach (var mapping in this)
            {
                mapping.CopyOptionToSetting(OptionsPageViewModel);
            }
        }
    }
}