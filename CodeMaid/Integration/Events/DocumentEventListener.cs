using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using System;
using System.Threading.Tasks;

namespace SteveCadwallader.CodeMaid.Integration.Events
{
    /// <summary>
    /// A class that encapsulates listening for document events.
    /// </summary>
    internal sealed class DocumentEventListener : BaseEventListener
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentEventListener" /> class.
        /// </summary>
        /// <param name="package">The package hosting the event listener.</param>
        private DocumentEventListener(CodeMaidPackage package)
            : base(package)
        {
            // Store access to the document events, otherwise events will not register properly via DTE.
            DocumentEvents = Package.IDE.Events.DocumentEvents;
        }

        /// <summary>
        /// An event raised when a document is closing.
        /// </summary>
        internal event Action<Document> OnDocumentClosing;

        /// <summary>
        /// A singleton instance of this command.
        /// </summary>
        public static DocumentEventListener Instance { get; private set; }

        /// <summary>
        /// Gets or sets a pointer to the IDE document events.
        /// </summary>
        private DocumentEvents DocumentEvents { get; }

        /// <summary>
        /// Initializes a singleton instance of this event listener.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>A task.</returns>
        public static async Task InitializeAsync(CodeMaidPackage package)
        {
            Instance = new DocumentEventListener(package);
            await Instance.SwitchAsync(on: true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release
        /// only unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;

                if (disposing && DocumentEvents != null)
                {
                    await SwitchAsync(on: false);
                }
            }
        }

        /// <summary>
        /// Registers event handlers with the IDE.
        /// </summary>
        /// <returns>A task.</returns>
        protected override async Task RegisterListenersAsync()
        {
            DocumentEvents.DocumentClosing += DocumentEvents_DocumentClosing;
        }

        /// <summary>
        /// Unregisters event handlers with the IDE.
        /// </summary>
        /// <returns>A task.</returns>
        protected override async Task UnRegisterListenersAsync()
        {
            DocumentEvents.DocumentClosing -= DocumentEvents_DocumentClosing;
        }

        /// <summary>
        /// An event handler for a document closing.
        /// </summary>
        /// <param name="document">The document that is closing.</param>
        private void DocumentEvents_DocumentClosing(Document document)
        {
            var onDocumentClosing = OnDocumentClosing;
            if (onDocumentClosing != null)
            {
                OutputWindowHelper.DiagnosticWriteLine($"DocumentEventListener.OnDocumentClosing raised for '{(document != null ? document.FullName : "(null)")}'");

                onDocumentClosing(document);
            }
        }
    }
}