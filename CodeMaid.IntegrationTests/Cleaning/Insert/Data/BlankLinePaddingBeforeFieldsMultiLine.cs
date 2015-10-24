using System;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Insert.Data
{
    public class BlankLinePaddingBeforeFieldsMultiLine
    {
        // Field with a leading comment.
        public bool field1;
        // Field with a leading comment.
        public bool field2;
        public bool field3;
        // Field with a leading comment.
        public bool field4;
        /// <summary>
        /// An XML leading comment field.
        /// </summary>
        public bool field5;
    }
}
