using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using System;

namespace SteveCadwallader.CodeMaid.Integration.Events
{
    /// <summary>
    /// A class that encapsulates listening for text editor events.
    /// </summary>
    internal class TextEditorEventListener : BaseEventListener
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TextEditorEventListener" /> class.
        /// </summary>
        /// <param name="package">The package hosting the event listener.</param>
        internal TextEditorEventListener(CodeMaidPackage package)
            : base(package)
        {
            // Store access to the text editor events, otherwise events will not register properly
            // via DTE.
            TextEditorEvents = Package.IDE.Events.TextEditorEvents;
            TextEditorEvents.LineChanged += TextEditorEvents_LineChanged;
        }

        #endregion Constructors

        #region Internal Events

        /// <summary>
        /// An event raised when a line has been changed.
        /// </summary>
        internal event Action<Document> OnLineChanged;

        #endregion Internal Events

        #region Private Properties

        /// <summary>
        /// Gets or sets a pointer to the IDE text editor events.
        /// </summary>
        public TextEditorEvents TextEditorEvents { get; set; }

        #endregion Private Properties

        #region Private Event Handlers

        /// <summary>
        /// An event handler for a line being changed.
        /// </summary>
        /// <param name="startPoint">The starting point of the change.</param>
        /// <param name="endPoint">The ending point of the change.</param>
        /// <param name="hint">A hint as to the type of change that has occurred.</param>
        private void TextEditorEvents_LineChanged(TextPoint startPoint, TextPoint endPoint, int hint)
        {
            var textDocument = startPoint?.Parent;
            if (textDocument == null) return;

            var document = startPoint.Parent.Parent;

            var onLineChanged = OnLineChanged;
            if (onLineChanged != null && document != null)
            {
                OutputWindowHelper.DiagnosticWriteLine($"TextEditorEventListener.OnLineChanged raised for '{document.FullName}'");

                onLineChanged(document);
            }
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

                if (disposing && TextEditorEvents != null)
                {
                    TextEditorEvents.LineChanged -= TextEditorEvents_LineChanged;
                }
            }
        }

        #endregion IDisposable Members
    }
}