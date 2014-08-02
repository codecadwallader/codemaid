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
            var input = "<summary>abc</summary><returns>abc</returns>";
            var expected = "<summary>abc</summary>" + Environment.NewLine + "<returns>abc</returns>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions()
            {
                WrapAtColumn = 100,
                XmlSplitSummaryTag = false,
                XmlSplitAllTags = false
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
                XmlSplitSummaryTag = false,
                XmlSplitAllTags = true
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
                XmlSplitSummaryTag = true,
                XmlSplitAllTags = false
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
                XmlSplitSummaryTag = false,
                XmlSplitAllTags = false
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
                XmlTagsToLowerCase = false,
                XmlSplitAllTags = false,
                XmlSplitSummaryTag = false
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
                XmlTagsToLowerCase = true,
                XmlSplitAllTags = false,
                XmlSplitSummaryTag = false
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
                XmlSplitSummaryTag = false,
                XmlSplitAllTags = false,
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
                XmlSplitSummaryTag = false,
                XmlSplitAllTags = false,
                XmlSpaceSingleTags = false
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_RemoveSpaceFromTagContent()
        {
            var input = "<summary> <c> test </c> </summary>";
            var expected = "<summary><c>test</c></summary>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions()
            {
                WrapAtColumn = 100,
                XmlSplitSummaryTag = false,
                XmlSplitAllTags = false,
                XmlSpaceTagContent = false
            });
        }


        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_AddSpaceToTagContent()
        {
            var input = "<summary><c>test</c></summary>";
            var expected = "<summary> <c> test </c> </summary>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions()
            {
                WrapAtColumn = 100,
                XmlSplitSummaryTag = false,
                XmlSplitAllTags = false,
                XmlSpaceTagContent = true
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_SplitsTagsWhenLineDoesNotFit()
        {
            var input = "<test>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus nisi neque, placerat sed neque vitae.</test>";
            var expected = "<test>" + Environment.NewLine + 
                "Lorem ipsum dolor sit amet, consectetur adipiscing" + Environment.NewLine + 
                "elit. Vivamus nisi neque, placerat sed neque vitae." + Environment.NewLine + 
                "</test>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions()
            {
                WrapAtColumn = 50,
                XmlSplitSummaryTag = false,
                XmlSplitAllTags = false
            });
        }
    }
}