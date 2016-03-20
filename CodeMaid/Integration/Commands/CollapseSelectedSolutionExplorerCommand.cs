using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for collapsing selected nodes in the solution explorer tool window.
    /// </summary>
    internal class CollapseSelectedSolutionExplorerCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CollapseSelectedSolutionExplorerCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal CollapseSelectedSolutionExplorerCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(PackageGuids.GuidCodeMaidCommandCollapseSelectedSolutionExplorer, PackageIds.CmdIDCodeMaidCollapseSelectedSolutionExplorer))
        {
        }

        #endregion Constructors

        #region BaseCommand Methods

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

        #endregion BaseCommand Methods

        #region Private Properties

        /// <summary>
        /// Gets an enumerable collection of the selected UI hierarchy items.
        /// </summary>
        private IEnumerable<UIHierarchyItem> SelectedUIHierarchyItems => UIHierarchyHelper.GetSelectedUIHierarchyItems(Package);

        #endregion Private Properties
    }
}