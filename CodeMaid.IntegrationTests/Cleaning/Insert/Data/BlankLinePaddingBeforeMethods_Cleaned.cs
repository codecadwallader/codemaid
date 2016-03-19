using System;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert.Data
{
    public abstract class BlankLinePaddingBeforeMethods
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

    public class BlankLinePaddingBeforeMethods2
    {
        private bool field;

        static BlankLinePaddingBeforeMethods2()
        {
        }

        public BlankLinePaddingBeforeMethods()
        {
        }

        public BlankLinePaddingBeforeMethods(bool input)
        {
        }

        ~BlankLinePaddingBeforeMethods()
        {
        }
    }
}
