namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert.Data
{
    public class BlankLinePaddingBeforeSingleLineComments
    {
        // Single line comment after brace does not insert padding.
        // Additional comments next to another single line comment does not insert padding.

        private bool _field;
        // Floating single line comment inserts padding.
        // But not on the following line as well.

        private bool _field2;
        //// Quadruple slashed comments are ignored (i.e. commented out comments).

        private void Method()
        {
            if (true)
            {
            }
            // Comments hanging between sections should be padded.
            if (true)
            {
            }
        }
    }
}