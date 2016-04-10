using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.UI.Dialogs.CleanupProgress;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for cleaning up code in the selected documents.
    /// </summary>
    internal class CleanupSelectedCodeCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupSelectedCodeCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal CleanupSelectedCodeCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(PackageGuids.GuidCodeMaidCommandCleanupSelectedCode, PackageIds.CmdIDCodeMaidCleanupSelectedCode))
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
            Enabled = Package.IDE.Solution.IsOpen;
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            using (new ActiveDocumentRestorer(Package))
            {
                var viewModel = new CleanupProgressViewModel(Package, SelectedProjectItems);
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
        /// Gets the list of selected project items.
        /// </summary>
        private IEnumerable<ProjectItem> SelectedProjectItems
        {
            get { return SolutionHelper.GetSelectedProjectItemsRecursively(Package).Where(x => CodeCleanupAvailabilityLogic.CanCleanupProjectItem(x)); }
        }

        #endregion Private Properties
    }
}