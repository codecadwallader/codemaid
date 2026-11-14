using SteveCadwallader.CodeMaid.UI.Dialogs.About;
using System.Threading.Tasks;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for launching the CodeMaid about window.
    /// </summary>
    internal sealed class AboutCommand : BaseCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        private AboutCommand(CodeMaidPackage package)
            : base(package, PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidAbout)
        {
        }

        /// <summary>
        /// A singleton instance of this command.
        /// </summary>
        public static AboutCommand Instance { get; private set; }

        /// <summary>
        /// Initializes a singleton instance of this command.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>A task.</returns>
        public static async Task InitializeAsync(CodeMaidPackage package)
        {
            Instance = new AboutCommand(package);
            await Instance.SwitchAsync(on: true);
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            new AboutWindow().ShowModal();
        }
    }
}