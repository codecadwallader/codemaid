using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.Model.Comments;

namespace SteveCadwallader.CodeMaid.UnitTests.Formatting
{
    /// <summary>
    /// Class with simple unit tests for formatting XML based comments. This calls the formatter
    /// directly, rather than invoking it through the UI as with the integration tests.
    /// </summary>
    [TestClass]
    public class XmlFormattingTests
    {
        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_AllRootLevelTagsOnNewLine()
        {
            var input = "<summary></summary><returns></returns>";
            var expected = "<summary></summary>" + Environment.NewLine + "<returns></returns>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions()
            {
                XmlBreakSummaryTag = false,
                XmlBreakAllTags = false
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_BreakAllTags()
        {
            var input = "<summary></summary><returns></returns>";
            var expected = "<summary>" + Environment.NewLine + "</summary>" + Environment.NewLine + "<returns>" + Environment.NewLine + "</returns>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions()
            {
                XmlBreakSummaryTag = false,
                XmlBreakAllTags = true
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_BreakSummaryTags()
        {
            var input = "<summary></summary><returns></returns>";
            var expected = "<summary>" + Environment.NewLine + "</summary>" + Environment.NewLine + "<returns></returns>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions()
            {
                XmlBreakSummaryTag = true,
                XmlBreakAllTags = false
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_SeperateClosingTagOnRootLevel()
        {
            var input = "<summary/>";
            var expected = "<summary></summary>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions()
            {
                XmlBreakSummaryTag = false,
                XmlBreakAllTags = false
            });
        }
    }
}