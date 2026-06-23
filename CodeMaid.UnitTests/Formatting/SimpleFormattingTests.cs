using SteveCadwallader.CodeMaid.Properties;
using System;
using Xunit;

namespace SteveCadwallader.CodeMaid.UnitTests.Formatting
{
    /// <summary>
    /// Class with simple unit tests for formatting. This calls the formatter directly, rather than
    /// invoking it through the UI as with the integration tests.
    /// </summary>
    public class SimpleFormattingTests
    {
        public SimpleFormattingTests()
        {
            Settings.Default.Reset();
        }

        [Fact]
        [Trait("Category", "Formatting UnitTests")]
        public void SimpleFormattingTests_DoesNotCreateText()
        {
            CommentFormatHelper.AssertEqualAfterFormat(string.Empty, string.Empty);
        }

        [Fact]
        [Trait("Category", "Formatting UnitTests")]
        public void SimpleFormattingTests_DoesNotWrapShortLines()
        {
            var input = "Lorem ipsum dolor sit amet.";

            CommentFormatHelper.AssertEqualAfterFormat(input);
        }

        [Fact]
        [Trait("Category", "Formatting UnitTests")]
        public void SimpleFormattingTests_PreservesMultipleBlankLine()
        {
            var input = "Lorem ipsum\r\n\r\n\r\ndolor sit amet.";

            CommentFormatHelper.AssertEqualAfterFormat(input);
        }

        [Fact]
        [Trait("Category", "Formatting UnitTests")]
        public void SimpleFormattingTests_PreservesSingleBlankLine()
        {
            var input = "Lorem ipsum\r\n\r\ndolor sit amet.";

            CommentFormatHelper.AssertEqualAfterFormat(input);
        }

        [Fact]
        [Trait("Category", "Formatting UnitTests")]
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

        [Fact]
        [Trait("Category", "Formatting UnitTests")]
        public void SimpleFormattingTests_RemoveBlankLinesAfter()
        {
            var input = "Lorem ipsum dolor sit amet.\r\n\r\n";
            var expected = "Lorem ipsum dolor sit amet.";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }

        [Fact]
        [Trait("Category", "Formatting UnitTests")]
        public void SimpleFormattingTests_RemoveBlankLinesBefore()
        {
            var input = "\r\n\r\nLorem ipsum dolor sit amet.";
            var expected = "Lorem ipsum dolor sit amet.";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }

        [Fact]
        [Trait("Category", "Formatting UnitTests")]
        public void SimpleFormattingTests_RemovesLineBreaks()
        {
            var input = "Lorem ipsum\r\ndolor sit amet.";
            var expected = "Lorem ipsum dolor sit amet.";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }

        [Fact]
        [Trait("Category", "Formatting UnitTests")]
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

        [Fact]
        [Trait("Category", "Formatting UnitTests")]
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

        [Fact]
        [Trait("Category", "Formatting UnitTests")]
        public void SimpleFormattingTests_HyperlinkOnNewLine()
        {
            var input = "http://foo";
            CommentFormatHelper.AssertEqualAfterFormat(input);
        }

        [Fact]
        [Trait("Category", "Formatting UnitTests")]
        public void SimpleFormattingTests_HyperlinkBetweenWords()
        {
            var input = "Look at this http://foo pretty link.";
            CommentFormatHelper.AssertEqualAfterFormat(input);
        }

        [Fact]
        [Trait("Category", "Formatting UnitTests")]
        public void SimpleFormattingTests_WrapsLinesAsExpected()
        {
            var input = "Lorem ipsum dolor sit.";
            var expected = "Lorem ipsum\r\ndolor sit.";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, o => o.WrapColumn = 12);
        }

        [Fact]
        [Trait("Category", "Formatting UnitTests")]
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