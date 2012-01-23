#region CodeMaid is Copyright 2007-2012 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2012 Steve Cadwallader.

using System;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

namespace SteveCadwallader.CodeMaid.Events
{
    /// <summary>
    /// A class that encapsulates listening for shell events.
    /// </summary>
    internal class ShellEventListener : BaseEventListener, IVsShellPropertyEvents
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellEventListener"/> class.
        /// </summary>
        /// <param name="package">The package hosting the event listener.</param>
        /// <param name="shellService">The shell service.</param>
        internal ShellEventListener(CodeMaidPackage package, IVsShell shellService)
            : base(package)
        {
            _shellService = shellService;
            if (_shellService != null)
            {
                _shellService.AdviseShellPropertyChanges(this, out _eventCookie);
            }
        }

        #endregion Constructors

        #region Internal Events

        /// <summary>
        /// An event raised when the shell is available.
        /// </summary>
        internal event Action ShellAvailable;

        #endregion Internal Events

        #region Private Fields

        /// <summary>
        /// An event cookie used as a notification token.
        /// </summary>
        private uint _eventCookie;

        /// <summary>
        /// The shell service.
        /// </summary>
        private readonly IVsShell _shellService;

        #endregion Private Fields

        #region IVsShellPropertyEvents Members

        /// <summary>
        /// Called when a shell property has changed.
        /// </summary>
        /// <param name="propid">The id of the property that changed.</param>
        /// <param name="var">The new value of the property.</param>
        /// <returns>S_OK if successful, otherwise an error code.</returns>
        public int OnShellPropertyChange(int propid, object var)
        {
            // Check if the zombie state property of the shell has changed to false.
            if ((int)__VSSPROPID.VSSPROPID_Zombie == propid && ((bool)var == false))
            {
                // Raise the shell available event.
                if (ShellAvailable != null)
                {
                    ShellAvailable();
                }
            }

            return VSConstants.S_OK;
        }

        #endregion IVsShellPropertyEvents Members

        #region IDisposable Members

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;

                if (disposing && _shellService != null && _eventCookie != 0)
                {
                    _shellService.UnadviseShellPropertyChanges(_eventCookie);
                    _eventCookie = 0;
                }
            }
        }

        #endregion IDisposable Members
    }
}