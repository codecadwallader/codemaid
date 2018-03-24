using SteveCadwallader.CodeMaid.Model.CodeTree;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for setting Spade to file sort order.
    /// </summary>
    internal sealed class SpadeSortOrderFileCommand : BaseCommand
    {
        #region Singleton

        public static SpadeSortOrderFileCommand Instance { get; private set; }

        public static void Initialize(CodeMaidPackage package)
        {
            Instance = new SpadeSortOrderFileCommand(package);
            Instance.Switch(on: true);
        }

        #endregion Singleton

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeSortOrderFileCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal SpadeSortOrderFileCommand(CodeMaidPackage package)
            : base(package, PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidSpadeSortOrderFile)
        {
        }

        #endregion Constructors

        #region BaseCommand Methods

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            var spade = Package.Spade;
            if (spade != null)
            {
                Checked = spade.SortOrder == CodeSortOrder.File;
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
                spade.SortOrder = CodeSortOrder.File;
            }
        }

        #endregion BaseCommand Methods
    }
}