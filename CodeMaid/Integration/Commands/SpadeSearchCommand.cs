using System.ComponentModel.Design;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for setting focus on the search bar in the Spade tool window.
    /// </summary>
    internal class SpadeSearchCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeSearchCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal SpadeSearchCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(PackageGuids.GuidCodeMaidCommandSpadeSearch, PackageIds.CmdIDCodeMaidSpadeSearch))
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
                spade.SearchHost.Activate();
            }
        }

        #endregion BaseCommand Methods
    }
}