using System;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert.Data
{
    public class ExplicitAccessModifiersOnEvents : IExplicitAccessModifiersOnEvents
    {
        event EventHandler UnspecifiedEvent;

        public event EventHandler PublicEvent;

        internal event EventHandler InternalEvent;

        protected event EventHandler ProtectedEvent;

        private event EventHandler PrivateEvent;

        // Explicit interface implementations should not be given an explicit access modifier.
        event EventHandler IExplicitAccessModifiersOnEvents.EventInInterface
        {
            add { }
            remove { }
        }
    }

    public interface IExplicitAccessModifiersOnEvents
    {
        // Events in an interface should not be given an explicit access modifier.
        event EventHandler EventInInterface;
    }
}