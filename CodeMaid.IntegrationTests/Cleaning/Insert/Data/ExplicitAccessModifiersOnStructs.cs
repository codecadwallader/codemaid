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

        private struct PrivateStruct
        {
        }
    }

    internal struct InternalStruct
    {
    }
}