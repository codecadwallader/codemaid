using EnvDTE;
using System;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A class that handles tracking a document and switching back to it, typically in a using
    /// statement context.
    /// </summary>
    internal class ActiveDocumentRestorer : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveDocumentRestorer" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        internal ActiveDocumentRestorer(CodeMaidPackage package)
        {
            Package = package;

            StartTracking();
        }

        /// <summary>
        /// Gets or sets the hosting package.
        /// </summary>
        private CodeMaidPackage Package { get; set; }

        /// <summary>
        /// Gets or sets the active document.
        /// </summary>
        private Document TrackedDocument { get; set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            RestoreTrackedDocument();
        }

        /// <summary>
        /// Restores the tracked document if not already active.
        /// </summary>
        internal void RestoreTrackedDocument()
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            if (TrackedDocument != null && Package.ActiveDocument != TrackedDocument)
            {
                TrackedDocument.Activate();
            }
        }

        /// <summary>
        /// Starts tracking the active document.
        /// </summary>
        internal void StartTracking()
        {
            // Cache the active document.
            TrackedDocument = Package.ActiveDocument;
        }
    }
}