using System;
using System.Threading.Tasks;

namespace SteveCadwallader.CodeMaid.Integration.Events
{
    /// <summary>
    /// The base implementation of an event listener.
    /// </summary>
    internal abstract class BaseEventListener : ISwitchableFeature, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEventListener" /> class.
        /// </summary>
        /// <param name="package">The package hosting the event listener.</param>
        protected BaseEventListener(CodeMaidPackage package)
        {
            Package = package;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is disposed.
        /// </summary>
        protected bool IsDisposed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether listeners are registered.
        /// </summary>
        protected bool IsListening { get; set; }

        /// <summary>
        /// Gets the hosting package.
        /// </summary>
        protected CodeMaidPackage Package { get; private set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Switches the event listener on or off, registering/unregistering from events from the IDE.
        /// </summary>
        /// <param name="on">True if switching the event listener on, otherwise false.</param>
        /// <returns>A task.</returns>
        public async Task SwitchAsync(bool on)
        {
            if (on && !IsListening)
            {
                IsListening = true;
                await RegisterListenersAsync();
            }
            else if (IsListening && !on)
            {
                IsListening = false;
                await UnRegisterListenersAsync();
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release
        /// only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
            }
        }

        /// <summary>
        /// Registers event handlers with the IDE.
        /// </summary>
        /// <returns>A task.</returns>
        protected abstract Task RegisterListenersAsync();

        /// <summary>
        /// Unregisters event handlers with the IDE.
        /// </summary>
        /// <returns>A task.</returns>
        protected abstract Task UnRegisterListenersAsync();
    }
}