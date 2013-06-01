using System.ComponentModel;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Remove.Data
{
    [Description("Some description")]

    public class BlankLinesAfterAttributes
    {
        public void NoAttributeMethod()
        {
        }

        [Description("Some description")]


        public void SingleAttributeMethod()
        {
        }

        [Description("Some description")]


        [Category("Some category")]

        public void MultipleAttributeMethod()
        {
        }
    }
}