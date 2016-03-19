using SteveCadwallader.CodeMaid.UI.Dialogs.About;
using System.ComponentModel.Design;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for launching the CodeMaid about window.
    /// </summary>
    internal class AboutCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal AboutCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(PackageGuids.GuidCodeMaidCommandAbout, PackageIds.CmdIDCodeMaidAbout))
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

            new AboutWindow().ShowModal();
        }

        #endregion BaseCommand Methods
    }
}