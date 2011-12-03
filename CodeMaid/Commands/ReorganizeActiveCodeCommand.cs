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

using System.ComponentModel.Design;
using EnvDTE;

namespace SteveCadwallader.CodeMaid.Commands
{
    /// <summary>
    /// A command that provides for reorganizing code in the active document.
    /// </summary>
    internal class ReorganizeActiveCodeCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReorganizeActiveCodeCommand"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal ReorganizeActiveCodeCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(GuidList.GuidCodeMaidCommandReorganizeActiveCode, (int)PkgCmdIDList.CmdIDCodeMaidReorganizeActiveCode))
        {
        }

        #endregion Constructors

        #region BaseCommand Methods

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            Enabled = false;

            if (Enabled)
            {
                Text = "Reorgani&ze " + ActiveDocument.Name;
            }
            else
            {
                Text = "Reorgani&ze Code";
            }
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
        }

        #endregion BaseCommand Methods

        #region Private Properties

        /// <summary>
        /// Gets the active document.
        /// </summary>
        private Document ActiveDocument { get { return Package.IDE.ActiveDocument; } }

        #endregion Private Properties
    }
}