using System.Runtime.Serialization;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Formatting.Data
{
    [DataContract(Name = "SecurityContext", Namespace = "http://schemas.datacontract.org/2004/07/Example")]
    public class StandardCommentFormat
    {
        // This is a traditional comment that started on column nine and will go on to column one
        // hundred and twenty three.

        //This comment has no space after the comment prefix, so it should be treated as code and ignored instead of formatted.

		// This comment is led with tabs instead of spaces and it is expected that this line keeps
		// its formatting and following ones are defined by VS settings.

        public void Method()
        {
            string s = "This is a string that includes the comment prefix // but it should not trigger any re-formatting.";
        }

        ////public void CommentedOutMethod()
        ////{
        ////    string[] array = new string[0];

        ////    foreach (var item in array)
        ////    {
        ////        Console.WriteLine("Test");
        ////    }
        ////}

        // This comment would only have a single word to wrap, which is wasteful and should be ignored.

        //// List:
        ////  1) item one
        ////  2) item two

        //// var buffer = input as object[];
        //// if (buffer != null)
        //// {
        ////     Console.WriteLine("some example code");
        //// }
    }
}