using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using SteveCadwallader.CodeMaid.Helpers;
using System;

namespace SteveCadwallader.CodeMaid.Integration.Events
{
    /// <summary>
    /// A class that encapsulates listening for shell events.
    /// </summary>
    internal sealed class ShellEventListener : BaseEventListener, IVsBroadcastMessageEvents
    {
        #region Singleton

        public static ShellEventListener Instance { get; private set; }

        public static void Intialize(CodeMaidPackage package, IVsShell shellService)
            => Instance = new ShellEventListener(package, shellService);

        #endregion Singleton

        #region Constants

        private const uint WM_SYSCOLORCHANGE = 0x0015;

        #endregion Constants

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellEventListener" /> class.
        /// </summary>
        /// <param name="package">The package hosting the event listener.</param>
        /// <param name="shellService">The shell service.</param>
        internal ShellEventListener(CodeMaidPackage package, IVsShell shellService)
            : base(package)
        {
            _shellService = shellService;
            Switch(on: true);
        }

        #endregion Constructors

        #region Internal Events

        /// <summary>
        /// An event raised when an environment color has changed.
        /// </summary>
        internal event Action EnvironmentColorChanged;

        #endregion Internal Events

        #region Private Fields

        /// <summary>
        /// An event cookie used as a notification token for broadcast events.
        /// </summary>
        private uint _broadcastEventCookie;

        /// <summary>
        /// The shell service.
        /// </summary>
        private readonly IVsShell _shellService;

        #endregion Private Fields

        #region IVsBroadcastMessageEvents Members

        /// <summary>
        /// Called when a message is broadcast to the environment window.
        /// </summary>
        /// <param name="msg">The notification message.</param>
        /// <param name="wParam">The word value parameter.</param>
        /// <param name="lParam">The long integer parameter.</param>
        /// <returns>S_OK if successful, otherwise an error code.</returns>
        public int OnBroadcastMessage(uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (msg == WM_SYSCOLORCHANGE)
            {
                var environmentColorChanged = EnvironmentColorChanged;
                if (environmentColorChanged != null)
                {
                    OutputWindowHelper.DiagnosticWriteLine("ShellEventListener.EnvironmentColorChanged raised");

                    environmentColorChanged();
                }
            }

            return VSConstants.S_OK;
        }

        #endregion IVsBroadcastMessageEvents Members

        #region ISwitchable Members

        protected override void RegisterListeners()
        {
            _shellService.AdviseBroadcastMessages(this, out _broadcastEventCookie);
        }

        protected override void UnRegisterListeners()
        {
            _shellService.UnadviseBroadcastMessages(_broadcastEventCookie);
            _broadcastEventCookie = 0;
        }

        #endregion ISwitchable Members

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

                if (disposing && _shellService != null)
                {
                    if (_broadcastEventCookie != 0)
                    {
                        Switch(on: false);
                    }
                }
            }
        }

        #endregion IDisposable Members
    }
}