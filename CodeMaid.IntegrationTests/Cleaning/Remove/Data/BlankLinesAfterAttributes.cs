using System.ComponentModel;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Remove.Data
{
    [Description("Some description")] // Trailing comments should be kept in place

    public class BlankLinesAfterAttributes
    {
        public void NoAttributeMethod()
        {
        }

        [Description("Some description")]


        public void SingleAttributeMethod()
        {
        }

        [Description("Some description")] // Trailing comments should be kept in place


        [Category("Some category")]

        public void MultipleAttributeMethod()
        {
        }
    }
}