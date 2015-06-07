#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

using System;
using System.Configuration;
using System.IO;
using System.Xml;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// This class handles customizing the settings persistance.
    /// </summary>
    public class CodeMaidSettingsProvider : LocalFileSettingsProvider
    {
        #region Constants

        private const string ConfigFilename = "CodeMaid.config";

        #endregion Constants

        #region Overridden Members

        /// <summary>
        /// Returns the collection of setting property values for the specified application instance
        /// and settings property group.
        /// </summary>
        /// <param name="context">
        /// A <see cref="T:System.Configuration.SettingsContext"/> describing the current
        /// application usage.
        /// </param>
        /// <param name="properties">
        /// A <see cref="T:System.Configuration.SettingsPropertyCollection"/> containing the
        /// settings property group whose values are to be retrieved.
        /// </param>
        /// <returns>
        /// A <see cref="T:System.Configuration.SettingsPropertyValueCollection"/> containing the
        /// values for the specified settings property group.
        /// </returns>
        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection properties)
        {
            try
            {
                var solutionConfigPath = GetSolutionConfigPath(context);
                var userConfigPath = GetUserConfigPath();
                var sectionName = GetSectionName(context);

                var userSettings = ReadSettingsFromFile(userConfigPath, sectionName);
                var solutionSettings = ReadSettingsFromFile(solutionConfigPath, sectionName);

                var propertyValues = MergeSettingsIntoPropertyValues(userSettings, solutionSettings, properties);

                return propertyValues;
            }
            catch (Exception ex)
            {
                OutputWindowHelper.ExceptionWriteLine("Unable to GetPropertyValues.", ex);
                throw;
            }
        }

        /// <summary>
        /// Sets the values of the specified group of property settings.
        /// </summary>
        /// <param name="context">
        /// A <see cref="T:System.Configuration.SettingsContext"/> describing the current
        /// application usage.
        /// </param>
        /// <param name="values">
        /// A <see cref="T:System.Configuration.SettingsPropertyValueCollection"/> representing the
        /// group of property settings to set.
        /// </param>
        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection values)
        {
            try
            {
                var configPath = GetSolutionConfigPath(context) ?? GetUserConfigPath();
                var sectionName = GetSectionName(context);

                WriteSettingsToFile(configPath, sectionName, values);
            }
            catch (Exception ex)
            {
                OutputWindowHelper.ExceptionWriteLine("Unable to SetPropertyValues.", ex);
                throw;
            }
        }

        #endregion Overridden Members

        #region Shared Methods

        /// <summary>
        /// Gets the path to the user configuration file.
        /// </summary>
        private static string GetUserConfigPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CodeMaid", ConfigFilename);
        }

        /// <summary>
        /// Gets the path to the solution configuration file based on the specified <see cref="SettingsContext"/>.
        /// </summary>
        /// <param name="context">
        /// A <see cref="T:System.Configuration.SettingsContext"/> describing the current
        /// application usage.
        /// </param>
        /// <returns>The path to the solution configuration, otherwise null.</returns>
        private static string GetSolutionConfigPath(SettingsContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            var solutionPath = context["SolutionPath"];

            return solutionPath != null ? Path.Combine(solutionPath.ToString(), ConfigFilename) : null;
        }

        /// <summary>
        /// Gets the name of the section where settings will be located based on the specified <see cref="SettingsContext"/>.
        /// </summary>
        /// <param name="context">
        /// A <see cref="T:System.Configuration.SettingsContext"/> describing the current
        /// application usage.
        /// </param>
        /// <returns>The section name, otherwise null.</returns>
        private static string GetSectionName(SettingsContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            var groupName = context["GroupName"];

            return groupName != null ? groupName.ToString() : null;
        }

        /// <summary>
        /// Gets the <see cref="Configuration"/> at the specified path.
        /// </summary>
        /// <param name="path">The path to the configuration file.</param>
        /// <returns>The <see cref="Configuration"/> object.</returns>
        private static Configuration GetConfiguration(string path)
        {
            if (path == null) throw new ArgumentNullException("path");

            var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = path };
            var config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            return config;
        }

        /// <summary>
        /// Gets the <see cref="SettingElementCollection"/> for the specified section name within
        /// the specified configuration.
        /// </summary>
        /// <param name="config">The <see cref="Configuration"/> object.</param>
        /// <param name="sectionName">The settings section name.</param>
        /// <returns>
        /// A <see cref="SettingElementCollection"/> for the section, or an empty section if not found.
        /// </returns>
        private static SettingElementCollection GetSettingElementCollection(Configuration config, string sectionName)
        {
            var userSettings = config.GetSectionGroup("userSettings");
            if (userSettings != null)
            {
                var section = userSettings.Sections[sectionName] as ClientSettingsSection;
                if (section != null)
                {
                    return section.Settings;
                }
            }

            return new SettingElementCollection();
        }

        #endregion Shared Methods

        #region Read Methods

        /// <summary>
        /// Reads settings from a configuration file at the specified path.
        /// </summary>
        /// <param name="path">The configuration file path.</param>
        /// <param name="sectionName">The name of the settings section.</param>
        /// <returns>A collection representing the settings, otherwise an empty collection.</returns>
        private static SettingElementCollection ReadSettingsFromFile(string path, string sectionName)
        {
            try
            {
                if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(sectionName))
                {
                    var config = GetConfiguration(path);
                    var settings = GetSettingElementCollection(config, sectionName);

                    return settings;
                }
            }
            catch (Exception ex)
            {
                OutputWindowHelper.ExceptionWriteLine("Unable to read settings.", ex);
            }

            return new SettingElementCollection();
        }

        /// <summary>
        /// Merges the specified user and solution settings into a new <see cref="SettingsPropertyValueCollection"/>.
        /// </summary>
        /// <param name="userSettings">The user settings.</param>
        /// <param name="solutionSettings">The solution settings.</param>
        /// <param name="properties">The setting properties collection.</param>
        /// <returns>A merged <see cref="SettingsPropertyValueCollection"/>.</returns>
        private static SettingsPropertyValueCollection MergeSettingsIntoPropertyValues(SettingElementCollection userSettings, SettingElementCollection solutionSettings, SettingsPropertyCollection properties)
        {
            var values = new SettingsPropertyValueCollection();

            foreach (SettingsProperty property in properties)
            {
                var value = new SettingsPropertyValue(property);

                ApplySettingToValue(value, userSettings);
                ApplySettingToValue(value, solutionSettings);

                value.IsDirty = false;

                values.Add(value);
            }
            return values;
        }

        /// <summary>
        /// Checks the specified settings collection to see if it has a serialized value that should
        /// be applied to the specified <see cref="SettingsPropertyValue"/>.
        /// </summary>
        /// <param name="value">An individual settings property value.</param>
        /// <param name="settings">A collection representing the settings.</param>
        private static void ApplySettingToValue(SettingsPropertyValue value, SettingElementCollection settings)
        {
            var setting = settings.Get(value.Name);
            if (setting != null)
            {
                value.SerializedValue = setting.Value.ValueXml.InnerText;

                // Mark the value as not deserialized, which will trigger a deserialization of the SerializedValue into the PropertyValue.
                value.Deserialized = false;
            }
        }

        #endregion Read Methods

        #region Write Methods

        /// <summary>
        /// Writes settings to a configuration file at the specified path.
        /// </summary>
        /// <param name="path">The configuration file path.</param>
        /// <param name="sectionName">The name of the settings section.</param>
        /// <param name="values">
        /// A <see cref="T:System.Configuration.SettingsPropertyValueCollection"/> representing the
        /// group of property settings to set.
        /// </param>
        private static void WriteSettingsToFile(string path, string sectionName, SettingsPropertyValueCollection values)
        {
            try
            {
                if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(sectionName))
                {
                    var config = GetConfiguration(path);
                    var settings = GetSettingElementCollection(config, sectionName);

                    UpdateSettingsFromPropertyValues(settings, values);

                    config.Save();
                }
            }
            catch (Exception ex)
            {
                OutputWindowHelper.ExceptionWriteLine("Unable to write settings.", ex);
            }
        }

        /// <summary>
        /// Updates the <see cref="SettingElementCollection"/> from the <see cref="SettingsPropertyValueCollection"/>.
        /// </summary>
        /// <param name="settings">A collection representing the settings.</param>
        /// <param name="values">
        /// A <see cref="T:System.Configuration.SettingsPropertyValueCollection"/> representing the
        /// group of property settings to set.
        /// </param>
        private static void UpdateSettingsFromPropertyValues(SettingElementCollection settings, SettingsPropertyValueCollection values)
        {
            foreach (SettingsPropertyValue value in values)
            {
                if (value.IsDirty)
                {
                    var element = settings.Get(value.Name);
                    if (element == null)
                    {
                        // Note: We only support string serialization for brevity of implementation.
                        element = new SettingElement(value.Name, SettingsSerializeAs.String);
                        settings.Add(element);
                    }

                    element.SerializeAs = SettingsSerializeAs.String;
                    element.Value.ValueXml = CreateXmlValue(value.SerializedValue);
                }
            }
        }

        /// <summary>
        /// Creates an <see cref="XmlNode"/> containing the specified serialized value.
        /// </summary>
        /// <param name="serializedValue">The serialized value.</param>
        /// <returns>The <see cref="XmlNode"/>.</returns>
        private static XmlNode CreateXmlValue(object serializedValue)
        {
            var node = new XmlDocument().CreateElement("value");
            node.InnerText = serializedValue.ToString();

            return node;
        }

        #endregion Write Methods
    }
}