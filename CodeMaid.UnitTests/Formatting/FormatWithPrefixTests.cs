using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.Properties;
using System;

namespace SteveCadwallader.CodeMaid.UnitTests.Formatting
{
    /// <summary>
    /// </summary>
    [TestClass]
    public class FormatWithPrefixTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Settings.Default.Reset();
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormatWithPrefixTests_KeepsPrefix()
        {
            var input = "// Lorem ipsum dolor sit amet, consectetur adipiscing elit.";
            var expected =
                "// Lorem ipsum dolor sit amet," + Environment.NewLine +
                "// consectetur adipiscing elit.";
            CommentFormatHelper.AssertEqualAfterFormat(input, expected, "//", o => o.WrapColumn = 40);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormatWithPrefixTests_TrimsTrailingSpace()
        {
            var input = "//   ";
            var expected = "//";
            CommentFormatHelper.AssertEqualAfterFormat(input, expected, "//");
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormatWithPrefixTests_KeepsLeadingSpace()
        {
            var input = "    // Lorem ipsum.";
            CommentFormatHelper.AssertEqualAfterFormat(input, input, "    //");
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormatWithPrefixTests_AlignsToFirstPrefix()
        {
            var input =
                "    // Lorem ipsum dolor sit amet, consectetur" + Environment.NewLine +
                "  // adipiscing elit.";
            var expected =
                "    // Lorem ipsum dolor sit amet," + Environment.NewLine +
                "    // consectetur adipiscing elit.";
            CommentFormatHelper.AssertEqualAfterFormat(input, expected, "    //", o => o.WrapColumn = 40);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void SimpleFormatWithPrefixTests_NoTrailingWhitespaceOnEmptyLine()
        {
            var input =
                "// Lorem ipsum dolor sit amet." + Environment.NewLine +
                "//" + Environment.NewLine +
                "// Consectetur adipiscing elit.";
            CommentFormatHelper.AssertEqualAfterFormat(input, input, "//", o => o.WrapColumn = 40);
        }
    }
}