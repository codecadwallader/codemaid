using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;
using System.ComponentModel.Design;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for cleaning up code in the active document.
    /// </summary>
    internal class CleanupActiveCodeCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupActiveCodeCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal CleanupActiveCodeCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(PackageGuids.GuidCodeMaidCommandCleanupActiveCode, PackageIds.CmdIDCodeMaidCleanupActiveCode))
        {
            CodeCleanupAvailabilityLogic = CodeCleanupAvailabilityLogic.GetInstance(Package);
            CodeCleanupManager = CodeCleanupManager.GetInstance(Package);
        }

        #endregion Constructors

        #region BaseCommand Members

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

            CodeCleanupManager.Cleanup(Package.ActiveDocument);
        }

        #endregion BaseCommand Members

        #region Internal Methods

        /// <summary>
        /// Called before a document is saved in order to potentially run code cleanup.
        /// </summary>
        /// <param name="document">The document about to be saved.</param>
        internal void OnBeforeDocumentSave(Document document)
        {
            if (!Settings.Default.Cleaning_AutoCleanupOnFileSave) return;
            if (!CodeCleanupAvailabilityLogic.CanCleanupDocument(document)) return;

            try
            {
                Package.IsAutoSaveContext = true;

                using (new ActiveDocumentRestorer(Package))
                {
                    CodeCleanupManager.Cleanup(document);
                }
            }
            finally
            {
                Package.IsAutoSaveContext = false;
            }
        }

        #endregion Internal Methods

        #region Private Properties

        /// <summary>
        /// Gets the code cleanup availability logic.
        /// </summary>
        private CodeCleanupAvailabilityLogic CodeCleanupAvailabilityLogic { get; }

        /// <summary>
        /// Gets the code cleanup manager.
        /// </summary>
        private CodeCleanupManager CodeCleanupManager { get; }

        #endregion Private Properties
    }
}