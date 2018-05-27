using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.Properties;
using System;

namespace SteveCadwallader.CodeMaid.UnitTests.Formatting
{
    /// <summary>
    /// Class with simple unit tests for formatting. This calls the formatter directly, rather than
    /// invoking it through the UI as with the integration tests.
    /// </summary>
    [TestClass]
    public class SimpleFormattingTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Settings.Default.Reset();
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormattingTests_DoesNotCreateText()
        {
            CommentFormatHelper.AssertEqualAfterFormat(string.Empty, string.Empty);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormattingTests_DoesNotWrapShortLines()
        {
            var input = "Lorem ipsum dolor sit amet.";

            CommentFormatHelper.AssertEqualAfterFormat(input);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormattingTests_PreservesMultipleBlankLine()
        {
            var input = "Lorem ipsum\r\n\r\n\r\ndolor sit amet.";

            CommentFormatHelper.AssertEqualAfterFormat(input);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormattingTests_PreservesSingleBlankLine()
        {
            var input = "Lorem ipsum\r\n\r\ndolor sit amet.";

            CommentFormatHelper.AssertEqualAfterFormat(input);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormattingTests_NoTrailingWhitespace()
        {
            var input =
                "Lorem ipsum " + Environment.NewLine + " " +
                Environment.NewLine + " " +
                "dolor sit amet. ";

            var expected =
                "Lorem ipsum" + Environment.NewLine +
                Environment.NewLine +
                "dolor sit amet.";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormattingTests_RemoveBlankLinesAfter()
        {
            var input = "Lorem ipsum dolor sit amet.\r\n\r\n";
            var expected = "Lorem ipsum dolor sit amet.";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormattingTests_RemoveBlankLinesBefore()
        {
            var input = "\r\n\r\nLorem ipsum dolor sit amet.";
            var expected = "Lorem ipsum dolor sit amet.";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormattingTests_RemovesLineBreaks()
        {
            var input = "Lorem ipsum\r\ndolor sit amet.";
            var expected = "Lorem ipsum dolor sit amet.";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormattingTests_SkipWrapOnLastWord()
        {
            var input = "Lorem ipsum dolor sit amet.";
            var expected = "Lorem ipsum\r\ndolor sit amet.";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, o =>
            {
                o.WrapColumn = 12;
                o.SkipWrapOnLastWord = true;
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormattingTests_WrapOnLastWord()
        {
            var input = "Lorem ipsum dolor sit amet.";
            var expected = "Lorem ipsum\r\ndolor sit\r\namet.";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, o =>
            {
                o.WrapColumn = 12;
                o.SkipWrapOnLastWord = false;
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormattingTests_HyperlinkOnNewLine()
        {
            var input = "http://foo";
            CommentFormatHelper.AssertEqualAfterFormat(input);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormattingTests_HyperlinkBetweenWords()
        {
            var input = "Look at this http://foo pretty link.";
            CommentFormatHelper.AssertEqualAfterFormat(input);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormattingTests_WrapsLinesAsExpected()
        {
            var input = "Lorem ipsum dolor sit.";
            var expected = "Lorem ipsum\r\ndolor sit.";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, o => o.WrapColumn = 12);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormattingTests_MergesHyphenAndNonHyphenLines()
        {
            var input =
                @"-----" + Environment.NewLine +
                @"Second line to merge onto hyphen line";

            var expected =
                @"----- Second line to merge onto hyphen line";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }
    }
}