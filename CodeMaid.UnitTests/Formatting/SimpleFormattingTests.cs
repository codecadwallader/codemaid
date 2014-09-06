#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.Model.Comments;
using SteveCadwallader.CodeMaid.Properties;

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
            CommentFormatHelper.AssertEqualAfterFormat(string.Empty, string.Empty, new CodeCommentOptions(Settings.Default));
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormattingTests_DoesNotWrapShortLines()
        {
            var input = "Lorem ipsum dolor sit amet.";

            CommentFormatHelper.AssertEqualAfterFormat(input, new CodeCommentOptions(Settings.Default)
            {
                WrapAtColumn = 100
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormattingTests_PreservesMultipleBlankLine()
        {
            var input = "Lorem ipsum\r\n\r\n\r\ndolor sit amet.";

            CommentFormatHelper.AssertEqualAfterFormat(input, new CodeCommentOptions(Settings.Default)
            {
                WrapAtColumn = 100,
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormattingTests_PreservesSingleBlankLine()
        {
            var input = "Lorem ipsum\r\n\r\ndolor sit amet.";

            CommentFormatHelper.AssertEqualAfterFormat(input, new CodeCommentOptions(Settings.Default)
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

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions(Settings.Default)
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

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions(Settings.Default)
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

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions(Settings.Default)
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

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions(Settings.Default)
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

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions(Settings.Default)
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

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions(Settings.Default)
            {
                WrapAtColumn = 12,
            });
        }
    }
}