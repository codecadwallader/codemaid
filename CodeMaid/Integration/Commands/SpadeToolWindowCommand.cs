using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using System.Threading.Tasks;

namespace SteveCadwallader.CodeMaid.Integration.Commands
{
    /// <summary>
    /// A command that provides for launching the Spade tool window.
    /// </summary>
    internal sealed class SpadeToolWindowCommand : BaseCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpadeToolWindowCommand" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal SpadeToolWindowCommand(CodeMaidPackage package)
            : base(package, PackageGuids.GuidCodeMaidMenuSet, PackageIds.CmdIDCodeMaidSpadeToolWindow)
        {
        }

        /// <summary>
        /// A singleton instance of this command.
        /// </summary>
        public static SpadeToolWindowCommand Instance { get; private set; }

        /// <summary>
        /// Initializes a singleton instance of this command.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>A task.</returns>
        public static async Task InitializeAsync(CodeMaidPackage package)
        {
            Instance = new SpadeToolWindowCommand(package);
            await package.SettingsMonitor.WatchAsync(s => s.Feature_SpadeToolWindow, Instance.SwitchAsync);
        }

        public override async Task SwitchAsync(bool on)
        {
            await base.SwitchAsync(on);

            if (!on)
            {
                Package.Spade?.Close();
            }
        }

        /// <summary>
        /// Called when a document has been saved.
        /// </summary>
        /// <param name="document">The document that was saved.</param>
        internal void OnAfterDocumentSave(Document document)
        {
            var spade = Package.Spade;
            if (spade != null)
            {
                spade.NotifyDocumentSave(document);
            }
        }

        /// <summary>
        /// Called when a window change has occurred, potentially to be used by the Spade tool window.
        /// </summary>
        /// <param name="document">The document that got focus, may be null.</param>
        internal void OnWindowChange(Document document)
        {
            var spade = Package.Spade;
            if (spade != null)
            {
                spade.NotifyActiveDocument(document);
            }
        }

        /// <summary>
        /// Called to execute the command.
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            var spade = Package.SpadeForceLoad;
            if (spade?.Frame is IVsWindowFrame spadeFrame)
            {
                spadeFrame.Show();
            }
        }
    }
}