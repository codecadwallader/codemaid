#region CodeMaid is Copyright 2007-2016 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2016 Steve Cadwallader.

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