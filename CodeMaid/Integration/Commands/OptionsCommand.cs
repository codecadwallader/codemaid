using SteveCadwallader.CodeMaid.UI.Dialogs.Options;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for launching the CodeMaid Options to the general cleanup page.
    /// </summary>
    internal sealed class OptionsCommand : BaseCommand
    {
        #region Singleton

        public static OptionsCommand Instance { get; private set; }

        public static void Initialize(CodeMaidPackage package)
        {
            Instance = new OptionsCommand(package);
            Instance.Switch(on: true);
        }

        #endregion Singleton

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal OptionsCommand(CodeMaidPackage package)
            : base(package, PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidOptions)
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

            new OptionsWindow { DataContext = new OptionsViewModel(Package) }.ShowModal();
        }

        #endregion BaseCommand Methods
    }
}