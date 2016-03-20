using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using System;

namespace SteveCadwallader.CodeMaid.Integration.Events
{
    /// <summary>
    /// A class that encapsulates listening for window events.
    /// </summary>
    internal class WindowEventListener : BaseEventListener
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowEventListener" /> class.
        /// </summary>
        /// <param name="package">The package hosting the event listener.</param>
        internal WindowEventListener(CodeMaidPackage package)
            : base(package)
        {
            // Store access to the window events, otherwise events will not register properly via DTE.
            WindowEvents = Package.IDE.Events.get_WindowEvents(null);
            WindowEvents.WindowActivated += WindowEvents_WindowActivated;
        }

        #endregion Constructors

        #region Internal Events

        /// <summary>
        /// An event raised when a window change has occurred.
        /// </summary>
        internal event Action<Document> OnWindowChange;

        #endregion Internal Events

        #region Private Properties

        /// <summary>
        /// Gets or sets a pointer to the IDE window events.
        /// </summary>
        private WindowEvents WindowEvents { get; }

        #endregion Private Properties

        #region Private Event Handlers

        /// <summary>
        /// An event handler for a window being activated.
        /// </summary>
        /// <param name="gotFocus">The window that got focus.</param>
        /// <param name="lostFocus">The window that lost focus.</param>
        private void WindowEvents_WindowActivated(Window gotFocus, Window lostFocus)
        {
            if (gotFocus.Kind == "Document")
            {
                RaiseWindowChange(gotFocus.Document);
            }
            else if (Package.ActiveDocument == null)
            {
                RaiseWindowChange(null);
            }
        }

        #endregion Private Event Handlers

        #region Private Methods

        /// <summary>
        /// Raises the window change event.
        /// </summary>
        /// <param name="document">The document that got focus, may be null.</param>
        private void RaiseWindowChange(Document document)
        {
            var onWindowChange = OnWindowChange;
            if (onWindowChange != null)
            {
                OutputWindowHelper.DiagnosticWriteLine($"WindowEventListener.OnWindowChange raised for '{(document != null ? document.FullName : "(null)")}'");

                onWindowChange(document);
            }
        }

        #endregion Private Methods

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

                if (disposing && WindowEvents != null)
                {
                    WindowEvents.WindowActivated -= WindowEvents_WindowActivated;
                }
            }
        }

        #endregion IDisposable Members
    }
}