using SteveCadwallader.CodeMaid.Model.CodeTree;
using System.Threading.Tasks;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for setting Spade to type sort order.
    /// </summary>
    internal sealed class SpadeSortOrderTypeCommand : BaseCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeSortOrderTypeCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal SpadeSortOrderTypeCommand(CodeMaidPackage package)
            : base(package, PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidSpadeSortOrderType)
        {
        }

        /// <summary>
        /// A singleton instance of this command.
        /// </summary>
        public static SpadeSortOrderTypeCommand Instance { get; private set; }

        /// <summary>
        /// Initializes a singleton instance of this command.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>A task.</returns>
        public static Task InitializeAsync(CodeMaidPackage package)
        {
            Instance = new SpadeSortOrderTypeCommand(package);
            return Instance.SwitchAsync(on: true);
        }

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            var spade = Package.Spade;
            if (spade != null)
            {
                Checked = spade.SortOrder == CodeSortOrder.Type;
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
                spade.SortOrder = CodeSortOrder.Type;
            }
        }
    }
}