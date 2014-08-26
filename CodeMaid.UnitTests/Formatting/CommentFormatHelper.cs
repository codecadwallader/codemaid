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
        public static void AssertEqualAfterFormat(string input, CodeCommentOptions options)
        {
            AssertEqualAfterFormat(input, input, options);
        }

        public static void AssertEqualAfterFormat(string input, string expected, CodeCommentOptions options)
        {
            var result = Format(input, options);
            Assert.AreEqual(expected, result);
        }

        public static string Format(IEnumerable<string> text, CodeCommentOptions options)
        {
            return Format(string.Join(Environment.NewLine, text), options);
        }

        public static string Format(string text, CodeCommentOptions options)
        {
            var xml = XElement.Parse(string.Format("<doc>{0}</doc>", text));
            var line = new CommentLineXml(xml, options);
            var regex = CodeCommentHelper.GetCommentRegex("CSharp", false);
            var formatter = new CommentFormatter(line, string.Empty, options, regex);
            return formatter.ToString();
        }
    }
}