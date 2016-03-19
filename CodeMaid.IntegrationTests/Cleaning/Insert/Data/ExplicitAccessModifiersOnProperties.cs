namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert.Data
{
    public class ExplicitAccessModifiersOnProperties : IExplicitAccessModifiersOnProperties
    {
        bool UnspecifiedProperty { get; set; }

        public bool PublicProperty { get; set; }

        internal bool InternalProperty { get; set; }

        protected bool ProtectedProperty { get; set; }

        private bool PrivateProperty { get; set; }

        public bool PrivateGetterProperty { private get; set; }

        public bool PrivateSetterProperty { get; private set; }

        // Explicit interface implementations should not be given an explicit access modifier.
        bool IExplicitAccessModifiersOnProperties.PropertyInInterface
        {
            get { return false; }
        }
    }

    public interface IExplicitAccessModifiersOnProperties
    {
        // Properties in an interface should not be given an explicit access modifier.
        bool PropertyInInterface { get; }
    }
}