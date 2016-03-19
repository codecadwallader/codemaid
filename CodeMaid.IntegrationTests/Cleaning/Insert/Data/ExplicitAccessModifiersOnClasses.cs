namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert.Data
{
    class ExplicitAccessModifiersOnClasses
    {
    }

    public class PublicClass
    {
        class NestedClass
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