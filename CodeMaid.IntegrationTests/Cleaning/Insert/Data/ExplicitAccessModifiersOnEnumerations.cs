namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert.Data
{
    enum UnspecifiedEnum
    {
    }

    public enum PublicEnum
    {
    }

    internal enum InternalEnum
    {
    }

    public class ExplicitAccessModifiersOnEnumerations
    {
        enum UnspecifiedEnum
        {
        }

        public enum PublicEnum
        {
        }

        internal enum InternalEnum
        {
        }

        protected enum ProtectedEnum
        {
        }

        private enum PrivateEnum
        {
        }
    }
}