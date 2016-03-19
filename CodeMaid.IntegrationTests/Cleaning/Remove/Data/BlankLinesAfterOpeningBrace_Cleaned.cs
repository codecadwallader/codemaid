namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Remove.Data
{
    public class BlankLinesAfterOpeningBrace
    {
        public void Method()
        {
            if (true)
            { // Trailing comments should be kept in place.
            }
            else
            {
            }
        }
    }
}