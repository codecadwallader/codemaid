#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using EnvDTE;
using Microsoft.VisualStudio.Package;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using SteveCadwallader.CodeMaid.UI;
using SteveCadwallader.CodeMaid.UI.Dialogs.Prompts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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
        /// The singleton instance of the <see cref="CodeCleanupAvailabilityLogic" /> class.
        /// </summary>
        private static CodeCleanupAvailabilityLogic _instance;

        /// <summary>
        /// Gets an instance of the <see cref="CodeCleanupAvailabilityLogic" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>An instance of the <see cref="CodeCleanupAvailabilityLogic" /> class.</returns>
        internal static CodeCleanupAvailabilityLogic GetInstance(CodeMaidPackage package)
        {
            return _instance ?? (_instance = new CodeCleanupAvailabilityLogic(package));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeCleanupAvailabilityLogic" /> class.
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
        /// Determines if the specified document is external to the solution.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>True if the document is external, otherwise false.</returns>
        internal bool IsDocumentExternal(Document document)
        {
            var projectItem = document.ProjectItem;
            if (projectItem == null || projectItem.Collection == null || projectItem.Kind != Constants.vsProjectItemKindPhysicalFile) return true;

            return projectItem.Collection.OfType<ProjectItem>().All(x => x.Object != projectItem.Object);
        }

        /// <summary>
        /// Determines if the specified document should be cleaned up.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="allowUserPrompts">A flag indicating if user prompts should be allowed.</param>
        /// <returns>True if item should be cleaned up, otherwise false.</returns>
        internal bool ShouldCleanup(Document document, bool allowUserPrompts = false)
        {
            return IsCleanupEnvironmentAvailable() &&
                   document != null &&
                   IsDocumentLanguageIncludedByOptions(document) &&
                   !IsDocumentExcludedBecauseExternal(document, allowUserPrompts) &&
                   !IsFileNameExcludedByOptions(document.Name) &&
                   !IsParentCodeGeneratorExcludedByOptions(document);
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
                   !IsFileNameExcludedByOptions(projectItem.Name) &&
                   !IsParentCodeGeneratorExcludedByOptions(projectItem);
        }

        #endregion Internal Methods

        #region Private Methods

        /// <summary>
        /// Attempts to get the file extension for the specified project item, otherwise an empty string.
        /// </summary>
        /// <param name="projectItem">The project item.</param>
        /// <returns>The file extension, otherwise an empty string.</returns>
        private static string GetProjectItemExtension(ProjectItem projectItem)
        {
            return Path.GetExtension(projectItem.Name) ?? string.Empty;
        }

        /// <summary>
        /// Determines whether the specified document should be excluded because it is external to
        /// the solution. Conditionally includes prompting the user.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="allowUserPrompts">A flag indicating if user prompts should be allowed.</param>
        /// <returns>
        /// True if document should be excluded because it is external to the solution, otherwise false.
        /// </returns>
        private bool IsDocumentExcludedBecauseExternal(Document document, bool allowUserPrompts)
        {
            bool isExternal = IsDocumentExternal(document);
            if (!isExternal) return false;

            switch ((AskYesNo)Settings.Default.Cleaning_PerformPartialCleanupOnExternal)
            {
                case AskYesNo.Ask:
                    if (allowUserPrompts)
                    {
                        return !PromptUserAboutCleaningExternalFiles(document);
                    }
                    break;

                case AskYesNo.Yes:
                    return false;

                case AskYesNo.No:
                    return true;
            }

            // If unresolved, defer exclusion for now.
            return false;
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
                case "Basic": return Settings.Default.Cleaning_IncludeVB;
                case "CSharp": return Settings.Default.Cleaning_IncludeCSharp;
                case "C/C++": return Settings.Default.Cleaning_IncludeCPlusPlus;
                case "CSS": return Settings.Default.Cleaning_IncludeCSS;
                case "F#": return Settings.Default.Cleaning_IncludeFSharp;
                case "HTML":
                case "HTMLX": return Settings.Default.Cleaning_IncludeHTML;
                case "JavaScript":
                case "JScript": return Settings.Default.Cleaning_IncludeJavaScript;
                case "LESS": return Settings.Default.Cleaning_IncludeLESS;
                case "TypeScript": return Settings.Default.Cleaning_IncludeTypeScript;
                case "XAML": return Settings.Default.Cleaning_IncludeXAML;
                case "XML": return Settings.Default.Cleaning_IncludeXML;
                default:
                    OutputWindowHelper.DiagnosticWriteLine(
                        string.Format("IsDocumentLanguageIncludedByOptions picked up an unrecognized document language '{0}'", document.Language));
                    return false;
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
        /// Determines whether the specified document has a parent item that is a code generator
        /// which is excluded by options.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>True if the parent is excluded by options, otherwise false.</returns>
        private static bool IsParentCodeGeneratorExcludedByOptions(Document document)
        {
            if (document == null) return false;

            return IsParentCodeGeneratorExcludedByOptions(document.ProjectItem);
        }

        /// <summary>
        /// Determines whether the specified project item has a parent item that is a code generator
        /// which is excluded by options.
        /// </summary>
        /// <param name="projectItem">The project item.</param>
        /// <returns>True if the parent is excluded by options, otherwise false.</returns>
        private static bool IsParentCodeGeneratorExcludedByOptions(ProjectItem projectItem)
        {
            if (projectItem == null || projectItem.Collection == null) return false;

            var parentProjectItem = projectItem.Collection.Parent as ProjectItem;
            if (parentProjectItem == null) return false;

            var extension = GetProjectItemExtension(parentProjectItem);
            if (extension.Equals(".tt", StringComparison.CurrentCultureIgnoreCase))
            {
                return Settings.Default.Cleaning_ExcludeT4GeneratedCode;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the language for the specified project item is included by configuration.
        /// </summary>
        /// <param name="projectItem">The project item.</param>
        /// <returns>True if the document language is included, otherwise false.</returns>
        private bool IsProjectItemLanguageIncludedByOptions(ProjectItem projectItem)
        {
            var extension = GetProjectItemExtension(projectItem);
            if (extension.Equals(".js", StringComparison.CurrentCultureIgnoreCase))
            {
                // Make an exception for JavaScript files - they may incorrectly return the HTML
                // language service.
                return Settings.Default.Cleaning_IncludeJavaScript;
            }

            var languageServiceGuid = EditorFactory.GetLanguageService(extension);
            switch (languageServiceGuid)
            {
                case "{694DD9B6-B865-4C5B-AD85-86356E9C88DC}": return Settings.Default.Cleaning_IncludeCSharp;
                case "{B2F072B0-ABC1-11D0-9D62-00C04FD9DFD9}": return Settings.Default.Cleaning_IncludeCPlusPlus;
                case "{a764e898-518d-11d2-9a89-00c04f79efc3}":
                case "{A764E898-518D-11d2-9A89-00C04F79EFC3}": return Settings.Default.Cleaning_IncludeCSS;
                case "{bc6dd5a5-d4d6-4dab-a00d-a51242dbaf1b}": return Settings.Default.Cleaning_IncludeFSharp;
                case "{9bbfd173-9770-47dc-b191-651b7ff493cd}":
                case "{58E975A0-F8FE-11D2-A6AE-00104BCC7269}": return Settings.Default.Cleaning_IncludeHTML;
                case "{59E2F421-410A-4fc9-9803-1F4E79216BE8}": return Settings.Default.Cleaning_IncludeJavaScript;
                case "{71d61d27-9011-4b17-9469-d20f798fb5c0}": return Settings.Default.Cleaning_IncludeJavaScript;
                case "{7b22909e-1b53-4cc7-8c2b-1f5c5039693a}": return Settings.Default.Cleaning_IncludeLESS;
                case "{4a0dddb5-7a95-4fbf-97cc-616d07737a77}": return Settings.Default.Cleaning_IncludeTypeScript;
                case "{E34ACDC0-BAAE-11D0-88BF-00A0C9110049}": return Settings.Default.Cleaning_IncludeVB;
                case "{c9164055-039b-4669-832d-f257bd5554d4}": return Settings.Default.Cleaning_IncludeXAML;
                case "{f6819a78-a205-47b5-be1c-675b3c7f0b8e}": return Settings.Default.Cleaning_IncludeXML;
                default:
                    OutputWindowHelper.DiagnosticWriteLine(
                        string.Format("IsProjectItemLanguageIncludedByOptions picked up an unrecognized language service guid '{0}'", languageServiceGuid));
                    return false;
            }
        }

        /// <summary>
        /// Prompts the user about cleaning up external files.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>True if external files should be cleaned, otherwise false.</returns>
        private static bool PromptUserAboutCleaningExternalFiles(Document document)
        {
            var viewModel = new YesNoPromptViewModel
                                {
                                    Title = @"CodeMaid: Cleanup External File",
                                    Message =
                                        document.Name + " is not in the solution so some cleanup actions may be unavailable." +
                                        Environment.NewLine + Environment.NewLine +
                                        "Do you want to perform a partial cleanup?",
                                    CanRemember = true
                                };

            var window = new YesNoPromptWindow { DataContext = viewModel };
            var response = window.ShowModal();

            if (!response.HasValue)
            {
                return false;
            }

            if (viewModel.Remember)
            {
                var preference = (int)(response.Value ? AskYesNo.Yes : AskYesNo.No);

                Settings.Default.Cleaning_PerformPartialCleanupOnExternal = preference;
                Settings.Default.Save();
            }

            return response.Value;
        }

        #endregion Private Methods
    }
}