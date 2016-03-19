using SteveCadwallader.CodeMaid.Model.CodeTree;
using System.ComponentModel.Design;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for setting Spade to alphabetical sort order.
    /// </summary>
    internal class SpadeSortOrderAlphaCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeSortOrderAlphaCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal SpadeSortOrderAlphaCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(PackageGuids.GuidCodeMaidCommandSpadeSortOrderAlpha, PackageIds.CmdIDCodeMaidSpadeSortOrderAlpha))
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

        #endregion BaseCommand Methods
    }
}