using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;
using System.Threading.Tasks;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for cleaning up code in the active document.
    /// </summary>
    internal sealed class CleanupActiveCodeCommand : BaseCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupActiveCodeCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal CleanupActiveCodeCommand(CodeMaidPackage package)
            : base(package, PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidCleanupActiveCode)
        {
            CodeCleanupAvailabilityLogic = CodeCleanupAvailabilityLogic.GetInstance(Package);
            CodeCleanupManager = CodeCleanupManager.GetInstance(Package);
        }

        /// <summary>
        /// A singleton instance of this command.
        /// </summary>
        public static CleanupActiveCodeCommand Instance { get; private set; }

        /// <summary>
        /// Gets the code cleanup availability logic.
        /// </summary>
        private CodeCleanupAvailabilityLogic CodeCleanupAvailabilityLogic { get; }

        /// <summary>
        /// Gets the code cleanup manager.
        /// </summary>
        private CodeCleanupManager CodeCleanupManager { get; }

        /// <summary>
        /// Initializes a singleton instance of this command.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>A task.</returns>
        public static async Task InitializeAsync(CodeMaidPackage package)
        {
            Instance = new CleanupActiveCodeCommand(package);
            await package.SettingsMonitor.WatchAsync(s => s.Feature_CleanupActiveCode, Instance.SwitchAsync);
        }

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
    }
}