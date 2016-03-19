using System.ComponentModel.Design;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for refreshing the Spade tool window.
    /// </summary>
    internal class SpadeRefreshCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeRefreshCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal SpadeRefreshCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(PackageGuids.GuidCodeMaidCommandSpadeRefresh, PackageIds.CmdIDCodeMaidSpadeRefresh))
        {
        }

        #endregion Constructors

        #region BaseCommand Methods

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            var spade = Package.Spade;
            if (spade != null)
            {
                spade.Refresh();
            }
        }

        #endregion BaseCommand Methods
    }
}