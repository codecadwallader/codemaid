namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert.Data
{
    public class ExplicitAccessModifiersOnProperties
    {
        bool UnspecifiedProperty { get; set; }

        public bool PublicProperty { get; set; }

        internal bool InternalProperty { get; set; }

        protected bool ProtectedProperty { get; set; }

        private bool PrivateProperty { get; set; }
    }
}