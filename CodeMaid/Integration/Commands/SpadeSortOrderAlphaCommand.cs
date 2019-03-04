using SteveCadwallader.CodeMaid.Model.CodeTree;
using System.Threading.Tasks;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for setting Spade to alphabetical sort order.
    /// </summary>
    internal sealed class SpadeSortOrderAlphaCommand : BaseCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeSortOrderAlphaCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal SpadeSortOrderAlphaCommand(CodeMaidPackage package)
            : base(package, PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidSpadeSortOrderAlpha)
        {
        }

        /// <summary>
        /// A singleton instance of this command.
        /// </summary>
        public static SpadeSortOrderAlphaCommand Instance { get; private set; }

        /// <summary>
        /// Initializes a singleton instance of this command.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>A task.</returns>
        public static async Task InitializeAsync(CodeMaidPackage package)
        {
            Instance = new SpadeSortOrderAlphaCommand(package);
            await Instance.SwitchAsync(on: true);
        }

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            var spade = Package.Spade;
            if (spade != null)
            {
                Checked = spade.SortOrder == CodeSortOrder.Alpha;
            }
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            var spade = Package.Spade;
            if (spade != null)
            {
                spade.SortOrder = CodeSortOrder.Alpha;
            }
        }
    }
}