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
                WrapAtColumn = 100,
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
                WrapAtColumn = 100,
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
                WrapAtColumn = 100,
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
                WrapAtColumn = 100,
                XmlBreakSummaryTag = false,
                XmlBreakAllTags = false
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_TagNameKeepCase()
        {
            var input = "<Summary></Summary>";

            CommentFormatHelper.AssertEqualAfterFormat(input, new CodeCommentOptions()
            {
                WrapAtColumn = 100,
                XmlTagsToLowerCase = false
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_TagNameToLowerCase()
        {
            var input = "<Summary></Summary>";
            var expected = "<summary></summary>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions()
            {
                WrapAtColumn = 100,
                XmlTagsToLowerCase = true
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_AddSpaceToInsideTags()
        {
            var input = "<summary><see/></summary>";
            var expected = "<summary><see /></summary>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions()
            {
                WrapAtColumn = 100,
                XmlBreakSummaryTag = false,
                XmlBreakAllTags = false,
                XmlSpaceSingleTags = true
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_RemoveSpaceFromInsideTags()
        {
            var input = "<summary><see /></summary>";
            var expected = "<summary><see/></summary>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions()
            {
                WrapAtColumn = 100,
                XmlBreakSummaryTag = false,
                XmlBreakAllTags = false,
                XmlSpaceSingleTags = false
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_RemoveSpaceFromTagContent()
        {
            var input = "<summary><c> test </c></summary>";
            var expected = "<summary><c>test</c></summary>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions()
            {
                WrapAtColumn = 100,
                XmlBreakSummaryTag = false,
                XmlBreakAllTags = false,
                XmlSpaceTagContent = false
            });
        }


        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_AddSpaceToTagContent()
        {
            var input = "<summary><c>test</c></summary>";
            var expected = "<summary><c> test </c></summary>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions()
            {
                WrapAtColumn = 100,
                XmlBreakSummaryTag = false,
                XmlBreakAllTags = false,
                XmlSpaceTagContent = true
            });
        }
    }
}