#region CodeMaid is Copyright 2007-2016 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2016 Steve Cadwallader.

using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using System.ComponentModel.Design;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for collapsing nodes in the solution explorer tool window.
    /// </summary>
    internal class CollapseAllSolutionExplorerCommand : BaseCommand
    {
        #region Fields

        /// <summary>
        /// A flag indicating if waiting to execute.
        /// </summary>
        private bool _isWaitingToExecute;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CollapseAllSolutionExplorerCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal CollapseAllSolutionExplorerCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(GuidList.GuidCodeMaidCommandCollapseAllSolutionExplorer, PackageIds.CmdIDCodeMaidCollapseAllSolutionExplorer))
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

            if (Enabled && _isWaitingToExecute)
            {
                OnExecute();
            }
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            _isWaitingToExecute = false;

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

            var topItem = TopUIHierarchyItem;
            if (topItem == null || topItem.UIHierarchyItems.Count == 0)
            {
                _isWaitingToExecute = true;
            }
            else
            {
                OnExecute();
            }
        }

        #endregion Methods
    }
}