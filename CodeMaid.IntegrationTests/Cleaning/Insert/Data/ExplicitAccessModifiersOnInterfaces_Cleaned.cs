namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert.Data
{
    internal interface UnknownInterface
    {
    }

    public interface PublicInterface
    {
    }

    internal interface InternalInterface
    {
    }

    public class PublicClass
    {
        private interface NestedInterface
        {
        }

        protected interface ProtectedInterface
        {
        }

        private interface PrivateInterface
        {
        }
    }
}