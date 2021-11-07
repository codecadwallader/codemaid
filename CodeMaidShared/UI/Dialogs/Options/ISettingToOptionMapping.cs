using SteveCadwallader.CodeMaid.Properties;
using System.Reflection;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options
{
    /// <summary>
    /// An interface describing a setting to option mapping.
    /// </summary>
    public interface ISettingToOptionMapping
    {
        /// <summary>
        /// Gets the <see cref="PropertyInfo"/> defining the setting property.
        /// </summary>
        PropertyInfo SettingProperty { get; }

        /// <summary>
        /// Gets the <see cref="PropertyInfo"/> defining the option property.
        /// </summary>
        PropertyInfo OptionProperty { get; }

        /// <summary>
        /// Copies the value within the setting property onto the option property.
        /// </summary>
        /// <param name="settingsClass">The class instance for the settings property.</param>
        /// <param name="optionClass">The class instance for the option property.</param>
        void CopySettingToOption(Settings settingsClass, object optionClass);

        /// <summary>
        /// Copies the value within the option property onto the setting property.
        /// </summary>
        /// <param name="settingsClass">The class instance for the settings property.</param>
        /// <param name="optionClass">The class instance for the option property.</param>
        void CopyOptionToSetting(Settings settingsClass, object optionClass);
    }
}