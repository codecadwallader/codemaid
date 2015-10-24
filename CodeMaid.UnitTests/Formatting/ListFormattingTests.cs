#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

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
    /// Class with list oriented unit tests for formatting. This calls the formatter directly,
    /// rather than invoking it through the UI as with the integration tests.
    /// </summary>
    [TestClass]
    public class ListFormattingTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Settings.Default.Reset();
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
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

            Settings.Default.Formatting_CommentWrapColumn = 30;

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
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

            Settings.Default.Formatting_CommentWrapColumn = 30;

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }

        [TestMethod]
        [TestCategory("Formatting UnitTests")]
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

            Settings.Default.Formatting_CommentWrapColumn = 35;

            CommentFormatHelper.AssertEqualAfterFormat(input, expected);
        }
    }
}