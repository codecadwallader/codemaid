using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using System;
using System.ComponentModel.Design;
using System.IO;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for toggling the read only attribute of a file.
    /// </summary>
    internal class ReadOnlyToggleCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyToggleCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal ReadOnlyToggleCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(PackageGuids.GuidCodeMaidCommandReadOnlyToggle, PackageIds.CmdIDCodeMaidReadOnlyToggle))
        {
        }

        #endregion Constructors

        #region BaseCommand Methods

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            Enabled = Package.ActiveDocument != null;
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            Document document = Package.ActiveDocument;
            if (document != null)
            {
                try
                {
                    FileAttributes originalAttributes = File.GetAttributes(document.FullName);
                    FileAttributes newAttributes = originalAttributes ^ FileAttributes.ReadOnly;

                    File.SetAttributes(document.FullName, newAttributes);
                }
                catch (Exception ex)
                {
                    OutputWindowHelper.ExceptionWriteLine($"Unable to toggle read only state on '{document.FullName}'", ex);
                }
            }
        }

        #endregion BaseCommand Methods
    }
}