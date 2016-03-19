using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for switching between files based on their filename.
    /// </summary>
    internal class SwitchFileCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchFileCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal SwitchFileCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(PackageGuids.GuidCodeMaidCommandSwitchFile, PackageIds.CmdIDCodeMaidSwitchFile))
        {
        }

        #endregion Constructors

        #region BaseCommand Methods

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            string alternatePath = GetAlternatePathIfExists(Package.ActiveDocument);
            bool canAlterate = !String.IsNullOrEmpty(alternatePath);

            Enabled = canAlterate;
            if (canAlterate)
            {
                Text = "S&witch to " + Path.GetFileName(alternatePath);
            }
            else
            {
                Text = "S&witch File";
            }
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            string alternatePath = GetAlternatePathIfExists(Package.ActiveDocument);
            if (!String.IsNullOrEmpty(alternatePath))
            {
                Package.IDE.ItemOperations.OpenFile(alternatePath, Constants.vsViewKindAny);
            }
        }

        #endregion BaseCommand Methods

        #region Private Methods

        /// <summary>
        /// Attempts to get a path to an alternate document for the specified document as well as
        /// checking that the alternate document exists in the solution.
        /// </summary>
        /// <param name="document">The document to analyze.</param>
        /// <returns>The path to an alternate document, otherwise null.</returns>
        private string GetAlternatePathIfExists(Document document)
        {
            var alternatePaths = GetAlternatePaths(document);

            return alternatePaths.FirstOrDefault(x => !String.IsNullOrEmpty(x) && Package.IDE.Solution.FindProjectItem(x) != null);
        }

        /// <summary>
        /// Attempts to get the alternate paths for the specified document.
        /// </summary>
        /// <param name="document">The document to analyze.</param>
        /// <returns>The alternate paths, otherwise null.</returns>
        private IEnumerable<string> GetAlternatePaths(Document document)
        {
            var results = new List<string>();

            if (document != null)
            {
                var path = document.FullName;
                if (!string.IsNullOrEmpty(path))
                {
                    foreach (var rfeGroup in _relatedFileExtensions.Value)
                    {
                        foreach (var extension in rfeGroup)
                        {
                            if (path.EndsWith(extension, StringComparison.CurrentCultureIgnoreCase))
                            {
                                var fileName = path.Substring(0, path.Length - extension.Length);
                                int matchingIndex = rfeGroup.IndexOf(extension);

                                var alternates = from i in rfeGroup
                                                 where i != extension
                                                 let index = rfeGroup.IndexOf(i)
                                                 orderby index > matchingIndex descending, index
                                                 select fileName + i;

                                results.AddRange(alternates);
                            }
                        }
                    }
                }
            }

            return results;
        }

        #endregion Private Methods

        #region Private Fields

        /// <summary>
        /// A cached setting set container for accessing sets of related file extensions.
        /// </summary>
        private readonly CachedSettingSet<IList<string>> _relatedFileExtensions =
            new CachedSettingSet<IList<string>>(() => Settings.Default.Switching_RelatedFileExtensionsExpression,
                                                      expression =>
                                                      expression.Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries)
                                                                .Select(rfeGroup =>
                                                                        rfeGroup.Split(' ')
                                                                                .Select(item => item.Trim().ToLower())
                                                                                .Where(x => !string.IsNullOrEmpty(x)).ToList())
                                                                .Where(list => list.Count >= 2).ToList());

        #endregion Private Fields
    }
}