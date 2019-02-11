using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.Model.Comments.Options;
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
            var input = "<xml><see/></xml>";
            var expected = "<xml><see /></xml>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, o =>
            {
                o.Xml.Default.SpaceSelfClosing = true;
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_AddSpaceToTagContent()
        {
            var input = "<xml><c>test</c></xml>";
            var expected = "<xml> <c> test </c> </xml>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, o =>
            {
                o.Xml.Default.SpaceContent = true;
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_AddSpaceToTagContentWithSelfClosingTag()
        {
            var input = "<tag1><tag2/></tag1>";
            var expected = "<tag1> <tag2/> </tag1>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, o =>
            {
                o.Xml.Default.SpaceContent = true;
                o.Xml.Default.SpaceSelfClosing = false;
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_AddSpaceToTagContentWithSelfClosingTagMultiline()
        {
            // Add space to content should not add a space when tag content is on it's own line.
            var input = "<tag1><tag2/></tag1>";
            var expected =
                "<tag1>" + Environment.NewLine +
                "<tag2/>" + Environment.NewLine +
                "</tag1>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, o =>
            {
                o.Xml.Default.Split = XmlTagNewLine.Always;
                o.Xml.Default.SpaceContent = true;
                o.Xml.Default.SpaceSelfClosing = false;
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_AddSpaceToTagContentShouldLeaveNoTrailingWhitespace1()
        {
            var input = "<xml>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</xml>";
            var expected =
                "<xml>" + Environment.NewLine +
                "Lorem ipsum dolor sit amet," + Environment.NewLine +
                "consectetur adipiscing elit." + Environment.NewLine +
                "</xml>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, o =>
            {
                o.WrapColumn = 30;
                o.Xml.Default.Split = XmlTagNewLine.Always;
                o.Xml.Default.SpaceContent = true;
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_AddSpaceToTagContentShouldLeaveNoTrailingWhitespace2()
        {
            var input =
               "<remarks>" + Environment.NewLine +
               "Lorem ipsum dolor sit amet, consectetur adipiscing elit." + Environment.NewLine +
               "</remarks>";

            var expected =
               "<remarks>" + Environment.NewLine +
               "    Lorem ipsum dolor sit amet, consectetur" + Environment.NewLine +
               "    adipiscing elit." + Environment.NewLine +
               "</remarks>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, o =>
            {
                o.WrapColumn = 50;
                o.Xml.Default.Indent = 4;
                o.Xml.Default.SpaceContent = true;
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_AllRootLevelTagsOnNewLine()
        {
            var input = "<tag1>abc</tag1><tag2>abc</tag2>";
            var expected =
                "<tag1>abc</tag1>" + Environment.NewLine +
                "<tag2>abc</tag2>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_BreakAllTags()
        {
            var input = "<tag1></tag1><tag2></tag2>";
            var expected =
                "<tag1>" + Environment.NewLine +
                "</tag1>" + Environment.NewLine +
                "<tag2>" + Environment.NewLine +
                "</tag2>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, o =>
            {
                o.Xml.Default.Indent = 0;
                o.Xml.Default.Split = XmlTagNewLine.Always;
            });
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

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, o => o.WrapColumn = 60);
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
        /// If XML tag indenting is set, this should not affect any literal content. However, content
        /// after the literal should be indented as normal.
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
            Settings.Default.Formatting_CommentXmlSpaceSingleTags = false;

            // First pass.
            var result = CommentFormatHelper.AssertEqualAfterFormat(input, expected);

            // Second pass.
            CommentFormatHelper.AssertEqualAfterFormat(result, expected);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_DoesNotIndentCloseTag()
        {
            var input = "<tag1></tag1><tag2></tag2>";
            var expected =
                "<tag1>" + Environment.NewLine +
                "</tag1>" + Environment.NewLine +
                "<tag2></tag2>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, o =>
            {
                o.Xml.Default.Indent = 4;
                o.Xml.Default.KeepTogether = true;

                o.Xml.Tags.Clear();
                o.Xml.Tags["tag1"] = new FormatterOptionsXmlTag { Split = XmlTagNewLine.Always };
            });
        }

        /// <summary>
        /// If XML tag indenting is set, this should not affect any literal content. Since whitespace
        /// is preserved on literals, this would increase the indenting with every pass.
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
            CommentFormatHelper.AssertEqualAfterFormat("<xml></xml>");
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_DoNotAutoExpandTags()
        {
            CommentFormatHelper.AssertEqualAfterFormat("<xml/>", o => o.Xml.Default.SpaceSelfClosing = false);
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

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, o =>
            {
                o.WrapColumn = 60;
                o.Xml.Default.Indent = 4;
            });
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

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, o =>
            {
                o.Xml.Default.Indent = 4;
                o.Xml.Tags["summary"] = new FormatterOptionsXmlTag { Split = XmlTagNewLine.Always };
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

            CommentFormatHelper.AssertEqualAfterFormat(input, o => o.Xml.Default.SpaceSelfClosing = false);
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
            var input = "<xml><see /></xml>";
            var expected = "<xml><see/></xml>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, o => o.Xml.Default.SpaceSelfClosing = false);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_RemoveSpaceFromTagContent()
        {
            var input = "<xml> <c> test </c> </xml>";
            var expected = "<xml><c>test</c></xml>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, o => o.Xml.Default.SpaceContent = false);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_SplitAlwaysOnSingleTag()
        {
            var input = "<tag1></tag1><tag2></tag2>";
            var expected =
                "<tag1>" + Environment.NewLine +
                "</tag1>" + Environment.NewLine +
                "<tag2></tag2>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, o =>
            {
                o.Xml.Default.Indent = 0;
                o.Xml.Tags.Clear();
                o.Xml.Tags["tag1"] = new FormatterOptionsXmlTag { Split = XmlTagNewLine.Always };
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

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, o =>
            {
                o.WrapColumn = 50;
                o.SkipWrapOnLastWord = false;
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_TagCase_Keep()
        {
            var input = "<Xml></Xml>";

            CommentFormatHelper.AssertEqualAfterFormat(input, o => o.Xml.Default.Case = XmlTagCase.Keep);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_TagCase_Lower()
        {
            var input = "<Xml></Xml>";
            var expected = "<xml></xml>";
            CommentFormatHelper.AssertEqualAfterFormat(input, expected, o => o.Xml.Default.Case = XmlTagCase.LowerCase);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_TagCase_Upper()
        {
            var input = "<Xml></Xml>";
            var expected = "<XML></XML>";
            CommentFormatHelper.AssertEqualAfterFormat(input, expected, o => o.Xml.Default.Case = XmlTagCase.UpperCase);
        }

        /// <summary>
        /// If XML tag indenting is set, this should not affect any literal content. Since whitespace
        /// is preserved on literals, this would increase the indenting with every pass.
        /// </summary>
        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_Literal_DoesNotIndent()
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

            // First pass.
            var result = CommentFormatHelper.AssertEqualAfterFormat(input, expected, o => o.Xml.Default.Indent = 4);

            // Second pass.
            CommentFormatHelper.AssertEqualAfterFormat(result, expected, o => o.Xml.Default.Indent = 4);
        }

        /// <summary>
        /// If XML tag indenting is set, this should not affect any literal content. however, content
        /// after the literal should be indented as normal.
        /// </summary>
        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_Literal_IndentsAfterContent()
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

            // First pass.
            var result = CommentFormatHelper.AssertEqualAfterFormat(input, expected, o =>
            {
                o.Xml.Default.Indent = 4;
                o.Xml.Default.KeepTogether = true;
                o.Xml.Default.SpaceSelfClosing = false;
            });

            // Second pass.
            CommentFormatHelper.AssertEqualAfterFormat(result, expected, o =>
            {
                o.Xml.Default.Indent = 4;
                o.Xml.Default.KeepTogether = true;
                o.Xml.Default.SpaceSelfClosing = false;
            });
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void XmlFormattingTests_Literal_KeepFormatting()
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
    }
}