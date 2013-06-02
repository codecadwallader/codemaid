namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert.Data
{
    interface UnknownInterface
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
        interface NestedInterface
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