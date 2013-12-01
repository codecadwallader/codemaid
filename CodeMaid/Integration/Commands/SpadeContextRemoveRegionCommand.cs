#region CodeMaid is Copyright 2007-2013 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2013 Steve Cadwallader.

using System;
using System.ComponentModel.Design;
using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Model.CodeItems;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for removing a region within Spade.
    /// </summary>
    internal class SpadeContextRemoveRegionCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeContextRemoveRegionCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal SpadeContextRemoveRegionCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(GuidList.GuidCodeMaidCommandSpadeContextRemoveRegion, (int)PkgCmdIDList.CmdIDCodeMaidSpadeContextRemoveRegion))
        {
        }

        #endregion Constructors

        #region BaseCommand Methods

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            bool visible = false;

            var spade = Package.Spade;
            if (spade != null)
            {
                var region = spade.SelectedItem as CodeItemRegion;
                if (region != null)
                {
                    visible = !region.IsPseudoGroup;
                }
            }

            Visible = visible;
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            var spade = Package.Spade;
            if (spade != null)
            {
                var item = spade.SelectedItem;
                if (item != null && item.StartLine > 0 && item.EndLine > 0)
                {
                    new UndoTransactionHelper(Package, "CodeMaid Remove Region " + item.Name).Run(() =>
                    {
                        var end = item.EndPoint.CreateEditPoint();
                        end.StartOfLine();
                        end.Delete(end.LineLength);
                        end.DeleteWhitespace(vsWhitespaceOptions.vsWhitespaceOptionsVertical);
                        end.Insert(Environment.NewLine);

                        var start = item.StartPoint.CreateEditPoint();
                        start.StartOfLine();
                        start.Delete(start.LineLength);
                        start.DeleteWhitespace(vsWhitespaceOptions.vsWhitespaceOptionsVertical);
                        start.Insert(Environment.NewLine);
                    });

                    spade.Refresh();
                }
            }
        }

        #endregion BaseCommand Methods
    }
}