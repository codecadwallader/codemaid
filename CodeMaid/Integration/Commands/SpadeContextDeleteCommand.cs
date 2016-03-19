using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Model.CodeItems;
using System;
using System.ComponentModel.Design;
using System.Linq;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for deleting a member within Spade.
    /// </summary>
    internal class SpadeContextDeleteCommand : BaseCommand
    {
        #region Fields

        private readonly UndoTransactionHelper _undoTransactionHelper;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeContextDeleteCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal SpadeContextDeleteCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(PackageGuids.GuidCodeMaidCommandSpadeContextDelete, PackageIds.CmdIDCodeMaidSpadeContextDelete))
        {
            _undoTransactionHelper = new UndoTransactionHelper(package, "CodeMaid Delete Items");
        }

        #endregion Constructors

        #region BaseCommand Methods

        /// <summary>
        /// Called to update the current status of the command.
        /// </summary>
        protected override void OnBeforeQueryStatus()
        {
            bool visible = false;

            var spade = Package.Spade;
            if (spade != null)
            {
                visible = spade.SelectedItems.Any(IsDeletable);
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
                // Delay the check of start/end points until execution time, to avoid an intermediate state issue.
                var items = spade.SelectedItems.Where(IsDeletable).Where(x => x.StartPoint != null && x.EndPoint != null);

                _undoTransactionHelper.Run(() =>
                {
                    // Iterate through items in reverse order (reduces line number updates during removal).
                    foreach (var item in items.OrderByDescending(x => x.StartLine))
                    {
                        var start = item.StartPoint.CreateEditPoint();

                        start.Delete(item.EndPoint);
                        start.DeleteWhitespace(vsWhitespaceOptions.vsWhitespaceOptionsVertical);
                        start.Insert(Environment.NewLine);
                    }
                });

                spade.Refresh();
            }
        }

        #endregion BaseCommand Methods

        #region Methods

        /// <summary>
        /// Determines if the specified item is a candidate for deletion.
        /// </summary>
        /// <param name="codeItem">The code item.</param>
        /// <returns>True if the code item can be deleted, otherwise false.</returns>
        private static bool IsDeletable(BaseCodeItem codeItem)
        {
            return !(codeItem is CodeItemRegion) || !((CodeItemRegion)codeItem).IsPseudoGroup;
        }

        #endregion Methods
    }
}