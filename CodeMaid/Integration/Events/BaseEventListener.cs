using System.Threading.Tasks;

namespace SteveCadwallader.CodeMaid.Integration.Events
{
    /// <summary>
    /// The base implementation of an event listener.
    /// </summary>
    internal abstract class BaseEventListener : ISwitchableFeature
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
        /// Gets or sets a value indicating whether listeners are registered.
        /// </summary>
        protected bool IsListening { get; set; }

        /// <summary>
        /// Gets the hosting package.
        /// </summary>
        protected CodeMaidPackage Package { get; private set; }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        /// <summary>
        /// Switches the event listener on or off, registering/unregistering from events from the IDE.
        /// </summary>
        /// <param name="on">True if switching the event listener on, otherwise false.</param>
        /// <returns>A task.</returns>
        public async Task SwitchAsync(bool on)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (on && !IsListening)
            {
                IsListening = true;
                RegisterListeners();
            }
            else if (IsListening && !on)
            {
                IsListening = false;
                UnRegisterListeners();
            }
        }

        /// <summary>
        /// Registers event handlers with the IDE.
        /// </summary>
        protected abstract void RegisterListeners();

        /// <summary>
        /// Unregisters event handlers with the IDE.
        /// </summary>
        protected abstract void UnRegisterListeners();
    }
}