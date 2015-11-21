#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Logic.Reorganizing;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using SteveCadwallader.CodeMaid.Properties;
using System.ComponentModel.Design;
using System.Linq;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for inserting a region within Spade.
    /// </summary>
    internal class SpadeContextInsertRegionCommand : BaseCommand
    {
        #region Fields

        private readonly GenerateRegionLogic _generateRegionLogic;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeContextInsertRegionCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal SpadeContextInsertRegionCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(GuidList.GuidCodeMaidCommandSpadeContextInsertRegion, (int)PkgCmdIDList.CmdIDCodeMaidSpadeContextInsertRegion))
        {
            _generateRegionLogic = GenerateRegionLogic.GetInstance(package);
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
            if (spade?.Document != null)
            {
                visible = spade.Document.Language == "CSharp" &&
                          spade.SelectedItems.Count() >= 2;
            }

            Visible = visible;
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            var spade = Package.Spade;
            if (spade != null)
            {
                var region = new CodeItemRegion { Name = "New Region" };
                var startPoint = spade.SelectedItems.OrderBy(x => x.StartOffset).First().StartPoint;
                var endPoint = spade.SelectedItems.OrderBy(x => x.EndOffset).Last().EndPoint;

                // Create the new region.
                _generateRegionLogic.InsertEndRegionTag(region, endPoint);
                _generateRegionLogic.InsertRegionTag(region, startPoint);

                // Move to that element.
                TextDocumentHelper.MoveToCodeItem(spade.Document, region, Settings.Default.Digging_CenterOnWhole);

                // Highlight the line of text for renaming.
                var textDocument = spade.Document.GetTextDocument();
                textDocument.Selection.EndOfLine(true);
                textDocument.Selection.SwapAnchor();

                spade.Refresh();
            }
        }

        #endregion BaseCommand Methods
    }
}