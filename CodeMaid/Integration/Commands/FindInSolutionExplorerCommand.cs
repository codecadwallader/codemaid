using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using System;
using Task = System.Threading.Tasks.Task;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for finding a file in the solution explorer.
    /// </summary>
    internal sealed class FindInSolutionExplorerCommand : BaseCommand
    {
        private readonly CommandHelper _commandHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindInSolutionExplorerCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal FindInSolutionExplorerCommand(CodeMaidPackage package)
            : base(package, PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidFindInSolutionExplorer)
        {
            _commandHelper = CommandHelper.GetInstance(package);
        }

        /// <summary>
        /// A singleton instance of this command.
        /// </summary>
        public static FindInSolutionExplorerCommand Instance { get; private set; }

        /// <summary>
        /// Initializes a singleton instance of this command.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>A task.</returns>
        public static async Task InitializeAsync(CodeMaidPackage package)
        {
            Instance = new FindInSolutionExplorerCommand(package);
            await package.SettingsMonitor.WatchAsync(s => s.Feature_FindInSolutionExplorer, Instance.SwitchAsync);
        }

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            Enabled = Package.ActiveDocument != null;
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            Document document = Package.ActiveDocument;
            if (document != null)
            {
                if (Settings.Default.Finding_ClearSolutionExplorerSearch)
                {
                    ClearSolutionExplorerSearchFilter();
                }

                if (Settings.Default.Finding_TemporarilyOpenSolutionFolders)
                {
                    ToggleSolutionFoldersOpenTemporarily(UIHierarchyHelper.GetTopUIHierarchyItem(Package));
                }

                // Instead of directly using "SolutionExplorer.SyncWithActiveDocument" we are using
                // the GUID/ID pair. This is a workaround for the canonical name being undefined in
                // Spanish versions of Visual Studio.
                var command = _commandHelper.FindCommand("{D63DB1F0-404E-4B21-9648-CA8D99245EC3}", 36);
                if (command != null && command.IsAvailable)
                {
                    object customIn = null;
                    object customOut = null;
                    Package.IDE.Commands.Raise(command.Guid, command.ID, ref customIn, ref customOut);
                }
                else
                {
                    // The command will be unavailable if track active item is selected, and in those
                    // scenarios we just want to activate the solution explorer since the right item
                    // will already be highlighted.
                    Package.IDE.ExecuteCommand("View.SolutionExplorer", string.Empty);
                }
            }
        }

        /// <summary>
        /// Clears any exising search filtering in the solution explorer so that items not matching
        /// the query can be found.
        /// </summary>
        private void ClearSolutionExplorerSearchFilter()
        {
            var solutionExplorer = VsShellUtilities.GetUIHierarchyWindow(Package, VSConstants.StandardToolWindows.SolutionExplorer);
            var ws = solutionExplorer as IVsWindowSearch;
            ws?.ClearSearch();
        }

        /// <summary>
        /// Toggles all solution folders open temporarily to workaround searches not working inside
        /// solution folders that have never been expanded.
        /// </summary>
        /// <param name="parentItem">The parent item to inspect.</param>
        private void ToggleSolutionFoldersOpenTemporarily(UIHierarchyItem parentItem)
        {
            if (parentItem == null)
            {
                throw new ArgumentNullException(nameof(parentItem));
            }

            const string solutionFolderGuid = "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}";

            var project = parentItem.Object as Project;
            bool isCollapsedSolutionFolder = project != null && project.Kind == solutionFolderGuid && !parentItem.UIHierarchyItems.Expanded;

            // Expand the solution folder temporarily.
            if (isCollapsedSolutionFolder)
            {
                OutputWindowHelper.DiagnosticWriteLine($"FindInSolutionExplorerCommand.ToggleSolutionFoldersOpenTemporarily expanding '{parentItem.Name}'");

                parentItem.Select(vsUISelectionType.vsUISelectionTypeSelect);
                Package.IDE.ToolWindows.SolutionExplorer.DoDefaultAction();
            }

            // Run recursively to children as well for nested solution folders.
            foreach (UIHierarchyItem childItem in parentItem.UIHierarchyItems)
            {
                ToggleSolutionFoldersOpenTemporarily(childItem);
            }

            // Collapse the solution folder.
            if (isCollapsedSolutionFolder)
            {
                OutputWindowHelper.DiagnosticWriteLine($"FindInSolutionExplorerCommand.ToggleSolutionFoldersOpenTemporarily collapsing '{parentItem.Name}'");

                parentItem.Select(vsUISelectionType.vsUISelectionTypeSelect);
                Package.IDE.ToolWindows.SolutionExplorer.DoDefaultAction();
            }
        }
    }
}