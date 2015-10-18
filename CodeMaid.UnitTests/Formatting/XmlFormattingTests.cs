﻿#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        [TestInitialize]
        public void TestInitialize()
        {
            Settings.Default.Reset();
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_AddSpaceToInsideTags()
        {
            var input = "<summary><see/></summary>";
            var expected = "<summary><see /></summary>";

            Settings.Default.Formatting_CommentXmlSplitSummaryTagToMultipleLines = false;
            Settings.Default.Formatting_CommentXmlSpaceSingleTags = true;

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_AddSpaceToTagContent()
        {
            var input = "<summary><c>test</c></summary>";
            var expected = "<summary> <c> test </c> </summary>";

            Settings.Default.Formatting_CommentXmlSplitSummaryTagToMultipleLines = false;
            Settings.Default.Formatting_CommentXmlSpaceTags = true;

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_AllRootLevelTagsOnNewLine()
        {
            var input = "<summary>abc</summary><returns>abc</returns>";
            var expected = "<summary>abc</summary>" + Environment.NewLine + "<returns>abc</returns>";

            Settings.Default.Formatting_CommentXmlSplitSummaryTagToMultipleLines = false;

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_BreakAllTags()
        {
            var input = "<summary></summary><returns></returns>";
            var expected = "<summary>" + Environment.NewLine + "</summary>" + Environment.NewLine + "<returns>" + Environment.NewLine + "</returns>";

            Settings.Default.Formatting_CommentXmlSplitSummaryTagToMultipleLines = false;
            Settings.Default.Formatting_CommentXmlSplitAllTags = true;

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_BreakSummaryTags()
        {
            var input = "<summary></summary><returns></returns>";
            var expected = "<summary>" + Environment.NewLine + "</summary>" + Environment.NewLine + "<returns></returns>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_DoNotAutoCollapseTags()
        {
            var input = "<summary></summary>";

            Settings.Default.Formatting_CommentXmlSplitSummaryTagToMultipleLines = false;

            CommentFormatHelper.AssertEqualAfterFormat(input);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_DoNotAutoExpandTags()
        {
            var input = "<summary/>";

            Settings.Default.Formatting_CommentXmlSplitSummaryTagToMultipleLines = false;

            CommentFormatHelper.AssertEqualAfterFormat(input);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_HyperlinkOnNewLine()
        {
            var input = "<summary>" + Environment.NewLine + "http://foo" + Environment.NewLine + "</summary>";
            CommentFormatHelper.AssertEqualAfterFormat(input);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_HyperlinkBetweenWords()
        {
            var input = "<summary>" + Environment.NewLine + "Look at this http://foo pretty link." + Environment.NewLine + "</summary>";
            CommentFormatHelper.AssertEqualAfterFormat(input);
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

            CommentFormatHelper.AssertEqualAfterFormat(input);
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

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_RemoveSpaceFromInsideTags()
        {
            var input = "<summary><see /></summary>";
            var expected = "<summary><see/></summary>";

            Settings.Default.Formatting_CommentXmlSplitSummaryTagToMultipleLines = false;

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_RemoveSpaceFromTagContent()
        {
            var input = "<summary> <c> test </c> </summary>";
            var expected = "<summary><c>test</c></summary>";

            Settings.Default.Formatting_CommentXmlSplitSummaryTagToMultipleLines = false;

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
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

            Settings.Default.Formatting_CommentWrapColumn = 50;
            Settings.Default.Formatting_CommentSkipWrapOnLastWord = false;

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_SplitsTagsWhenItContainsElementOnSeparateLine()
        {
            var input = "<test><para>Lorem ipsum.</para></test>";
            var expected =
                "<test>" + Environment.NewLine +
                "<para>Lorem ipsum.</para>" + Environment.NewLine +
                "</test>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_TagNameKeepCase()
        {
            var input = "<Summary></Summary>";

            Settings.Default.Formatting_CommentXmlSplitSummaryTagToMultipleLines = false;
            Settings.Default.Formatting_CommentXmlTagsToLowerCase = false;

            CommentFormatHelper.AssertEqualAfterFormat(input);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_TagNameToLowerCase()
        {
            var input = "<Summary></Summary>";
            var expected = "<summary></summary>";

            Settings.Default.Formatting_CommentXmlSplitSummaryTagToMultipleLines = false;
            Settings.Default.Formatting_CommentXmlTagsToLowerCase = true;

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }
    }
}