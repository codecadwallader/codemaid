#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using System.ComponentModel.Design;
using SteveCadwallader.CodeMaid.UI.Dialogs.Options;
using SteveCadwallader.CodeMaid.UI.Dialogs.Options.Digging;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for launching the CodeMaid configuration to the Spade page.
    /// </summary>
    internal class SpadeConfigurationCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeConfigurationCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal SpadeConfigurationCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(GuidList.GuidCodeMaidCommandSpadeConfiguration, (int)PkgCmdIDList.CmdIDCodeMaidSpadeConfiguration))
        {
        }

        #endregion Constructors

        #region BaseCommand Methods

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            new OptionsWindow { DataContext = new OptionsViewModel(Package, typeof(DiggingViewModel)) }.ShowModal();
        }

        #endregion BaseCommand Methods
    }
}