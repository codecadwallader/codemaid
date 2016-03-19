using SteveCadwallader.CodeMaid.Model.CodeTree;
using System.ComponentModel.Design;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for setting Spade to type sort order.
    /// </summary>
    internal class SpadeSortOrderTypeCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeSortOrderTypeCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal SpadeSortOrderTypeCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(PackageGuids.GuidCodeMaidCommandSpadeSortOrderType, PackageIds.CmdIDCodeMaidSpadeSortOrderType))
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

        #endregion BaseCommand Methods
    }
}