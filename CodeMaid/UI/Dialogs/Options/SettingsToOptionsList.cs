using SteveCadwallader.CodeMaid.Properties;
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
        /// <param name="activeSettings">The active settings.</param>
        /// <param name="optionsPageViewModel">The options page view model.</param>
        public SettingsToOptionsList(Settings activeSettings, OptionsPageViewModel optionsPageViewModel)
        {
            ActiveSettings = activeSettings;
            OptionsPageViewModel = optionsPageViewModel;
        }

        /// <summary>
        /// Gets the active settings.
        /// </summary>
        public Settings ActiveSettings { get; private set; }

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
                mapping.CopySettingToOption(ActiveSettings, OptionsPageViewModel);
            }
        }

        /// <summary>
        /// Iterates across all mappings, copying the option values onto the settings.
        /// </summary>
        public void CopyOptionsToSettings()
        {
            foreach (var mapping in this)
            {
                mapping.CopyOptionToSetting(ActiveSettings, OptionsPageViewModel);
            }
        }
    }
}