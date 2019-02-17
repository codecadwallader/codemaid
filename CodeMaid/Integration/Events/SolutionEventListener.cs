using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using System;
using System.Threading.Tasks;

namespace SteveCadwallader.CodeMaid.Integration.Events
{
    /// <summary>
    /// A class that encapsulates listening for solution events.
    /// </summary>
    internal sealed class SolutionEventListener : BaseEventListener
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionEventListener" /> class.
        /// </summary>
        /// <param name="package">The package hosting the event listener.</param>
        private SolutionEventListener(CodeMaidPackage package)
            : base(package)
        {
            // Store access to the solutions events, otherwise events will not register properly via DTE.
            SolutionEvents = Package.IDE.Events.SolutionEvents;
        }

        /// <summary>
        /// An event raised when a solution has closed.
        /// </summary>
        internal event Action OnSolutionClosed;

        /// <summary>
        /// An event raised when a solution has opened.
        /// </summary>
        internal event Action OnSolutionOpened;

        /// <summary>
        /// A singleton instance of this command.
        /// </summary>
        public static SolutionEventListener Instance { get; private set; }

        /// <summary>
        /// Gets or sets a pointer to the IDE solution events.
        /// </summary>
        private SolutionEvents SolutionEvents { get; set; }

        /// <summary>
        /// Initializes a singleton instance of this event listener.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <returns>A task.</returns>
        public static async Task InitializeAsync(CodeMaidPackage package)
        {
            Instance = new SolutionEventListener(package);
            await Instance.SwitchAsync(on: true);
        }

        /// <summary>
        /// Fires the solution opened event directly, used for lazy loading scenarios where we detect
        /// the solution load after it happens.
        /// </summary>
        internal void FireSolutionOpenedEvent() => SolutionEvents_Opened();

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

                if (disposing && SolutionEvents != null)
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
            SolutionEvents.Opened += SolutionEvents_Opened;
            SolutionEvents.AfterClosing += SolutionEvents_AfterClosing;
        }

        /// <summary>
        /// Unregisters event handlers with the IDE.
        /// </summary>
        /// <returns>A task.</returns>
        protected override async Task UnRegisterListenersAsync()
        {
            SolutionEvents.Opened -= SolutionEvents_Opened;
            SolutionEvents.AfterClosing -= SolutionEvents_AfterClosing;
        }

        /// <summary>
        /// An event handler for a solution being closed.
        /// </summary>
        private void SolutionEvents_AfterClosing()
        {
            var onSolutionClosed = OnSolutionClosed;
            if (onSolutionClosed != null)
            {
                OutputWindowHelper.DiagnosticWriteLine("SolutionEventListener.OnSolutionClosed raised");

                onSolutionClosed();
            }
        }

        /// <summary>
        /// An event handler for a solution being opened.
        /// </summary>
        private void SolutionEvents_Opened()
        {
            var onSolutionOpened = OnSolutionOpened;
            if (onSolutionOpened != null)
            {
                OutputWindowHelper.DiagnosticWriteLine("SolutionEventListener.OnSolutionOpened raised");

                onSolutionOpened();
            }
        }
    }
}