using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.UI.Dialogs.CleanupProgress;
using System.Collections.Generic;
using System.Linq;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for cleaning up code in the open documents.
    /// </summary>
    internal sealed class CleanupOpenCodeCommand : BaseCommand
    {
        #region Singleton

        public static CleanupOpenCodeCommand Instance { get; private set; }

        public static void Initialize(CodeMaidPackage package)
        {
            Instance = new CleanupOpenCodeCommand(package);
            package.SettingMonitor.Watch(s => s.Feature_CleanupOpenCode, Instance.Switch);
        }

        #endregion Singleton

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupOpenCodeCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal CleanupOpenCodeCommand(CodeMaidPackage package)
            : base(package, PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidCleanupOpenCode)
        {
            CodeCleanupAvailabilityLogic = CodeCleanupAvailabilityLogic.GetInstance(Package);
        }

        #endregion Constructors

        #region BaseCommand Members

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            Enabled = OpenDocuments.Any();
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            using (new ActiveDocumentRestorer(Package))
            {
                var viewModel = new CleanupProgressViewModel(Package, OpenCleanableDocuments);
                var window = new CleanupProgressWindow { DataContext = viewModel };

                window.ShowModal();
            }
        }

        #endregion BaseCommand Members

        #region Private Properties

        /// <summary>
        /// Gets or sets the code cleanup availability logic.
        /// </summary>
        private CodeCleanupAvailabilityLogic CodeCleanupAvailabilityLogic { get; }

        /// <summary>
        /// Gets the list of open documents.
        /// </summary>
        private IEnumerable<Document> OpenDocuments
        {
            get { return Package.IDE.Documents.OfType<Document>().Where(x => x.ActiveWindow != null); }
        }

        /// <summary>
        /// Gets the list of open documents that are cleanup candidates.
        /// </summary>
        private IEnumerable<Document> OpenCleanableDocuments
        {
            get { return OpenDocuments.Where(x => CodeCleanupAvailabilityLogic.CanCleanupDocument(x)); }
        }

        #endregion Private Properties
    }
}