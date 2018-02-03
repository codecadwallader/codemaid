using SteveCadwallader.CodeMaid.UI.Dialogs.Options;
using SteveCadwallader.CodeMaid.UI.Dialogs.Options.Digging;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for launching the CodeMaid Options to the Spade page.
    /// </summary>
    internal sealed class SpadeOptionsCommand : BaseCommand
    {
        #region Singleton

        public static SpadeOptionsCommand Instance { get; private set; }

        public static void Initialize(CodeMaidPackage package)
        {
            Instance = new SpadeOptionsCommand(package);
            Instance.Switch(on: true);
        }

        #endregion Singleton

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeOptionsCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal SpadeOptionsCommand(CodeMaidPackage package)
            : base(package, PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidSpadeOptions)
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