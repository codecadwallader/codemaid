using Microsoft.CodeAnalysis;
using SteveCadwallader.CodeMaid.Helpers;

namespace SteveCadwallader.CodeMaid.Integration.Events
{
    /// <summary>
    /// A class that encapsulates listening for Roslyn workspace events.
    /// </summary>
    internal class WorkspaceEventListener : BaseEventListener
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkspaceEventListener" /> class.
        /// </summary>
        /// <param name="package">The package hosting the event listener.</param>
        internal WorkspaceEventListener(CodeMaidPackage package)
            : base(package)
        {
            if (Package.Workspace != null)
            {
                Package.Workspace.DocumentActiveContextChanged += OnDocumentActiveContextChanged;
                Package.Workspace.DocumentClosed += OnDocumentClosed;
                Package.Workspace.DocumentOpened += OnDocumentOpened;
            }
        }

        #endregion Constructors

        #region Private Event Handlers

        private void OnDocumentActiveContextChanged(object sender, DocumentActiveContextChangedEventArgs e)
        {
            OutputWindowHelper.DiagnosticWriteLine($"WorkspaceEventListener.OnDocumentActiveContextChanged raised for document id {e.NewActiveContextDocumentId}");
        }

        private void OnDocumentClosed(object sender, DocumentEventArgs e)
        {
            OutputWindowHelper.DiagnosticWriteLine($"WorkspaceEventListener.OnDocumentClosed raised for document {e.Document}");
        }

        private void OnDocumentOpened(object sender, DocumentEventArgs e)
        {
            OutputWindowHelper.DiagnosticWriteLine($"WorkspaceEventListener.OnDocumentOpened raised for document {e.Document}");
        }

        #endregion Private Event Handlers

        #region IDisposable Members

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

                if (disposing && Package.Workspace != null)
                {
                    Package.Workspace.DocumentActiveContextChanged -= OnDocumentActiveContextChanged;
                    Package.Workspace.DocumentClosed -= OnDocumentClosed;
                    Package.Workspace.DocumentOpened -= OnDocumentOpened;
                }
            }
        }

        #endregion IDisposable Members
    }
}