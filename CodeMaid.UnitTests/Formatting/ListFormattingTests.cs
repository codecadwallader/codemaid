using SteveCadwallader.CodeMaid.Properties;
using System;
using Xunit;

namespace SteveCadwallader.CodeMaid.UnitTests.Formatting
{
    /// <summary>
    /// Class with list oriented unit tests for formatting. This calls the formatter directly, rather
    /// than invoking it through the UI as with the integration tests.
    /// </summary>
    public class ListFormattingTests
    {
        public ListFormattingTests()
        {
            Settings.Default.Reset();
        }

        [Fact]
        [Trait("Category", "Formatting UnitTests")]
        public void ListFormattingTests_DashedList()
        {
            var input =
                @"Some text before." + Environment.NewLine +
                @"- The first item with enough words to require wrapping." + Environment.NewLine +
                @"- The second item with enough words to require wrapping." + Environment.NewLine +
                @"Some trailing text.";

            var expected =
                @"Some text before." + Environment.NewLine +
                @"- The first item with enough" + Environment.NewLine +
                @"  words to require wrapping." + Environment.NewLine +
                @"- The second item with enough" + Environment.NewLine +
                @"  words to require wrapping." + Environment.NewLine +
                @"Some trailing text.";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, o => o.WrapColumn = 30);
        }

        [Fact]
        [Trait("Category", "Formatting UnitTests")]
        public void ListFormattingTests_NumberedList()
        {
            var input =
                @"Some text before." + Environment.NewLine +
                @"1) The first item with enough words to require wrapping." + Environment.NewLine +
                @"2) The second item with enough words to require wrapping." + Environment.NewLine +
                @"Some trailing text.";

            var expected =
                @"Some text before." + Environment.NewLine +
                @"1) The first item with enough" + Environment.NewLine +
                @"   words to require wrapping." + Environment.NewLine +
                @"2) The second item with enough" + Environment.NewLine +
                @"   words to require wrapping." + Environment.NewLine +
                @"Some trailing text.";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, o => o.WrapColumn = 30);
        }

        [Fact]
        [Trait("Category", "Formatting UnitTests")]
        public void ListFormattingTests_WordList()
        {
            var input =
                @"Some text before." + Environment.NewLine +
                @"item) The first item with enough words to require wrapping." + Environment.NewLine +
                @"meti) The second item with enough words to require wrapping." + Environment.NewLine +
                @"Some trailing text.";

            var expected =
                @"Some text before." + Environment.NewLine +
                @"item) The first item with enough" + Environment.NewLine +
                @"      words to require wrapping." + Environment.NewLine +
                @"meti) The second item with enough" + Environment.NewLine +
                @"      words to require wrapping." + Environment.NewLine +
                @"Some trailing text.";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, o => o.WrapColumn = 35);
        }

        [Fact]
        [Trait("Category", "Formatting UnitTests")]
        public void ListFormattingTests_XmlListWithHeader()
        {
            var input =
                "Some text before." + Environment.NewLine +
                "<list type=\"bullet\">" + Environment.NewLine +
                "   <listheader>" + Environment.NewLine +
                "       <term>header term</term>" + Environment.NewLine +
                "       <description>description</description>" + Environment.NewLine +
                "   </listheader>" + Environment.NewLine +
                "   <item>" + Environment.NewLine +
                "       <term>item term</term>" + Environment.NewLine +
                "       <description>description</description>" + Environment.NewLine +
                "   </item>" + Environment.NewLine +
                "</list>" + Environment.NewLine +
                "Some trailing text.";

            var expected =
                "Some text before." + Environment.NewLine +
                "<list type=\"bullet\">" + Environment.NewLine +
                "<listheader>" + Environment.NewLine +
                "<term>header term</term>" + Environment.NewLine +
                "<description>description</description>" + Environment.NewLine +
                "</listheader>" + Environment.NewLine +
                "<item>" + Environment.NewLine +
                "<term>item term</term>" + Environment.NewLine +
                "<description>description</description>" + Environment.NewLine +
                "</item>" + Environment.NewLine +
                "</list>" + Environment.NewLine +
                "Some trailing text.";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }

        [Fact]
        [Trait("Category", "Formatting UnitTests")]
        public void ListFormattingTests_XmlListWithHeaderAndIndent()
        {
            var input =
                "Some text before." + Environment.NewLine +
                "<list type=\"bullet\">" + Environment.NewLine +
                "<listheader>" + Environment.NewLine +
                "<term>header term</term>" + Environment.NewLine +
                "<description>description</description>" + Environment.NewLine +
                "</listheader>" + Environment.NewLine +
                "<item>" + Environment.NewLine +
                "<term>item term</term>" + Environment.NewLine +
                "<description>description</description>" + Environment.NewLine +
                "</item>" + Environment.NewLine +
                "</list>" + Environment.NewLine +
                "Some trailing text.";

            var expected =
                "Some text before." + Environment.NewLine +
                "<list type=\"bullet\">" + Environment.NewLine +
                "  <listheader>" + Environment.NewLine +
                "    <term>header term</term>" + Environment.NewLine +
                "    <description>description</description>" + Environment.NewLine +
                "  </listheader>" + Environment.NewLine +
                "  <item>" + Environment.NewLine +
                "    <term>item term</term>" + Environment.NewLine +
                "    <description>description</description>" + Environment.NewLine +
                "  </item>" + Environment.NewLine +
                "</list>" + Environment.NewLine +
                "Some trailing text.";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, o => o.Xml.Default.Indent = 2);
        }
    }
}