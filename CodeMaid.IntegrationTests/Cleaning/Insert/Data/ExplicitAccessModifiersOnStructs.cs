namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert.Data
{
    struct ExplicitAccessModifiersOnStructs
    {
    }

    public struct PublicStruct
    {
        struct NestedStruct
        {
        }

        protected struct ProtectedStruct
        {
        }

        private struct PrivateStruct
        {
        }
    }

    internal struct InternalStruct
    {
    }
}