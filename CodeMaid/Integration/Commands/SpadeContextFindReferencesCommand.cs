#region CodeMaid is Copyright 2007-2013 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2013 Steve Cadwallader.

using System.ComponentModel.Design;
using System.Linq;
using EnvDTE;
using SteveCadwallader.CodeMaid.Model.CodeItems;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for finding references of a member within Spade.
    /// </summary>
    internal class SpadeContextFindReferencesCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeContextFindReferencesCommand"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal SpadeContextFindReferencesCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(GuidList.GuidCodeMaidCommandSpadeContextFindReferences, (int)PkgCmdIDList.CmdIDCodeMaidSpadeContextFindReferences))
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

            Visible = spade != null && spade.SelectedItem is BaseCodeItemElement;
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            var spade = Package.Spade;
            if (spade == null) return;

            var item = spade.SelectedItem as BaseCodeItemElement;
            if (item == null) return;

            var document = Package.IDE.ActiveDocument;
            if (document == null) return;

            var selection = ((TextSelection)document.Selection);

            // Activate the document and set the cursor position to set the command context.
            document.Activate();
            selection.MoveToPoint(item.CodeElement.StartPoint);
            selection.FindText(item.Name, (int)vsFindOptions.vsFindOptionsMatchInHiddenText);
            selection.MoveToPoint(selection.AnchorPoint);

            // Determine if ReSharper FindUsages command is available, otherwise use native VS command.
            if (Package.IDE.Commands.OfType<Command>().Any(x => x.Name == "ReSharper.ReSharper_FindUsages"))
            {
                Package.IDE.ExecuteCommand("ReSharper.ReSharper_FindUsages");
            }
            else
            {
                Package.IDE.ExecuteCommand("Edit.FindAllReferences");
            }
        }

        #endregion BaseCommand Methods
    }
}