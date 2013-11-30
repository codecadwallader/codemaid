using System.Runtime.Serialization;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Comments.Data
{
    [DataContract(Name = "SecurityContext", Namespace = "http://schemas.datacontract.org/2004/07/Example")]
    public class StandardCommentFormat
    {
        // This is a traditional comment that started on column nine and will go on to column one
        // hundred and twenty three.

        //This comment has no space after the comment prefix, so it should be treated as code and ignored instead of formatted.

		// This comment is led with tabs instead of spaces and it is expected that this line and
		// those following it contain the same formatting.

        public void Method()
        {
            string s = "This is a string that includes the comment prefix // but it should not trigger any re-formatting.";
        }
    }
}