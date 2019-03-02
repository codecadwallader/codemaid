using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;
using SteveCadwallader.CodeMaid.UI.Dialogs.CleanupProgress;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for cleaning up code in all documents.
    /// </summary>
    internal sealed class CleanupAllCodeCommand : BaseCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupAllCodeCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal CleanupAllCodeCommand(CodeMaidPackage package)
            : base(package, PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidCleanupAllCode)
        {
            CodeCleanupAvailabilityLogic = CodeCleanupAvailabilityLogic.GetInstance(Package);
        }

        /// <summary>
        /// A singleton instance of this command.
        /// </summary>
        public static CleanupAllCodeCommand Instance { get; private set; }

        /// <summary>
        /// Gets the list of all project items.
        /// </summary>
        private IEnumerable<ProjectItem> AllProjectItems
            => SolutionHelper.GetAllItemsInSolution<ProjectItem>(Package.IDE.Solution).Where(x => CodeCleanupAvailabilityLogic.CanCleanupProjectItem(x));

        /// <summary>
        /// Gets or sets the code cleanup availability logic.
        /// </summary>
        private CodeCleanupAvailabilityLogic CodeCleanupAvailabilityLogic { get; set; }

        /// <summary>
        /// Initializes a singleton instance of this command.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>A task.</returns>
        public static async Task InitializeAsync(CodeMaidPackage package)
        {
            Instance = new CleanupAllCodeCommand(package);
            await package.SettingsMonitor.WatchAsync(s => s.Feature_CleanupAllCode, Instance.SwitchAsync);
        }

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

            if (!CodeCleanupAvailabilityLogic.IsCleanupEnvironmentAvailable())
            {
                MessageBox.Show(Resources.CleanupCannotRunWhileDebugging,
                                Resources.CodeMaidCleanupAllCode,
                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (MessageBox.Show(Resources.AreYouReadyForCodeMaidToCleanEverythingInTheSolution,
                                     Resources.CodeMaidConfirmationForCleanupAllCode,
                                     MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No)
                         == MessageBoxResult.Yes)
            {
                using (new ActiveDocumentRestorer(Package))
                {
                    var viewModel = new CleanupProgressViewModel(Package, AllProjectItems);
                    var window = new CleanupProgressWindow { DataContext = viewModel };

                    window.ShowModal();
                }
            }
        }
    }
}