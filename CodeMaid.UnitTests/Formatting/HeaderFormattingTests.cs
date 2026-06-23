using SteveCadwallader.CodeMaid.Properties;
using System;
using Xunit;

namespace SteveCadwallader.CodeMaid.UnitTests.Formatting
{
    /// <summary>
    /// Class with simple unit tests for formatting header type comments. This calls the formatter
    /// directly, rather than invoking it through the UI as with the integration tests.
    /// </summary>
    public class HeaderFormattingTests
    {
        public HeaderFormattingTests()
        {
            Settings.Default.Reset();
        }

        /// <summary>
        /// Tests the forced indenting of the XML copyright file header.
        /// </summary>
        [Fact]
        [Trait("Category", "Formatting UnitTests")]
        public void HeaderFormattingTests_Copyright_Indenting()
        {
            var input =
                @"<copyright file=""NameOfFile.cs"" company=""CompanyName"">" + Environment.NewLine +
                @"Company copyright tag." + Environment.NewLine +
                @"</copyright>";

            var expected =
                @"<copyright file=""NameOfFile.cs"" company=""CompanyName"">" + Environment.NewLine +
                @"    Company copyright tag." + Environment.NewLine +
                @"</copyright>";

            CommentFormatHelper.AssertEqualAfterFormat(input, expected, o => o.Xml.Default.Indent = 0);
        }

        [Fact]
        [Trait("Category", "Formatting UnitTests")]
        public void HeaderFormattingTests_PreservesHyphenLinesWithoutXML()
        {
            var input =
                @"--------------------------------------------------------------------------------------------------------------------" + Environment.NewLine +
                Environment.NewLine +
                @"Copyright (c) 2012 - 2013 . All rights reserved." + Environment.NewLine +
                Environment.NewLine +
                @"--------------------------------------------------------------------------------------------------------------------";

            CommentFormatHelper.AssertEqualAfterFormat(input);
        }

        [Fact]
        [Trait("Category", "Formatting UnitTests")]
        public void HeaderFormattingTests_Copyright_PreservesHyphenLinesWithXML()
        {
            var input =
                @"-----------------------------------------------------------------------" + Environment.NewLine +
                @"<copyright file=""NameOfFile.cs"" company=""CompanyName"">" + Environment.NewLine +
                @"    Company copyright tag." + Environment.NewLine +
                @"</copyright>" + Environment.NewLine +
                @"-----------------------------------------------------------------------";

            CommentFormatHelper.AssertEqualAfterFormat(input);
        }
    }
}