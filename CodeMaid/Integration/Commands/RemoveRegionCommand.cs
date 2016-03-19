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
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Model;
using System.ComponentModel.Design;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for removing region(s).
    /// </summary>
    internal class RemoveRegionCommand : BaseCommand
    {
        #region Fields

        private readonly CodeModelHelper _codeModelHelper;
        private readonly RemoveRegionLogic _removeRegionLogic;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveRegionCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal RemoveRegionCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(PackageGuids.GuidCodeMaidCommandRemoveRegion, PackageIds.CmdIDCodeMaidRemoveRegion))
        {
            _codeModelHelper = CodeModelHelper.GetInstance(package);
            _removeRegionLogic = RemoveRegionLogic.GetInstance(package);
        }

        #endregion Constructors

        #region Enumerations

        /// <summary>
        /// An enumeration of region command scopes.
        /// </summary>
        private enum RegionCommandScope
        {
            None,
            Document,
            CurrentLine,
            Selection
        }

        #endregion Enumerations

        #region BaseCommand Methods

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            var regionCommandScope = GetRegionCommandScope();

            Enabled = regionCommandScope != RegionCommandScope.None;

            switch (regionCommandScope)
            {
                case RegionCommandScope.CurrentLine:
                    Text = "&Remove Current Region";
                    break;

                case RegionCommandScope.Selection:
                    Text = "&Remove Selected Regions";
                    break;

                default:
                    Text = "&Remove All Regions";
                    break;
            }
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            var regionCommandScope = GetRegionCommandScope();
            switch (regionCommandScope)
            {
                case RegionCommandScope.CurrentLine:
                    _removeRegionLogic.RemoveRegion(_codeModelHelper.RetrieveCodeRegionUnderCursor(ActiveTextDocument));
                    break;

                case RegionCommandScope.Selection:
                    _removeRegionLogic.RemoveRegions(ActiveTextDocument.Selection);
                    break;

                case RegionCommandScope.Document:
                    _removeRegionLogic.RemoveRegions(ActiveTextDocument);
                    break;
            }
        }

        #endregion BaseCommand Methods

        #region Private Properties

        /// <summary>
        /// Gets the active text document, otherwise null.
        /// </summary>
        private TextDocument ActiveTextDocument
        {
            get
            {
                var document = Package.ActiveDocument;

                return document != null ? document.GetTextDocument() : null;
            }
        }

        #endregion Private Properties

        #region Private Methods

        /// <summary>
        /// Gets the region command scope based on the current document and selection conditions.
        /// </summary>
        /// <returns>The scope that should be used for the region command.</returns>
        private RegionCommandScope GetRegionCommandScope()
        {
            if (_removeRegionLogic.CanRemoveRegions(Package.ActiveDocument))
            {
                var activeTextDocument = ActiveTextDocument;
                if (activeTextDocument != null)
                {
                    var textSelection = activeTextDocument.Selection;
                    if (textSelection != null)
                    {
                        if (!textSelection.IsEmpty)
                        {
                            return RegionCommandScope.Selection;
                        }

                        if (_codeModelHelper.IsCodeRegionUnderCursor(ActiveTextDocument))
                        {
                            return RegionCommandScope.CurrentLine;
                        }
                    }

                    return RegionCommandScope.Document;
                }
            }

            return RegionCommandScope.None;
        }

        #endregion Private Methods
    }
}