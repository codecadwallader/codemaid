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

using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// The base implementation of a command.
    /// </summary>
    internal abstract class BaseCommand : OleMenuCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCommand"/> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <param name="id">The id for the command.</param>
        protected BaseCommand(CodeMaidPackage package, CommandID id)
            : base(BaseCommand_Execute, id)
        {
            Package = package;

            BeforeQueryStatus += BaseCommand_BeforeQueryStatus;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the hosting package.
        /// </summary>
        protected CodeMaidPackage Package { get; private set; }

        #endregion Properties

        #region Event Handlers

        /// <summary>
        /// Handles the BeforeQueryStatus event of the BaseCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private static void BaseCommand_BeforeQueryStatus(object sender, EventArgs e)
        {
            BaseCommand command = sender as BaseCommand;
            if (command != null)
            {
                command.OnBeforeQueryStatus();
            }
        }

        /// <summary>
        /// Handles the Execute event of the BaseCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private static void BaseCommand_Execute(object sender, EventArgs e)
        {
            BaseCommand command = sender as BaseCommand;
            if (command != null)
            {
                command.OnExecute();
            }
        }

        #endregion Event Handlers

        #region Methods

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected virtual void OnBeforeQueryStatus()
        {
            // By default, commands are always enabled.
            Enabled = true;
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected abstract void OnExecute();

        #endregion Methods
    }
}