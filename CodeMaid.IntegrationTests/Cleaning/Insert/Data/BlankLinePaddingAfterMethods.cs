using System;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert.Data
{
    public abstract class BlankLinePaddingAfterMethods
    {
        public void Method()
        {
        }
        public void Method2() { }
        public static StaticMethod()
        {
        }
        public abstract void Method2();
    }

    public class BlankLinePaddingAfterMethods2
    {
        private bool field;
        static BlankLinePaddingAfterMethods2()
        {
        }
        public BlankLinePaddingAfterMethods()
        {
        }
        public BlankLinePaddingAfterMethods(bool input)
        {
        }
        ~BlankLinePaddingAfterMethods()
        {
        }
    }
}
