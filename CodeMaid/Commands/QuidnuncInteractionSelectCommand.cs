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
using SteveCadwallader.CodeMaid.Quidnunc;

namespace SteveCadwallader.CodeMaid.Commands
{
    /// <summary>
    /// A command that provides for setting quidnunc to selection interaction mode.
    /// </summary>
    internal class QuidnuncInteractionSelectCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="QuidnuncInteractionSelectCommand"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal QuidnuncInteractionSelectCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(GuidList.GuidCodeMaidCommandQuidnuncInteractionSelect, (int)PkgCmdIDList.CmdIDCodeMaidQuidnuncInteractionSelect))
        {
        }

        #endregion Constructors

        #region BaseCommand Methods

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            var quidnunc = Package.Quidnunc;
            if (quidnunc != null)
            {
                quidnunc.InteractionMode = QuidnuncInteractionMode.Select;
            }
        }

        #endregion BaseCommand Methods
    }
}