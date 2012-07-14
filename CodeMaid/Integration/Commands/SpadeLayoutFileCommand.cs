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
using SteveCadwallader.CodeMaid.Model.CodeTree;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for setting Spade to file layout mode.
    /// </summary>
    internal class SpadeLayoutFileCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeLayoutFileCommand"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal SpadeLayoutFileCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(GuidList.GuidCodeMaidCommandSpadeLayoutFile, (int)PkgCmdIDList.CmdIDCodeMaidSpadeLayoutFile))
        {
        }

        #endregion Constructors

        #region BaseCommand Methods

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            var spade = Package.Spade;
            if (spade != null)
            {
                Checked = spade.LayoutMode == TreeLayoutMode.FileLayout;
            }
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            var spade = Package.Spade;
            if (spade != null)
            {
                spade.LayoutMode = TreeLayoutMode.FileLayout;
            }
        }

        #endregion BaseCommand Methods
    }
}