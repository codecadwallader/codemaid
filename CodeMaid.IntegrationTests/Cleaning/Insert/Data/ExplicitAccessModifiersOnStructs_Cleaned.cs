namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert.Data
{
    internal struct ExplicitAccessModifiersOnStructs
    {
    }

    public struct PublicStruct
    {
        private struct NestedStruct
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