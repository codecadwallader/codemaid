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

            Settings.Default.Formatting_CommentXmlValueIndent = 0;
            Settings.Default.Formatting_CommentXmlSplitSummaryTagToMultipleLines = false;
            Settings.Default.Formatting_CommentXmlSplitAllTags = true;

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_BreakLongParagraphs()
        {
            var input = "<example><para>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus nisi neque, placerat sed neque vitae.</para></example>";
            var expected =
                "<example>" + Environment.NewLine +
                "<para>" + Environment.NewLine +
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit." + Environment.NewLine +
                "Vivamus nisi neque, placerat sed neque vitae." + Environment.NewLine +
                "</para>" + Environment.NewLine +
                "</example>";

            Settings.Default.Formatting_CommentWrapColumn = 60;
            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_BreakSummaryTags()
        {
            var input = "<summary></summary><returns></returns>";
            var expected = "<summary>" + Environment.NewLine + "</summary>" + Environment.NewLine + "<returns></returns>";

            Settings.Default.Formatting_CommentXmlValueIndent = 0;
            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_BreakTagsWhenContainsParagraphs()
        {
            var input = "<example><para>test</para></example>";
            var expected =
                "<example>" + Environment.NewLine +
                "<para>test</para>" + Environment.NewLine +
                "</example>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }

        /// <summary>
        /// If XML tag indenting is set, this should not affect any literal content. however,
        /// content after the literal should be indented as normal.
        /// </summary>
        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_DoesIndentAfterLiteralContent()
        {
            var input =
               "<example>" + Environment.NewLine +
               "Example usage :" + Environment.NewLine +
               "<code source=\"..\\MyExamples\\Examples.cs\" region=\"Example1\" language=\"cs\"/>" + Environment.NewLine +
               "Example usage with a location parameter and a location function:" + Environment.NewLine +
               "<code source=\"..\\MyExamples\\Examples.cs\" region=\"Example2\" language=\"cs\"/>" + Environment.NewLine +
               "And some final text that should also be formatted." + Environment.NewLine +
               "</example>";

            var expected =
               "<example>" + Environment.NewLine +
               "    Example usage :" + Environment.NewLine +
               "    <code source=\"..\\MyExamples\\Examples.cs\" region=\"Example1\" language=\"cs\"/>" + Environment.NewLine +
               "    Example usage with a location parameter and a location function:" + Environment.NewLine +
               "    <code source=\"..\\MyExamples\\Examples.cs\" region=\"Example2\" language=\"cs\"/>" + Environment.NewLine +
               "    And some final text that should also be formatted." + Environment.NewLine +
               "</example>";

            Settings.Default.Formatting_CommentXmlValueIndent = 4;
            Settings.Default.Formatting_CommentXmlKeepTagsTogether = true;

            // First pass.
            var result = CommentFormatHelper.AssertEqualAfterFormat(input, expected);

            // Second pass.
            CommentFormatHelper.AssertEqualAfterFormat(result, expected);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_DoesNotIndentCloseTag()
        {
            var input = "<summary></summary><returns></returns>";
            var expected = "<summary>" + Environment.NewLine + "</summary>" + Environment.NewLine + "<returns></returns>";

            Settings.Default.Formatting_CommentXmlValueIndent = 4;
            Settings.Default.Formatting_CommentXmlSplitSummaryTagToMultipleLines = true;
            Settings.Default.Formatting_CommentXmlSplitAllTags = false;

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }

        /// <summary>
        /// If XML tag indenting is set, this should not affect any literal content. Since
        /// whitespace is preserved on literals, this would increase the indenting with every pass.
        /// </summary>
        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_DoesNotIndentLiteralContent()
        {
            var input =
               "<test>" + Environment.NewLine +
               "<code>" + Environment.NewLine +
               "    Some code with." + Environment.NewLine +
               "   funny indenting" + Environment.NewLine +
               "" + Environment.NewLine +
               "  and a white line" + Environment.NewLine +
               "that should not change." + Environment.NewLine +
               "</code>" + Environment.NewLine +
               "</test>";

            var expected =
               "<test>" + Environment.NewLine +
               "    <code>" + Environment.NewLine +
               "    Some code with." + Environment.NewLine +
               "   funny indenting" + Environment.NewLine +
               "" + Environment.NewLine +
               "  and a white line" + Environment.NewLine +
               "that should not change." + Environment.NewLine +
               "    </code>" + Environment.NewLine +
               "</test>";

            Settings.Default.Formatting_CommentXmlValueIndent = 4;

            // First pass.
            var result = CommentFormatHelper.AssertEqualAfterFormat(input, expected);

            // Second pass.
            CommentFormatHelper.AssertEqualAfterFormat(result, expected);
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
        public void XmlFormattingTests_HyperlinkBetweenWords()
        {
            var input = "<summary>" + Environment.NewLine + "Look at this http://foo pretty link." + Environment.NewLine + "</summary>";
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
        public void XmlFormattingTests_IndentsXml()
        {
            var input = "<summary>Lorem ipsum dolor sit amet.</summary>";
            var expected =
                "<summary>" + Environment.NewLine +
                "    Lorem ipsum dolor sit amet." + Environment.NewLine +
                "</summary>";

            Settings.Default.Formatting_CommentXmlSplitSummaryTagToMultipleLines = true;
            Settings.Default.Formatting_CommentXmlValueIndent = 4;

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_IndentsXmlMultiLevel()
        {
            var input = "<summary>Lorem ipsum dolor <para>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus nisi neque, placerat sed neque vitae.</para> sit amet.</summary>";
            var expected =
                "<summary>" + Environment.NewLine +
                "    Lorem ipsum dolor" + Environment.NewLine +
                "    <para>" + Environment.NewLine +
                "        Lorem ipsum dolor sit amet, consectetur adipiscing" + Environment.NewLine +
                "        elit. Vivamus nisi neque, placerat sed neque vitae." + Environment.NewLine +
                "    </para>" + Environment.NewLine +
                "    sit amet." + Environment.NewLine +
                "</summary>";

            Settings.Default.Formatting_CommentWrapColumn = 60;
            Settings.Default.Formatting_CommentXmlSplitSummaryTagToMultipleLines = true;
            Settings.Default.Formatting_CommentXmlValueIndent = 4;

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_IndentsXmlSingleLevel()
        {
            var input = "<summary>Lorem ipsum dolor <para>Lorem ipsum dolor sit amet.</para> sit amet.</summary>";
            var expected =
                "<summary>" + Environment.NewLine +
                "    Lorem ipsum dolor" + Environment.NewLine +
                "    <para>Lorem ipsum dolor sit amet.</para>" + Environment.NewLine +
                "    sit amet." + Environment.NewLine +
                "</summary>";

            Settings.Default.Formatting_CommentXmlSplitSummaryTagToMultipleLines = true;
            Settings.Default.Formatting_CommentXmlValueIndent = 4;

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
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
                "<test>before <code>" + Environment.NewLine +
                "some" + Environment.NewLine +
                "  code" + Environment.NewLine +
                "stuff" + Environment.NewLine +
                "</code> after</test>";

            var expected =
                "<test>" + Environment.NewLine +
                "before" + Environment.NewLine +
                "<code>" + Environment.NewLine +
                "some" + Environment.NewLine +
                "  code" + Environment.NewLine +
                "stuff" + Environment.NewLine +
                "</code>" + Environment.NewLine +
                "after" + Environment.NewLine +
                "</test>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_KeepShortParagraphs()
        {
            var input =
                "<test>" + Environment.NewLine +
                "<para>" + Environment.NewLine +
                "Lorem ipsum dolor sit amet." + Environment.NewLine +
                "</para>" + Environment.NewLine +
                "</test>";

            var expected =
                "<test>" + Environment.NewLine +
                "<para>Lorem ipsum dolor sit amet.</para>" + Environment.NewLine +
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