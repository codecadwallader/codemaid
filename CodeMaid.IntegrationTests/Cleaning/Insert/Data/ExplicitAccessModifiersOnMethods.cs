namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert.Data
{
    public class ExplicitAccessModifiersOnMethods : IExplicitAccessModifiersOnMethods
    {
        // Skip static constructors - they should not have an access modifier.
        static ExplicitAccessModifiersOnMethods()
        {
        }

        // Skip destructors - they should not have an access modifier.
        ~ExplicitAccessModifiersOnMethods()
        {
        }

        void UnspecifiedMethod()
        {
        }

        public void PublicMethod()
        {
        }

        internal void InternalMethod()
        {
        }

        protected void ProtectedMethod()
        {
        }

        private void PrivateMethod()
        {
        }

        // Explicit interface implementations should not be given an explicit access modifier.
        void IExplicitAccessModifiersOnMethods.MethodInInterface()
        {
        }

        // Skip partial methods - access modifier may be specified elsewhere.
        partial void PartialMethod()
        {
        }
    }

    public interface IExplicitAccessModifiersOnMethods
    {
        // Methods in an interface should not be given an explicit access modifier.
        void MethodInInterface();
    }
}