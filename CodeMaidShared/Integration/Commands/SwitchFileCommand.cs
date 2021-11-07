using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Task = System.Threading.Tasks.Task;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for switching between files based on their filename.
    /// </summary>
    internal sealed class SwitchFileCommand : BaseCommand
    {
        /// <summary>
        /// A cached setting set container for accessing sets of related file extensions.
        /// </summary>
        private readonly CachedSettingSet<IList<string>> _relatedFileExtensions =
            new CachedSettingSet<IList<string>>(()
                => Settings.Default.Switching_RelatedFileExtensionsExpression,
                   expression => expression
                       .Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries)
                       .Select(rfeGroup => rfeGroup
                           .Split(' ')
                           .Select(item => item.Trim().ToLower())
                           .Where(x => !string.IsNullOrEmpty(x)).ToList())
                       .Where(list => list.Count >= 2)
                       .ToList());

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchFileCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal SwitchFileCommand(CodeMaidPackage package)
            : base(package, PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidSwitchFile)
        {
        }

        /// <summary>
        /// A singleton instance of this command.
        /// </summary>
        public static SwitchFileCommand Instance { get; private set; }

        /// <summary>
        /// Initializes a singleton instance of this command.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>A task.</returns>
        public static async Task InitializeAsync(CodeMaidPackage package)
        {
            Instance = new SwitchFileCommand(package);
            await package.SettingsMonitor.WatchAsync(s => s.Feature_SwitchFile, Instance.SwitchAsync);
        }

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            string alternatePath = GetAlternatePathIfExists(Package.ActiveDocument);
            bool canAlterate = !string.IsNullOrEmpty(alternatePath);

            Enabled = canAlterate;
            if (canAlterate)
            {
                Text = Resources.SWitchTo + Path.GetFileName(alternatePath);
            }
            else
            {
                Text = Resources.SWitchFile;
            }
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            string alternatePath = GetAlternatePathIfExists(Package.ActiveDocument);
            if (!string.IsNullOrEmpty(alternatePath))
            {
                Package.IDE.ItemOperations.OpenFile(alternatePath, Constants.vsViewKindAny);
            }
        }

        /// <summary>
        /// Attempts to get a path to an alternate document for the specified document as well as
        /// checking that the alternate document exists in the solution.
        /// </summary>
        /// <param name="document">The document to analyze.</param>
        /// <returns>The path to an alternate document, otherwise null.</returns>
        private string GetAlternatePathIfExists(Document document)
        {
            var alternatePaths = GetAlternatePaths(document);

            return alternatePaths.FirstOrDefault(x => !string.IsNullOrEmpty(x) && Package.IDE.Solution.FindProjectItem(x) != null);
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
    }
}