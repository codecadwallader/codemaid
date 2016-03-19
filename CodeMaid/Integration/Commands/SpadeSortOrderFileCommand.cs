using SteveCadwallader.CodeMaid.Model.CodeTree;
using System.ComponentModel.Design;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for setting Spade to file sort order.
    /// </summary>
    internal class SpadeSortOrderFileCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeSortOrderFileCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal SpadeSortOrderFileCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(PackageGuids.GuidCodeMaidCommandSpadeSortOrderFile, PackageIds.CmdIDCodeMaidSpadeSortOrderFile))
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