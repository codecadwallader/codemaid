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

using System;
using System.ComponentModel.Design;
using EnvDTE;

namespace SteveCadwallader.CodeMaid.Commands
{
    /// <summary>
    /// A command that provides for finding a file in the solution explorer.
    /// </summary>
    internal class FindInSolutionExplorerCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FindInSolutionExplorerCommand"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal FindInSolutionExplorerCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(GuidList.GuidCodeMaidCommandFindInSolutionExplorer, (int)PkgCmdIDList.CmdIDCodeMaidFindInSolutionExplorer))
        {
        }

        #endregion Constructors

        #region BaseCommand Methods

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            Enabled = Package.IDE.ActiveDocument != null;
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            Document document = Package.IDE.ActiveDocument;
            if (document != null)
            {
                Package.IDE.ExecuteCommand("View.TrackActivityinSolutionExplorer", String.Empty);
                Package.IDE.ExecuteCommand("View.TrackActivityinSolutionExplorer", String.Empty);
                Package.IDE.ExecuteCommand("View.SolutionExplorer", String.Empty);
            }
        }

        #endregion BaseCommand Methods
    }
}