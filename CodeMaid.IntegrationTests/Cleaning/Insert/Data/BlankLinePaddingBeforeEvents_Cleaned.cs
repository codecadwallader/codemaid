using System;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert.Data
{
    public class BlankLinePaddingBeforeEvents
    {
        public event EventHandler Event1();

        public event EventHandler Event2();

        public event EventHandler Event3();
        public delegate void Delegate1();
    }
}
