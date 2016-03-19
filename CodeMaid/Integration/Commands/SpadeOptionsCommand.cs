using SteveCadwallader.CodeMaid.UI.Dialogs.Options;
using SteveCadwallader.CodeMaid.UI.Dialogs.Options.Digging;
using System.ComponentModel.Design;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for launching the CodeMaid Options to the Spade page.
    /// </summary>
    internal class SpadeOptionsCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeOptionsCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal SpadeOptionsCommand(CodeMaidPackage package)
            : base(package,
                   new CommandID(PackageGuids.GuidCodeMaidCommandSpadeOptions, PackageIds.CmdIDCodeMaidSpadeOptions))
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

            new OptionsWindow { DataContext = new OptionsViewModel(Package, typeof(DiggingViewModel)) }.ShowModal();
        }

        #endregion BaseCommand Methods
    }
}