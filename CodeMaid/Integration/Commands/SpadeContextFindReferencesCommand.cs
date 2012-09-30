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

using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell.Interop;
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
            if (spade != null)
            {
                var item = spade.SelectedItem as BaseCodeItemElement;
                if (item != null)
                {
                    var vsSymbolScopeAll = new Guid(SymbolScopeGuids80.Solution);
                    var searchCriteria = new[]
                                             {
                                                 new VSOBSEARCHCRITERIA2
                                                     {
                                                         eSrchType = VSOBSEARCHTYPE.SO_ENTIREWORD,
                                                         grfOptions = (uint)_VSOBSEARCHOPTIONS.VSOBSO_CASESENSITIVE,
                                                         szName = item.CodeElement.FullName
                                                     }
                                             };

                    Package.FindSymbolService.DoSearch(vsSymbolScopeAll, searchCriteria);
                }
            }
        }

        #endregion BaseCommand Methods
    }
}