using Microsoft.VisualStudio.Shell;
using SteveCadwallader.CodeMaid.Helpers;
using System;
using System.ComponentModel.Design;
using Task = System.Threading.Tasks.Task;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// The base implementation of a command.
    /// </summary>
    internal abstract class BaseCommand : OleMenuCommand, ISwitchableFeature
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <param name="menuGroup">The GUID for the command ID.</param>
        /// <param name="commandID">The id for the command ID.</param>
        protected BaseCommand(CodeMaidPackage package, Guid menuGroup, int commandID)
            : base(BaseCommand_Execute, null, BaseCommand_BeforeQueryStatus, new CommandID(menuGroup, commandID))
        {
            Package = package;
        }

        /// <summary>
        /// Gets the hosting package.
        /// </summary>
        protected CodeMaidPackage Package { get; private set; }

        /// <summary>
        /// Switches the command on or off, adding/removing it from the IDE.
        /// </summary>
        /// <param name="on">True if switching the command on, otherwise false.</param>
        /// <returns>A task.</returns>
        public virtual async Task SwitchAsync(bool on)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(Package.DisposalToken);

            if (await Package.GetServiceAsync(typeof(IMenuCommandService)) is OleMenuCommandService commandService)
            {
                if (on && commandService.FindCommand(CommandID) == null)
                {
                    commandService.AddCommand(this);
                }
                else if (!on)
                {
                    commandService.RemoveCommand(this);
                }
            }
        }

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
        protected virtual void OnExecute()
        {
            OutputWindowHelper.DiagnosticWriteLine($"{GetType().Name}.OnExecute invoked");
        }

        /// <summary>
        /// Handles the BeforeQueryStatus event of the BaseCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private static void BaseCommand_BeforeQueryStatus(object sender, EventArgs e)
        {
            var command = sender as BaseCommand;
            command?.OnBeforeQueryStatus();
        }

        /// <summary>
        /// Handles the Execute event of the BaseCommand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private static void BaseCommand_Execute(object sender, EventArgs e)
        {
            var command = sender as BaseCommand;
            command?.OnExecute();
        }
    }
}