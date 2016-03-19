namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert.Data
{
    public class ExplicitAccessModifiersOnDelegates
    {
        delegate void UnspecifiedDelegate();

        public delegate void PublicDelegate();

        protected delegate void ProtectedDelegate();

        internal delegate void InternalDelegate();

        private delegate void PrivateDelegate();
    }
}