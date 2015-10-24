using System;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert.Data
{
    public class BlankLinePaddingBeforePropertiesMultiLine
    {
        public bool Property4
        {
            get { return false; }
        }
        public bool Property5
        {
            get { return false; }
            set { }
        }
        public void Method()
        {
        }
    }
}
