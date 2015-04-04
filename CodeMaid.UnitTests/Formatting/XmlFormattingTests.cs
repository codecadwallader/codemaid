#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.Model.Comments;
using SteveCadwallader.CodeMaid.Properties;
using System;

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
        public void XmlFormattingTests_AddSpaceToInsideTags()
        {
            var input = "<summary><see/></summary>";
            var expected = "<summary><see /></summary>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions(Settings.Default)
            {
                WrapAtColumn = 100,
                XmlSplitSummaryTag = false,
                XmlSplitAllTags = false,
                XmlSpaceSingleTags = true
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_AddSpaceToTagContent()
        {
            var input = "<summary><c>test</c></summary>";
            var expected = "<summary> <c> test </c> </summary>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions(Settings.Default)
            {
                WrapAtColumn = 100,
                XmlSplitSummaryTag = false,
                XmlSplitAllTags = false,
                XmlSpaceTagContent = true
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_AllRootLevelTagsOnNewLine()
        {
            var input = "<summary>abc</summary><returns>abc</returns>";
            var expected = "<summary>abc</summary>" + Environment.NewLine + "<returns>abc</returns>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions(Settings.Default)
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

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions(Settings.Default)
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

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions(Settings.Default)
            {
                WrapAtColumn = 100,
                XmlSplitSummaryTag = true,
                XmlSplitAllTags = false
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_DoNotAutoCollapseTags()
        {
            var input = "<summary></summary>";

            CommentFormatHelper.AssertEqualAfterFormat(input, new CodeCommentOptions(Settings.Default)
            {
                WrapAtColumn = 100,
                XmlSplitSummaryTag = false,
                XmlSplitAllTags = false
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_DoNotAutoExpandTags()
        {
            var input = "<summary/>";

            CommentFormatHelper.AssertEqualAfterFormat(input, new CodeCommentOptions(Settings.Default)
            {
                WrapAtColumn = 100,
                XmlSplitSummaryTag = false,
                XmlSplitAllTags = false
            });
        }

        /// <summary>
        /// Test to make sure there is no spacing is added between an inline XML tag directly
        /// followed by interpunction.
        /// </summary>
        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_InterpunctionNoSpacing()
        {
            var input = "<test>Line with <interpunction/>.</test>";

            CommentFormatHelper.AssertEqualAfterFormat(input, new CodeCommentOptions(Settings.Default)
            {
                WrapAtColumn = 100,
                XmlSplitSummaryTag = false,
                XmlSplitAllTags = false,
                SkipWrapOnLastWord = false
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_KeepCodeFormatting()
        {
            var input =
                "<test><code>" + Environment.NewLine +
                "some" + Environment.NewLine +
                "  code" + Environment.NewLine +
                "stuff" + Environment.NewLine +
                "</code></test>";

            var expected =
                "<test>" + Environment.NewLine +
                "<code>" + Environment.NewLine +
                "some" + Environment.NewLine +
                "  code" + Environment.NewLine +
                "stuff" + Environment.NewLine +
                "</code>" + Environment.NewLine +
                "</test>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions(Settings.Default)
            {
                WrapAtColumn = 100,
                XmlSplitSummaryTag = false,
                XmlSplitAllTags = false
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_RemoveSpaceFromInsideTags()
        {
            var input = "<summary><see /></summary>";
            var expected = "<summary><see/></summary>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions(Settings.Default)
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

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions(Settings.Default)
            {
                WrapAtColumn = 100,
                XmlSplitSummaryTag = false,
                XmlSplitAllTags = false,
                XmlSpaceTagContent = false
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_SplitsTagsWhenLineDoesNotFit()
        {
            var input = "<test>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus nisi neque, placerat sed neque vitae</test>";
            var expected = "<test>" + Environment.NewLine +
                "Lorem ipsum dolor sit amet, consectetur adipiscing" + Environment.NewLine +
                "elit. Vivamus nisi neque, placerat sed neque vitae" + Environment.NewLine +
                "</test>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions(Settings.Default)
            {
                WrapAtColumn = 50,
                XmlSplitSummaryTag = false,
                XmlSplitAllTags = false,
                SkipWrapOnLastWord = false
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_TagNameKeepCase()
        {
            var input = "<Summary></Summary>";

            CommentFormatHelper.AssertEqualAfterFormat(input, new CodeCommentOptions(Settings.Default)
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

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions(Settings.Default)
            {
                WrapAtColumn = 100,
                XmlTagsToLowerCase = true,
                XmlSplitAllTags = false,
                XmlSplitSummaryTag = false
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_Hyperlink()
        {
            var input = "<summary>" + Environment.NewLine + "http://foo" + Environment.NewLine + "</summary>";
            var expected = input;
            CommentFormatHelper.AssertEqualAfterFormat(input, expected, new CodeCommentOptions(Settings.Default));
        }
    }
}