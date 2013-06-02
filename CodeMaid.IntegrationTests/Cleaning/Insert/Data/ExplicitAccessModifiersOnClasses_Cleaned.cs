namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert.Data
{
    internal class ExplicitAccessModifiersOnClasses
    {
    }

    public class PublicClass
    {
        private class NestedClass
        {
        }

        protected class ProtectedClass
        {
        }

        private class PrivateClass
        {
        }
    }

    internal class InternalClass
    {
    }
}