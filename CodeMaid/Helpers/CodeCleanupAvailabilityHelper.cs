#region CodeMaid is Copyright 2007-2011 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2011 Steve Cadwallader.

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using EnvDTE;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A class for determining if cleanup can/should occur on specified items.
    /// </summary>
    internal class CodeCleanupAvailabilityHelper
    {
        #region Constructors

        /// <summary>
        /// The singleton instance of the <see cref="CodeCleanupAvailabilityHelper"/> class.
        /// </summary>
        private static CodeCleanupAvailabilityHelper _instance;

        /// <summary>
        /// Gets an instance of the <see cref="CodeCleanupAvailabilityHelper"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="CodeCleanupAvailabilityHelper"/> class.</returns>
        internal static CodeCleanupAvailabilityHelper GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new CodeCleanupAvailabilityHelper(package));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeCleanupAvailabilityHelper"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private CodeCleanupAvailabilityHelper(CodeMaidPackage package)
        {
            Package = package;
        }

        #endregion Constructors

        #region Internal Methods

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
                   !IsFileNameExcludedByOptions(projectItem.Name);
        }

        #endregion Internal Methods

        #region Private Methods

        /// <summary>
        /// Determines whether the environment is in a valid state for cleanup.
        /// </summary>
        /// <returns>True if cleanup can occur, false otherwise.</returns>
        private bool IsCleanupEnvironmentAvailable()
        {
            return Package.IDE.Debugger.CurrentMode == dbgDebugMode.dbgDesignMode;
        }

        /// <summary>
        /// Determines whether the language for the specified document is included by configuration.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>True if the document language is included, otherwise false.</returns>
        private bool IsDocumentLanguageIncludedByOptions(Document document)
        {
            switch (document.Language)
            {
                case "CSharp": return Package.Options.CleanupFileTypes.CleanupIncludeCSharp;
                case "C/C++": return Package.Options.CleanupFileTypes.CleanupIncludeCPlusPlus;
                case "CSS": return Package.Options.CleanupFileTypes.CleanupIncludeCSS;
                case "JScript": return Package.Options.CleanupFileTypes.CleanupIncludeJavaScript;
                case "HTML": return Package.Options.CleanupFileTypes.CleanupIncludeHTML;
                case "XAML": return Package.Options.CleanupFileTypes.CleanupIncludeXAML;
                case "XML": return Package.Options.CleanupFileTypes.CleanupIncludeXML;
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

        #endregion Private Methods

        #region Private Properties

        /// <summary>
        /// Gets a list of cleanup exclusion filters.
        /// </summary>
        public List<string> CleanupExclusions
        {
            get
            {
                var cleanupExclusionExpression = Package.Options.CleanupFileTypes.CleanupExclusionExpression;
                if (_cachedCleanupExclusionExpression != cleanupExclusionExpression)
                {
                    _cleanupExclusions = new List<string>();

                    if (!string.IsNullOrEmpty(cleanupExclusionExpression))
                    {
                        var filters = cleanupExclusionExpression.Split(';')
                                                                .Select(x => x.Trim().ToLower())
                                                                .Where(y => !string.IsNullOrEmpty(y));

                        foreach (var filter in filters)
                        {
                            _cleanupExclusions.Add(filter);
                        }
                    }

                    _cachedCleanupExclusionExpression = cleanupExclusionExpression;
                }

                return _cleanupExclusions;
            }
        }

        /// <summary>
        /// Gets or sets the hosting package.
        /// </summary>
        private CodeMaidPackage Package { get; set; }

        #endregion Private Properties

        #region Private Fields

        /// <summary>
        /// The cached state of the cleanup exclusion expression from options.
        /// </summary>
        private string _cachedCleanupExclusionExpression;

        /// <summary>
        /// A list of cleanup exclusion filters.
        /// </summary>
        private List<string> _cleanupExclusions = new List<string>();

        #endregion Private Fields
    }
}