#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using EnvDTE;
using SteveCadwallader.CodeMaid.Logic.Reorganizing;
using System.ComponentModel.Design;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for reorganizing code in the active document.
    /// </summary>
    internal class ReorganizeActiveCodeCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReorganizeActiveCodeCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal ReorganizeActiveCodeCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(GuidList.GuidCodeMaidCommandReorganizeActiveCode, (int)PkgCmdIDList.CmdIDCodeMaidReorganizeActiveCode))
        {
            CodeReorderManager = CodeReorderManager.GetInstance(Package);
        }

        #endregion Constructors

        #region BaseCommand Methods

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            Enabled = CodeReorderManager.CanReorganize(ActiveDocument);

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
            base.OnExecute();

            CodeReorderManager.Reorganize(ActiveDocument, false);
        }

        #endregion BaseCommand Methods

        #region Private Properties

        /// <summary>
        /// Gets the active document.
        /// </summary>
        private Document ActiveDocument { get { return Package.IDE.ActiveDocument; } }

        /// <summary>
        /// Gets or sets the code reorder manager.
        /// </summary>
        private CodeReorderManager CodeReorderManager { get; set; }

        #endregion Private Properties
    }
}