using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using System.ComponentModel.Design;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for launching the Spade tool window.
    /// </summary>
    internal class SpadeToolWindowCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeToolWindowCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal SpadeToolWindowCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(PackageGuids.GuidCodeMaidCommandSpadeToolWindow, PackageIds.CmdIDCodeMaidSpadeToolWindow))
        {
        }

        #endregion Constructors

        #region BaseCommand Methods

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            var spade = Package.SpadeForceLoad;
            if (spade != null)
            {
                var spadeFrame = spade.Frame as IVsWindowFrame;
                if (spadeFrame != null)
                {
                    spadeFrame.Show();
                }
            }
        }

        #endregion BaseCommand Methods

        #region Internal Methods

        /// <summary>
        /// Called when a document has been saved.
        /// </summary>
        /// <param name="document">The document that was saved.</param>
        internal void OnAfterDocumentSave(Document document)
        {
            var spade = Package.Spade;
            if (spade != null)
            {
                spade.NotifyDocumentSave(document);
            }
        }

        /// <summary>
        /// Called when a window change has occurred, potentially to be used by the Spade tool window.
        /// </summary>
        /// <param name="document">The document that got focus, may be null.</param>
        internal void OnWindowChange(Document document)
        {
            var spade = Package.Spade;
            if (spade != null)
            {
                spade.NotifyActiveDocument(document);
            }
        }

        #endregion Internal Methods
    }
}