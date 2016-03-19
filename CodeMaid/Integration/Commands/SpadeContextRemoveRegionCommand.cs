#region CodeMaid is Copyright 2007-2016 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2016 Steve Cadwallader.

using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using System.ComponentModel.Design;
using System.Linq;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for removing a region within Spade.
    /// </summary>
    internal class SpadeContextRemoveRegionCommand : BaseCommand
    {
        #region Fields

        private readonly RemoveRegionLogic _removeRegionLogic;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeContextRemoveRegionCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal SpadeContextRemoveRegionCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(GuidList.GuidCodeMaidCommandSpadeContextRemoveRegion, PackageIds.CmdIDCodeMaidSpadeContextRemoveRegion))
        {
            _removeRegionLogic = RemoveRegionLogic.GetInstance(package);
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
                visible = spade.SelectedItems.OfType<CodeItemRegion>().Any(IsRemoveableRegion);
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
                var regions = spade.SelectedItems.OfType<CodeItemRegion>().Where(IsRemoveableRegion);
                _removeRegionLogic.RemoveRegions(regions);

                spade.Refresh();
            }
        }

        #endregion BaseCommand Methods

        #region Methods

        /// <summary>
        /// Determines if the specified region is a candidate for removal.
        /// </summary>
        /// <param name="region">The region.</param>
        /// <returns>True if the region can be removed, otherwise false.</returns>
        private static bool IsRemoveableRegion(CodeItemRegion region)
        {
            return !region.IsPseudoGroup && region.StartLine > 0 && region.EndLine > 0;
        }

        #endregion Methods
    }
}