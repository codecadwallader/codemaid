namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert.Data
{
    public class ExplicitAccessModifiersOnFields
    {
        private bool UnspecifiedField;
        public bool PublicField;
        internal bool InternalField;
        protected bool ProtectedField;
        private bool PrivateField;
    }

    // Enumeration items are registered as fields, but should not be given an explicit access modifier.
    public enum ExplicitAccessModifiersOnFieldsEnum
    {
        EnumItem1,
        EnumItem2,
        EnumItem3
    }
}