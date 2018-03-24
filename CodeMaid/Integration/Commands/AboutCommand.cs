using SteveCadwallader.CodeMaid.UI.Dialogs.About;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for launching the CodeMaid about window.
    /// </summary>
    internal sealed class AboutCommand : BaseCommand
    {
        #region Singleton

        public static AboutCommand Instance { get; private set; }

        public static void Initialize(CodeMaidPackage package)
        {
            Instance = new AboutCommand(package);
            Instance.Switch(on: true);
        }

        #endregion Singleton

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal AboutCommand(CodeMaidPackage package)
            : base(package, PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidAbout)
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