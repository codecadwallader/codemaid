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

    // Partial classes should be ignored since the access modifier may be specified in another location.
    partial class PartialClass
    {
    }
}