using System;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert.Data
{
    public class BlankLinePaddingBeforeProperties
    {
        public bool AutoImplementedProperty1 { get; set; }
        public bool AutoImplementedProperty2 { get; set; }
        public bool AutoImplementedProperty3 { get; set; }
        public bool Property4
        {
            get { return false; }
        }
        public bool Property5
        {
            get { return false; }
            set { }
        }
    }
}
