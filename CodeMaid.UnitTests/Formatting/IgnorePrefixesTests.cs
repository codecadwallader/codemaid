using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.Properties;
using System;

namespace SteveCadwallader.CodeMaid.UnitTests.Formatting
{
    /// <summary>
    /// Test for the ignoring of comments lines starting with certain prefixes.
    /// </summary>
    [TestClass]
    public class IgnorePrefixesTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Settings.Default.Reset();
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void IgnorePrefixesTests_DoesNotWrapSingleLine()
        {
            CommentFormatHelper.AssertEqualAfterFormat(@"TODO: Lorem ipsum dolor sit amet, consectetur adipiscing elit.", o => o.WrapColumn = 30);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void IgnorePrefixesTests_DoesNotWrapLineInsideComment()
        {
            var input =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit." + Environment.NewLine +
                "TODO: Lorem ipsum dolor sit amet, consectetur adipiscing elit." + Environment.NewLine +
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit.";

            // Expect all lines to be wrapped except for the one starting with "TODO".
            var expected =
                "Lorem ipsum dolor sit amet," + Environment.NewLine +
                "consectetur adipiscing elit." + Environment.NewLine +
                "TODO: Lorem ipsum dolor sit amet, consectetur adipiscing elit." + Environment.NewLine +
                "Lorem ipsum dolor sit amet," + Environment.NewLine +
                "consectetur adipiscing elit.";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, o => o.WrapColumn = 30);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
        public void IgnorePrefixesTests_DoesNotCombineSubsequentLines()
        {
            var input =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit." + Environment.NewLine +
                "TODO: Lorem ipsum dolor sit amet, consectetur adipiscing elit." + Environment.NewLine +
                "TODO: Lorem ipsum dolor sit amet, consectetur adipiscing elit." + Environment.NewLine +
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit.";

            // Expect every "ignored" line to stay on it's own.
            var expected =
                "Lorem ipsum dolor sit amet," + Environment.NewLine +
                "consectetur adipiscing elit." + Environment.NewLine +
                "TODO: Lorem ipsum dolor sit amet, consectetur adipiscing elit." + Environment.NewLine +
                "TODO: Lorem ipsum dolor sit amet, consectetur adipiscing elit." + Environment.NewLine +
                "Lorem ipsum dolor sit amet," + Environment.NewLine +
                "consectetur adipiscing elit.";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, o => o.WrapColumn = 30);
        }
    }
}