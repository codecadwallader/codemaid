using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.Model.Comments;

namespace SteveCadwallader.CodeMaid.UnitTests.Formatting
{
    /// <summary>
    /// Class with simple unit tests for formatting. This calls the formatter directly, rather than
    /// invoking it through the UI as with the integration tests.
    /// </summary>
    [TestClass]
    public class SimpleFormattingTests
    {
        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormattingTests_DoesNotCreateText()
        {
            CommentFormatHelper.AssertEqualAfterFormat(string.Empty, string.Empty, new CodeCommentOptions());
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormattingTests_DoesNotWrapShortLines()
        {
            var input = "Lorem ipsum dolor sit amet.";

            CommentFormatHelper.AssertEqualAfterFormat(input, new CodeCommentOptions()
            {
                WrapAtColumn = 100
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormattingTests_PreservesMultipleBlankLine()
        {
            var input = "Lorem ipsum\r\n\r\n\r\ndolor sit amet.";

            CommentFormatHelper.AssertEqualAfterFormat(input, new CodeCommentOptions()
            {
                WrapAtColumn = 100,
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormattingTests_PreservesSingleBlankLine()
        {
            var input = "Lorem ipsum\r\n\r\ndolor sit amet.";

            CommentFormatHelper.AssertEqualAfterFormat(input, new CodeCommentOptions()
            {
                WrapAtColumn = 100,
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormattingTests_RemoveBlankLinesAfter()
        {
            var input = "Lorem ipsum dolor sit amet.\r\n\r\n";
            var expected = "Lorem ipsum dolor sit amet.";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions()
            {
                WrapAtColumn = 100,
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormattingTests_RemoveBlankLinesBefore()
        {
            var input = "\r\n\r\nLorem ipsum dolor sit amet.";
            var expected = "Lorem ipsum dolor sit amet.";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions()
            {
                WrapAtColumn = 100,
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormattingTests_RemovesLineBreaks()
        {
            var input = "Lorem ipsum\r\ndolor sit amet.";
            var expected = "Lorem ipsum dolor sit amet.";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions()
            {
                WrapAtColumn = 100,
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormattingTests_SkipWrapOnLastWord()
        {
            var input = "Lorem ipsum dolor sit amet.";
            var expected = "Lorem ipsum\r\ndolor sit amet.";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions()
            {
                WrapAtColumn = 12,
                SkipWrapOnLastWord = true
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormattingTests_WrapOnLastWord()
        {
            var input = "Lorem ipsum dolor sit amet.";
            var expected = "Lorem ipsum\r\ndolor sit\r\namet.";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions()
            {
                WrapAtColumn = 12,
                SkipWrapOnLastWord = false
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormattingTests_WrapsLinesAsExpected()
        {
            var input = "Lorem ipsum dolor sit.";
            var expected = "Lorem ipsum\r\ndolor sit.";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions()
            {
                WrapAtColumn = 12,
            });
        }
    }
}