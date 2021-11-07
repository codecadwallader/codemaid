using SteveCadwallader.CodeMaid.UI.Dialogs.Options;
using System.Threading.Tasks;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for launching the CodeMaid Options to the general cleanup page.
    /// </summary>
    internal sealed class OptionsCommand : BaseCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal OptionsCommand(CodeMaidPackage package)
            : base(package, PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidOptions)
        {
        }

        /// <summary>
        /// A singleton instance of this command.
        /// </summary>
        public static OptionsCommand Instance { get; private set; }

        /// <summary>
        /// Initializes a singleton instance of this command.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>A task.</returns>
        public static async Task InitializeAsync(CodeMaidPackage package)
        {
            Instance = new OptionsCommand(package);
            await Instance.SwitchAsync(on: true);
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            new OptionsWindow { DataContext = new OptionsViewModel(Package) }.ShowModal();
        }
    }
}