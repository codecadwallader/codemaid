using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using SteveCadwallader.CodeMaid.Helpers;
using System.Linq;

namespace SteveCadwallader.CodeMaid.Integration.Events
{
    /// <summary>
    /// A class that listens for running document table events.
    /// </summary>
    internal class RunningDocumentTableEventListener : BaseEventListener, IVsRunningDocTableEvents3
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RunningDocumentTableEventListener" /> class.
        /// </summary>
        /// <param name="package">The package hosting the event listener.</param>
        internal RunningDocumentTableEventListener(CodeMaidPackage package)
            : base(package)
        {
            // Create and store a reference to the running document table.
            RunningDocumentTable = new RunningDocumentTable(package);

            // Register with the running document table for events.
            EventCookie = RunningDocumentTable.Advise(this);
        }

        #endregion Constructors

        #region Internal Events

        /// <summary>
        /// A delegate specifying the contract for a document save event.
        /// </summary>
        /// <param name="document">The document being saved.</param>
        internal delegate void OnDocumentSaveEventHandler(Document document);

        /// <summary>
        /// An event raised after a document is saved.
        /// </summary>
        internal event OnDocumentSaveEventHandler AfterSave;

        /// <summary>
        /// An event raised before a document is saved.
        /// </summary>
        internal event OnDocumentSaveEventHandler BeforeSave;

        #endregion Internal Events

        #region Private Properties

        /// <summary>
        /// Gets or sets an event cookie used as a notification token.
        /// </summary>
        private uint EventCookie { get; set; }

        /// <summary>
        /// Gets or sets a reference to the running document table.
        /// </summary>
        private RunningDocumentTable RunningDocumentTable { get; }

        #endregion Private Properties

        #region Private Methods

        /// <summary>
        /// Gets the document object from a document cookie.
        /// </summary>
        /// <param name="docCookie">The document cookie.</param>
        /// <returns>The document object, otherwise null.</returns>
        private Document GetDocumentFromCookie(uint docCookie)
        {
            // Retrieve document information from the cookie to get the full document name.
            var documentName = RunningDocumentTable.GetDocumentInfo(docCookie).Moniker;

            // Search against the IDE documents to find the object that matches the full document name.
            return Package.IDE.Documents.OfType<Document>().FirstOrDefault(x => x.FullName == documentName);
        }

        #endregion Private Methods

        #region IVsRunningDocTableEvents3 Members

        public int OnAfterAttributeChange(uint docCookie, uint grfAttribs)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterAttributeChangeEx(uint docCookie, uint grfAttribs, IVsHierarchy pHierOld, uint itemidOld, string pszMkDocumentOld, IVsHierarchy pHierNew, uint itemidNew, string pszMkDocumentNew)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterDocumentWindowHide(uint docCookie, IVsWindowFrame pFrame)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterFirstDocumentLock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Called when a document has been saved.
        /// </summary>
        /// <param name="docCookie">An abstract value representing the document about to be saved.</param>
        /// <returns>S_OK if successful, otherwise an error code.</returns>
        public int OnAfterSave(uint docCookie)
        {
            var afterSave = AfterSave;
            if (afterSave != null)
            {
                Document document = GetDocumentFromCookie(docCookie);

                OutputWindowHelper.DiagnosticWriteLine($"RunningDocumentTableEventListener.AfterSave raised for '{(document != null ? document.FullName : "(null)")}'");

                afterSave(document);
            }

            return VSConstants.S_OK;
        }

        public int OnBeforeDocumentWindowShow(uint docCookie, int fFirstShow, IVsWindowFrame pFrame)
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeLastDocumentUnlock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Called when a document is about to be saved.
        /// </summary>
        /// <param name="docCookie">An abstract value representing the document about to be saved.</param>
        /// <returns>S_OK if successful, otherwise an error code.</returns>
        public int OnBeforeSave(uint docCookie)
        {
            var beforeSave = BeforeSave;
            if (beforeSave != null)
            {
                Document document = GetDocumentFromCookie(docCookie);

                OutputWindowHelper.DiagnosticWriteLine($"RunningDocumentTableEventListener.BeforeSave raised for '{(document != null ? document.FullName : "(null)")}'");

                beforeSave(document);
            }

            return VSConstants.S_OK;
        }

        #endregion IVsRunningDocTableEvents3 Members

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

                if (disposing && RunningDocumentTable != null && EventCookie != 0)
                {
                    RunningDocumentTable.Unadvise(EventCookie);
                    EventCookie = 0;
                }
            }
        }

        #endregion IDisposable Members
    }
}