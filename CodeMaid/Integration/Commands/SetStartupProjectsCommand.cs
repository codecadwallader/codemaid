#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using SteveCadwallader.CodeMaid.Logic.Starting;
using System.ComponentModel.Design;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for setting the startup projects.
    /// </summary>
    internal class SetStartupProjectsCommand : BaseCommand
    {
        #region Fields

        private readonly StartupProjectsLogic _startupProjectsLogic;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SetStartupProjectsCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal SetStartupProjectsCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(GuidList.GuidCodeMaidCommandSetStartupProjects, (int)PkgCmdIDList.CmdIDCodeMaidSetStartupProjects))
        {
            _startupProjectsLogic = StartupProjectsLogic.GetInstance(package);
        }

        #endregion Constructors

        #region BaseCommand Methods

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            Enabled = Package.IDE.Solution.IsOpen;
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            _startupProjectsLogic.GetStartupProjects();
        }

        #endregion BaseCommand Methods
    }
}