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

        private const string SettingsFilename = "CodeMaid.config";

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
        /// Gets the path to the user settings file.
        /// </summary>
        internal static string GetUserSettingsPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CodeMaid", SettingsFilename);
        }

        /// <summary>
        /// Gets the path to the solution settings file based on the specified <see cref="SettingsContext"/>.
        /// </summary>
        /// <param name="context">
        /// A <see cref="T:System.Configuration.SettingsContext"/> describing the current
        /// application usage.
        /// </param>
        /// <returns>The path to the solution settings, otherwise null.</returns>
        internal static string GetSolutionSettingsPath(SettingsContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var solutionPath = context["SolutionPath"];

            return solutionPath != null ? Path.Combine(solutionPath.ToString(), SettingsFilename) : null;
        }

        /// <summary>
        /// Called when a solution is opened.
        /// </summary>
        internal void OnSolutionOpened()
        {
            LoadSolutionSpecificSettings(Settings.Default);
        }

        /// <summary>
        /// Called when a solution is closed.
        /// </summary>
        internal void OnSolutionClosed()
        {
            UnloadSolutionSpecificSettings(Settings.Default);
        }

        /// <summary>
        /// Loads the specified settings object with solution-specific settings if they exist or can
        /// be created.
        /// </summary>
        /// <param name="settings">The settings to update.</param>
        /// <param name="canCreate">A flag indicating if solution-specific settings can be created.</param>
        /// <returns>True if solution-specific settings were loaded, otherwise false.</returns>
        internal bool LoadSolutionSpecificSettings(Settings settings, bool canCreate = false)
        {
            if (_package.IDE.Solution.IsOpen && !string.IsNullOrWhiteSpace(_package.IDE.Solution.FullName))
            {
                var solutionPath = Path.GetDirectoryName(_package.IDE.Solution.FullName);
                if (!string.IsNullOrWhiteSpace(solutionPath))
                {
                    var solutionConfig = Path.Combine(solutionPath, SettingsFilename);

                    // Determine if there is a solution-specific settings file or one can be created.
                    if (File.Exists(solutionConfig) || canCreate)
                    {
                        // Reload the solution settings into the given settings (merge on top of user settings).
                        settings.Context["SolutionPath"] = solutionPath;
                        settings.Reload();
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Unloads solution-specific settings from the specified settings object.
        /// </summary>
        /// <param name="settings">The settings to update.</param>
        /// <returns>True if solution-specific settings were unloaded, otherwise false.</returns>
        internal bool UnloadSolutionSpecificSettings(Settings settings)
        {
            // Determine if there is a solution-specific settings file.
            if (settings.Context.ContainsKey("SolutionPath"))
            {
                // Unload the solution settings from the given settings (restore to user settings only).
                settings.Context.Remove("SolutionPath");
                settings.Reload();
                return true;
            }

            return false;
        }

        #endregion Methods
    }
}