#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

using SteveCadwallader.CodeMaid.Properties;
using System;
using System.Configuration;
using System.IO;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A singleton helper class for aiding in settings context operations (e.g. user or solution settings).
    /// </summary>
    internal class SettingsContextHelper
    {
        #region Constants

        private const string ConfigFilename = "CodeMaid.config";

        #endregion Constants

        #region Fields

        private readonly CodeMaidPackage _package;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// The singleton instance of the <see cref="SettingsContextHelper" /> class.
        /// </summary>
        private static SettingsContextHelper _instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsContextHelper" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private SettingsContextHelper(CodeMaidPackage package)
        {
            _package = package;
        }

        /// <summary>
        /// Gets an instance of the <see cref="SettingsContextHelper" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="SettingsContextHelper" /> class.</returns>
        internal static SettingsContextHelper GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new SettingsContextHelper(package));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets the path to the user configuration file.
        /// </summary>
        public static string GetUserConfigPath()
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
        public static string GetSolutionConfigPath(SettingsContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            var solutionPath = context["SolutionPath"];

            return solutionPath != null ? Path.Combine(solutionPath.ToString(), ConfigFilename) : null;
        }

        /// <summary>
        /// Called when a solution is opened.
        /// </summary>
        internal void OnSolutionOpened()
        {
            var solutionPath = Path.GetDirectoryName(_package.IDE.Solution.FullName);
            var solutionConfig = Path.Combine(solutionPath, ConfigFilename);

            // Determine if there is a solution-specific settings file.
            if (File.Exists(solutionConfig))
            {
                // Reload the solution settings into the default settings (merge on top of user settings).
                Settings.Default.Context["SolutionPath"] = solutionPath;
                Settings.Default.Reload();
            }
        }

        /// <summary>
        /// Called when a solution is closed.
        /// </summary>
        internal void OnSolutionClosed()
        {
            // Determine if there is a solution-specific settings file.
            if (Settings.Default.Context.ContainsKey("SolutionPath"))
            {
                // Unload the solution settings from the default settings (restore to user settings only).
                Settings.Default.Context.Remove("SolutionPath");
                Settings.Default.Reload();
            }
        }

        #endregion Methods
    }
}