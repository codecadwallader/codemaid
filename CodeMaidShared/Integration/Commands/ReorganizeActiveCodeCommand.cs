using SteveCadwallader.CodeMaid.Logic.Reorganizing;
using Task = System.Threading.Tasks.Task;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for reorganizing code in the active document.
    /// </summary>
    internal sealed class ReorganizeActiveCodeCommand : BaseCommand
    {
        private readonly CodeReorganizationAvailabilityLogic _codeReorganizationAvailabilityLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReorganizeActiveCodeCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal ReorganizeActiveCodeCommand(CodeMaidPackage package)
            : base(package, PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidReorganizeActiveCode)
        {
            CodeReorganizationManager = CodeReorganizationManager.GetInstance(Package);

            _codeReorganizationAvailabilityLogic = CodeReorganizationAvailabilityLogic.GetInstance(Package);
        }

        /// <summary>
        /// A singleton instance of this command.
        /// </summary>
        public static ReorganizeActiveCodeCommand Instance { get; private set; }

        /// <summary>
        /// Gets the code reorganization manager.
        /// </summary>
        private CodeReorganizationManager CodeReorganizationManager { get; }

        /// <summary>
        /// Initializes a singleton instance of this command.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>A task.</returns>
        public static async Task InitializeAsync(CodeMaidPackage package)
        {
            Instance = new ReorganizeActiveCodeCommand(package);
            await package.SettingsMonitor.WatchAsync(s => s.Feature_ReorganizeActiveCode, Instance.SwitchAsync);
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

            CodeReorganizationManager.Reorganize(Package.ActiveDocument);
        }
    }
}