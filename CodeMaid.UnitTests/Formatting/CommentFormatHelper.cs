#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Model.Comments;

namespace SteveCadwallader.CodeMaid.UnitTests.Formatting
{
    internal class CommentFormatHelper
    {
        public static string AssertEqualAfterFormat(string input)
        {
            return AssertEqualAfterFormat(input, input);
        }

        public static string AssertEqualAfterFormat(string input, string expected)
        {
            var result = Format(input);
            Assert.AreEqual(expected, result);
            return result;
        }

        public static string Format(IEnumerable<string> text)
        {
            return Format(string.Join(Environment.NewLine, text));
        }

        public static string Format(string text)
        {
            var xml = XElement.Parse(string.Format("<doc>{0}</doc>", text));
            var line = new CommentLineXml(xml);
            var regex = CodeCommentHelper.GetCommentRegex("CSharp", false);
            var formatter = new CommentFormatter(line, string.Empty, 4, regex);
            return formatter.ToString();
        }
    }
}