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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using EnvDTE;
using Microsoft.VisualStudio.Package;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.Logic.Cleaning
{
    /// <summary>
    /// A class for determining if cleanup can/should occur on specified items.
    /// </summary>
    internal class CodeCleanupAvailabilityLogic
    {
        #region Fields

        private readonly CodeMaidPackage _package;

        private readonly CachedSettingSet<string> _cleanupExclusions =
            new CachedSettingSet<string>(() => Settings.Default.Cleaning_ExclusionExpression,
                                         expression =>
                                         expression.Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries)
                                                   .Select(x => x.Trim().ToLower())
                                                   .Where(y => !string.IsNullOrEmpty(y))
                                                   .ToList());

        private EditorFactory _editorFactory;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// The singleton instance of the <see cref="CodeCleanupAvailabilityLogic"/> class.
        /// </summary>
        private static CodeCleanupAvailabilityLogic _instance;

        /// <summary>
        /// Gets an instance of the <see cref="CodeCleanupAvailabilityLogic"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="CodeCleanupAvailabilityLogic"/> class.</returns>
        internal static CodeCleanupAvailabilityLogic GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new CodeCleanupAvailabilityLogic(package));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeCleanupAvailabilityLogic"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private CodeCleanupAvailabilityLogic(CodeMaidPackage package)
        {
            _package = package;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets a set of cleanup exclusion filters.
        /// </summary>
        private IEnumerable<string> CleanupExclusions
        {
            get { return _cleanupExclusions.Value; }
        }

        /// <summary>
        /// A default editor factory, used for its knowledge of language service-extension mappings.
        /// </summary>
        private EditorFactory EditorFactory
        {
            get { return _editorFactory ?? (_editorFactory = new EditorFactory()); }
        }

        #endregion Properties

        #region Internal Methods

        /// <summary>
        /// Determines whether the environment is in a valid state for cleanup.
        /// </summary>
        /// <returns>True if cleanup can occur, false otherwise.</returns>
        internal bool IsCleanupEnvironmentAvailable()
        {
            return _package.IDE.Debugger.CurrentMode == dbgDebugMode.dbgDesignMode;
        }

        /// <summary>
        /// Determines if the specified document should be cleaned up.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>True if item should be cleaned up, otherwise false.</returns>
        internal bool ShouldCleanup(Document document)
        {
            return IsCleanupEnvironmentAvailable() &&
                   document != null &&
                   IsDocumentLanguageIncludedByOptions(document) &&
                   !IsFileNameExcludedByOptions(document.Name);
        }

        /// <summary>
        /// Determines if the specified project item should be cleaned up.
        /// </summary>
        /// <param name="projectItem">The project item.</param>
        /// <returns>True if item should be cleaned up, otherwise false.</returns>
        internal bool ShouldCleanup(ProjectItem projectItem)
        {
            return IsCleanupEnvironmentAvailable() &&
                   projectItem != null &&
                   projectItem.Kind == Constants.vsProjectItemKindPhysicalFile &&
                   IsProjectItemLanguageIncludedByOptions(projectItem) &&
                   !IsFileNameExcludedByOptions(projectItem.Name);
        }

        #endregion Internal Methods

        #region Private Methods

        /// <summary>
        /// Determines whether the language for the specified document is included by configuration.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>True if the document language is included, otherwise false.</returns>
        private bool IsDocumentLanguageIncludedByOptions(Document document)
        {
            switch (document.Language)
            {
                case "CSharp": return Settings.Default.Cleaning_IncludeCSharp;
                case "C/C++": return Settings.Default.Cleaning_IncludeCPlusPlus;
                case "CSS": return Settings.Default.Cleaning_IncludeCSS;
                case "HTML": return Settings.Default.Cleaning_IncludeHTML;
                case "JScript": return Settings.Default.Cleaning_IncludeJavaScript;
                case "XAML": return Settings.Default.Cleaning_IncludeXAML;
                case "XML": return Settings.Default.Cleaning_IncludeXML;
                default: return false;
            }
        }

        /// <summary>
        /// Determines whether the specified filename is excluded by configuration.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns>True if the filename is excluded, otherwise false.</returns>
        private bool IsFileNameExcludedByOptions(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return false;
            }

            var cleanupExclusions = CleanupExclusions;
            if (cleanupExclusions == null)
            {
                return false;
            }

            return cleanupExclusions.Any(cleanupExclusion => Regex.IsMatch(filename, cleanupExclusion, RegexOptions.IgnoreCase));
        }

        /// <summary>
        /// Determines whether the language for the specified project item is included by configuration.
        /// </summary>
        /// <param name="projectItem">The project item.</param>
        /// <returns>True if the document language is included, otherwise false.</returns>
        private bool IsProjectItemLanguageIncludedByOptions(ProjectItem projectItem)
        {
            var extension = Path.GetExtension(projectItem.Name) ?? string.Empty;
            if (extension.Equals(".js", StringComparison.CurrentCultureIgnoreCase))
            {
                // Make an exception for JavaScript files - they incorrectly return the HTML language service.
                return Settings.Default.Cleaning_IncludeJavaScript;
            }

            var languageServiceGuid = EditorFactory.GetLanguageService(extension);
            switch (languageServiceGuid)
            {
                case "{694DD9B6-B865-4C5B-AD85-86356E9C88DC}": return Settings.Default.Cleaning_IncludeCSharp;
                case "{B2F072B0-ABC1-11D0-9D62-00C04FD9DFD9}": return Settings.Default.Cleaning_IncludeCPlusPlus;
                case "{A764E898-518D-11d2-9A89-00C04F79EFC3}": return Settings.Default.Cleaning_IncludeCSS;
                case "{58E975A0-F8FE-11D2-A6AE-00104BCC7269}": return Settings.Default.Cleaning_IncludeHTML;
                case "{59E2F421-410A-4fc9-9803-1F4E79216BE8}": return Settings.Default.Cleaning_IncludeJavaScript;
                case "{c9164055-039b-4669-832d-f257bd5554d4}": return Settings.Default.Cleaning_IncludeXAML;
                case "{f6819a78-a205-47b5-be1c-675b3c7f0b8e}": return Settings.Default.Cleaning_IncludeXML;
                default: return false;
            }
        }

        #endregion Private Methods
    }
}