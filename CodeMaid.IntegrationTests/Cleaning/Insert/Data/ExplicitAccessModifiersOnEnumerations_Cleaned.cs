namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert.Data
{
    internal enum UnspecifiedEnum
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
        private enum UnspecifiedEnum
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