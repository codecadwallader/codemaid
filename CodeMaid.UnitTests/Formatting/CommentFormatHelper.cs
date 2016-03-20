using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Model.Comments;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

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
            return AssertEqualAfterFormat(input, expected, null);
        }

        public static string AssertEqualAfterFormat(string input, string expected, string prefix)
        {
            var result = Format(input, prefix);
            Assert.AreEqual(expected, result);
            return result;
        }

        public static string Format(IEnumerable<string> text)
        {
            return Format(string.Join(Environment.NewLine, text));
        }

        public static string Format(string text)
        {
            return Format(text, null);
        }

        public static string Format(string text, string prefix)
        {
            var xml = XElement.Parse($"<doc>{text}</doc>");
            var line = new CommentLineXml(xml);
            var regex = CodeCommentHelper.GetCommentRegex(CodeLanguage.CSharp, !string.IsNullOrEmpty(prefix));
            var formatter = new CommentFormatter(line, prefix, 4, regex);
            return formatter.ToString();
        }
    }
}