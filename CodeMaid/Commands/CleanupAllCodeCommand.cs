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

using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Windows.Forms;
using EnvDTE;
using SteveCadwallader.CodeMaid.Dialogs;
using SteveCadwallader.CodeMaid.Helpers;

namespace SteveCadwallader.CodeMaid.Commands
{
    /// <summary>
    /// A command that provides for cleaning up code in all documents.
    /// </summary>
    internal class CleanupAllCodeCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupAllCodeCommand"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal CleanupAllCodeCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(GuidList.GuidCodeMaidCommandCleanupAllCode, (int)PkgCmdIDList.CmdIDCodeMaidCleanupAllCode))
        {
            CodeCleanupAvailabilityHelper = CodeCleanupAvailabilityHelper.GetInstance(Package);
        }

        #endregion Constructors

        #region BaseCommand Members

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
            if (!CodeCleanupAvailabilityHelper.IsCleanupEnvironmentAvailable())
            {
                MessageBox.Show(@"Cleanup cannot run while debugging.",
                                @"CodeMaid: Cleanup All Code",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (MessageBox.Show(@"Are you ready for CodeMaid to clean everything in the solution?",
                                     @"CodeMaid: Confirmation for Cleanup All Code",
                                     MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                         == DialogResult.Yes)
            {
                using (new ActiveDocumentRestorer(Package))
                {
                    new CleanupProgressWindow(Package, AllProjectItems).ShowDialog();
                }
            }
        }

        #endregion BaseCommand Members

        #region Private Properties

        /// <summary>
        /// Gets the list of all project items.
        /// </summary>
        private IEnumerable<ProjectItem> AllProjectItems
        {
            get { return SolutionHelper.GetAllProjectItemsInSolution(Package).Where(x => CodeCleanupAvailabilityHelper.ShouldCleanup(x)); }
        }

        /// <summary>
        /// Gets or sets the code cleanup availability helper.
        /// </summary>
        private CodeCleanupAvailabilityHelper CodeCleanupAvailabilityHelper { get; set; }

        #endregion Private Properties
    }
}