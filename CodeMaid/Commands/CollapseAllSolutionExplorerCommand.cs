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

using System.ComponentModel.Design;
using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.Commands
{
    /// <summary>
    /// A command that provides for collapsing nodes in the solution explorer tool window.
    /// </summary>
    internal class CollapseAllSolutionExplorerCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CollapseAllSolutionExplorerCommand"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal CollapseAllSolutionExplorerCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(GuidList.GuidCodeMaidCommandCollapseAllSolutionExplorer, (int)PkgCmdIDList.CmdIDCodeMaidCollapseAllSolutionExplorer))
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the top level (solution) UI hierarchy item.
        /// </summary>
        private UIHierarchyItem TopUIHierarchyItem
        {
            get { return UIHierarchyHelper.GetTopUIHierarchyItem(Package); }
        }

        #endregion Properties

        #region BaseCommand Methods

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            Enabled = Package.IDE.Solution.IsOpen;
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            var topItem = TopUIHierarchyItem;

            if (topItem != null && UIHierarchyHelper.HasExpandedChildren(topItem))
            {
                UIHierarchyHelper.CollapseRecursively(TopUIHierarchyItem);
            }
        }

        #endregion BaseCommand Methods

        #region Methods

        /// <summary>
        /// Called when a solution is opened.
        /// </summary>
        internal void OnSolutionOpened()
        {
            if (!Settings.Default.Collapsing_CollapseSolutionWhenOpened) return;

            OnExecute();
        }

        #endregion Methods
    }
}