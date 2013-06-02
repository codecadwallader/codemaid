using System;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert.Data
{
    public class ExplicitAccessModifiersOnEvents
    {
        private event EventHandler UnspecifiedEvent;

        public event EventHandler PublicEvent;

        internal event EventHandler InternalEvent;

        protected event EventHandler ProtectedEvent;

        private event EventHandler PrivateEvent;
    }
}