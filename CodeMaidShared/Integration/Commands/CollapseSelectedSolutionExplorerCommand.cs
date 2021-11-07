using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for collapsing selected nodes in the solution explorer tool window.
    /// </summary>
    internal sealed class CollapseSelectedSolutionExplorerCommand : BaseCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollapseSelectedSolutionExplorerCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal CollapseSelectedSolutionExplorerCommand(CodeMaidPackage package)
            : base(package, PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidCollapseSelectedSolutionExplorer)
        {
        }

        /// <summary>
        /// A singleton instance of this command.
        /// </summary>
        public static CollapseSelectedSolutionExplorerCommand Instance { get; private set; }

        /// <summary>
        /// Gets an enumerable collection of the selected UI hierarchy items.
        /// </summary>
        private IEnumerable<UIHierarchyItem> SelectedUIHierarchyItems => UIHierarchyHelper.GetSelectedUIHierarchyItems(Package);

        /// <summary>
        /// Initializes a singleton instance of this command.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>A task.</returns>
        public static async Task InitializeAsync(CodeMaidPackage package)
        {
            Instance = new CollapseSelectedSolutionExplorerCommand(package);
            await package.SettingsMonitor.WatchAsync(s => s.Feature_CollapseSelectedSolutionExplorer, Instance.SwitchAsync);
        }

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            Enabled = SelectedUIHierarchyItems.Any(x => x.UIHierarchyItems.Expanded);
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            foreach (UIHierarchyItem item in SelectedUIHierarchyItems)
            {
                UIHierarchyHelper.CollapseRecursively(item);
            }
        }
    }
}