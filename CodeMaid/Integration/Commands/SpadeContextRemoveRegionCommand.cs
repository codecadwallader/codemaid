using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using System.Linq;
using System.Threading.Tasks;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for removing a region within Spade.
    /// </summary>
    internal sealed class SpadeContextRemoveRegionCommand : BaseCommand
    {
        private readonly RemoveRegionLogic _removeRegionLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeContextRemoveRegionCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal SpadeContextRemoveRegionCommand(CodeMaidPackage package)
            : base(package, PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidSpadeContextRemoveRegion)
        {
            _removeRegionLogic = RemoveRegionLogic.GetInstance(package);
        }

        /// <summary>
        /// A singleton instance of this command.
        /// </summary>
        public static SpadeContextRemoveRegionCommand Instance { get; private set; }

        /// <summary>
        /// Initializes a singleton instance of this command.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>A task.</returns>
        public static async Task InitializeAsync(CodeMaidPackage package)
        {
            Instance = new SpadeContextRemoveRegionCommand(package);
            await Instance.SwitchAsync(on: true);
        }

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            bool visible = false;

            var spade = Package.Spade;
            if (spade != null)
            {
                visible = spade.SelectedItems.OfType<CodeItemRegion>().Any(IsRemoveableRegion);
            }

            Visible = visible;
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
                var regions = spade.SelectedItems.OfType<CodeItemRegion>().Where(IsRemoveableRegion);
                _removeRegionLogic.RemoveRegions(regions);

                spade.Refresh();
            }
        }

        /// <summary>
        /// Determines if the specified region is a candidate for removal.
        /// </summary>
        /// <param name="region">The region.</param>
        /// <returns>True if the region can be removed, otherwise false.</returns>
        private static bool IsRemoveableRegion(CodeItemRegion region)
        {
            return !region.IsPseudoGroup && region.StartLine > 0 && region.EndLine > 0;
        }
    }
}