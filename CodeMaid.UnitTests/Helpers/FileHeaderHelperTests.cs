using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using SteveCadwallader.CodeMaid.Helpers;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Assert = NUnit.Framework.Assert;

namespace SteveCadwallader.CodeMaid.UnitTests.Helpers
{
    // remark: EnvDTE only counts 1 character per newline
    [TestClass]
    public class FileHeaderHelperTests
    {
        [TestCase(CodeLanguage.CSharp, "// some CSharp test header\r\n")]
        [TestCase(CodeLanguage.CSharp, "/* some C# \r\n* test\r\n header\r\n *\r\n *\r\n* !!! */\r\n")]
        [TestCase(CodeLanguage.JavaScript, "// some JavaScript test header\r\n")]
        [TestCase(CodeLanguage.JavaScript, "/* some JS \r\n* test \r\n header !!! */\r\n")]
        [TestCase(CodeLanguage.LESS, "// some LESS test header\r\n")]
        [TestCase(CodeLanguage.LESS, "/* some LESS \r\n* test \r\n header !!! */\r\n")]
        [TestCase(CodeLanguage.SCSS, "// some SCSS test header\r\n")]
        [TestCase(CodeLanguage.SCSS, "/* some SCSS\r\n * test\r\n header !!! */\r\n")]
        [TestCase(CodeLanguage.TypeScript, "// some TypeScript test header\r\n")]
        [TestCase(CodeLanguage.TypeScript, "/* some TypeScript\r\n* test\r\n header !!! */\r\n")]
        [TestCase(CodeLanguage.HTML, "<!-- some HTML test header -->\r\n")]
        [TestCase(CodeLanguage.HTML, "<!-- some HTML \r\n test\r\n header ! -->")]
        [TestCase(CodeLanguage.XAML, "<!-- some XAML test header -->\r\n")]
        [TestCase(CodeLanguage.XAML, "<!-- some XAML \r\n test\r\n header ! -->")]
        [TestCase(CodeLanguage.XML, "<!-- some XML test header -->\r\n")]
        [TestCase(CodeLanguage.XML, "<!-- some XML \r\n test\r\n header ! -->")]
        [TestCase(CodeLanguage.CSS, "/* some CSS test header */\r\n")]
        [TestCase(CodeLanguage.CSS, "/* some CSS \r\ntest \r\nheader */\r\n")]
        [TestCase(CodeLanguage.CPlusPlus, "// some CPlusPlus test header\r\n")]
        [TestCase(CodeLanguage.CPlusPlus, "/* some C++ \r\ntest\r\n header */\r\n")]
        [TestCase(CodeLanguage.PHP, "// some PHP test header\r\n")]
        [TestCase(CodeLanguage.PHP, "# some PHP test header\r\n")]
        [TestCase(CodeLanguage.PHP, "/* some PHP \r\n test\r\nheader */")]
        [TestCase(CodeLanguage.PowerShell, "# some PowerShell test header\r\n")]
        [TestCase(CodeLanguage.PowerShell, "<# some PowerShell\r\ntest\r\n  header ! #>")]
        [TestCase(CodeLanguage.R, "# some R test header\r\n")]
        [TestCase(CodeLanguage.FSharp, "// some F# test header\r\n")]
        [TestCase(CodeLanguage.FSharp, "(* some F#\r\n test \r\nheader\r\n*)")]
        [TestCase(CodeLanguage.VisualBasic, "' some PowerShell test header\r\n")]
        public void GetHeaderLengthLanguage(CodeLanguage language, string text)
        {
            var headerLength = FileHeaderHelper.GetHeaderLength(language, text);

            Assert.IsTrue(headerLength > 0, $"Expecting value > 0, found {headerLength}");
        }

        [TestCase(CodeLanguage.CSharp, "# some CSharp test header\r\n")]
        [TestCase(CodeLanguage.JavaScript, "<!-- some JavaScript test header -->\r\n")]
        [TestCase(CodeLanguage.LESS, "' some LESS test header\r\n")]
        [TestCase(CodeLanguage.SCSS, "# some SCSS test header\r\n")]
        [TestCase(CodeLanguage.TypeScript, "<!-- some TypeScript test header -->\r\n")]
        [TestCase(CodeLanguage.HTML, "// some HTML test header\r\n")]
        [TestCase(CodeLanguage.XAML, "/* some XAML test header */\r\n")]
        [TestCase(CodeLanguage.XML, "' some XML test header\r\n")]
        [TestCase(CodeLanguage.CSS, "// some CSS test header\r\n")]
        [TestCase(CodeLanguage.CPlusPlus, "# some CPlusPlus test header\r\n")]
        [TestCase(CodeLanguage.PHP, "' some PHP test header\r\n")]
        [TestCase(CodeLanguage.PowerShell, "/* some PowerShell\r\ntest\r\n  header ! */")]
        [TestCase(CodeLanguage.R, "// some R test header\r\n")]
        [TestCase(CodeLanguage.FSharp, "/* some F#\r\n test \r\nheader\r\n*/")]
        [TestCase(CodeLanguage.VisualBasic, "// some PowerShell test header\r\n")]
        public void GetHeaderLengthLanguageWithWrongTags(CodeLanguage language, string text)
        {
            var headerLength = FileHeaderHelper.GetHeaderLength(language, text);

            Assert.IsTrue(headerLength == 0, $"Expecting 0, found {headerLength}");
        }

        [TestCase("/* */", "/*", "*/")]
        [TestCase("/* \r\n   Copyright © 2021 \r\n */", "/*", "*/")]
        [TestCase("<!-- -->", "<!--", "-->")]
        [TestCase("<!-- =========== \r\n \r\n  Copyright © 2021 \r\n \r\n =========== -->", "<!--", "-->")]
        [TestCase("<# #>", "<#", "#>")]
        [TestCase("<# //////////////// \r\n   Copyright © 2021 \r\n //////////////// #>", "<#", "#>")]
        [TestCase("(* *)", "(*", "*)")]
        [TestCase("(* ~~~~ \r\n ~~ \r\n  Copyright © 2021 \r\n ~~ \r\n ~~~~ *)", "(*", "*)")]
        public void GetHeaderLengthMultiLine(string text, string tagStart, string tagEnd)
        {
            var headerLength = FileHeaderHelper.GetHeaderLength(text, tagStart, tagEnd, false);
            var expectedLength = text.Length - Regex.Matches(text, Environment.NewLine).Count + 1;

            Assert.IsTrue(headerLength == expectedLength, $"Expecting {expectedLength}, found {headerLength}");
        }

        [TestCase("using System;\r\n/* */", "/*", "*/", 6)]
        [TestCase("using EnvDTE;\r\nusing System;\r\nusing SteveCadwallader.CodeMaid.Helpers;\r\n/* \r\n   Copyright © 2021 \r\n */", "/*", "*/", 29)]
        public void GetHeaderLengthMultiLineSkipUsings(string text, string tagStart, string tagEnd, int expectedLength)
        {
            var headerLength = FileHeaderHelper.GetHeaderLength(text, tagStart, tagEnd, true);

            Assert.IsTrue(headerLength == expectedLength, $"Expecting {expectedLength}, found {headerLength}");
        }

        [TestCase("\r\n/* */", "/*", "*/")]
        [TestCase("\r\n\r\n\r\n/*    Copyright © 2021  */", "/*", "*/")]
        [TestCase("\r\n\r\n<!-- -->", "<!--", "-->")]
        public void GetHeaderLengthMultiLineWithEmptyLines(string text, string tagStart, string tagEnd)
        {
            var headerLength = FileHeaderHelper.GetHeaderLength(text, tagStart, tagEnd, false);
            var expectedLength = text.Length - Regex.Matches(text, Environment.NewLine).Count + 1;

            Assert.IsTrue(headerLength == expectedLength, $"Expecting {expectedLength}, found {headerLength}");
        }

        [TestCase("using EnvDTE;\r\nusing System;\r\nusing SteveCadwallader.CodeMaid.Helpers;\r\n\r\n\r\n/* \r\n   Copyright © 2021 \r\n */", "/*", "*/", 29)]
        [TestCase("using System;\r\n\r\n<!-- -->", "<!--", "-->", 9)]
        public void GetHeaderLengthMultiLineWithEmptyLinesSkipUsings(string text, string tagStart, string tagEnd, int expectedLength)
        {
            var headerLength = FileHeaderHelper.GetHeaderLength(text, tagStart, tagEnd, true);

            Assert.IsTrue(headerLength == expectedLength, $"Expecting {expectedLength}, found {headerLength}");
        }

        [TestCase("", "/*", "*/")]
        [TestCase("some text", "/*", "*/")]
        [TestCase("/*     \r\n \r\n      ", "/*", "*/")]
        [TestCase("/* header */", "/*", "*>")]
        [TestCase("*/           ", "/*", "*/")]
        public void GetHeaderLengthMultiLineZero(string text, string tagStart, string tagEnd)
        {
            var headerLength = FileHeaderHelper.GetHeaderLength(text, tagStart, tagEnd, false);

            Assert.IsTrue(headerLength == 0, $"Expecting 0, found {headerLength}");
        }

        [TestCase("using System;", "/*", "*/")]
        [TestCase("using System;\r\n/*     \r\n \r\n      ", "/*", "*/")]
        [TestCase("using System;\r\n/* header */", "/*", "*>")]
        [TestCase("using System;\r\n*/           ", "/*", "*/")]
        public void GetHeaderLengthMultiLineZeroSkipUsings(string text, string tagStart, string tagEnd)
        {
            var headerLength = FileHeaderHelper.GetHeaderLength(text, tagStart, tagEnd, true);

            Assert.IsTrue(headerLength == 0, $"Expecting 0, found {headerLength}");
        }

        [TestCase("//", "// header fdsfndksjnfe\r\n")]
        [TestCase("#", "# header eropf opekfropze jaqozerijfg uihgsdfidnffdsfg\r\n")]
        [TestCase("'", "'header  sdvq qsudg bqudgybuydzeau _(àç_ç*ù$^$*ùàyguozadgzao\r\n")]
        [TestCase("//", "// ====================\r\n// Copyright\r\n// ====================\r\n")]
        [TestCase("#", "# ==============\r\n#   Copyright !\r\n#    ~~\r\n# ==============\r\n")]
        [TestCase("'", "' ==============\r\n' some text\r\n'   Copyright !\r\n' ==============\r\n")]
        public void GetHeaderLengthMultiSingleLine(string tag, string text)
        {
            var headerLength = FileHeaderHelper.GetHeaderLength(text, tag, false);
            var expectedLength = text.Length - Regex.Matches(text, Environment.NewLine).Count;

            Assert.IsTrue(headerLength == expectedLength, $"Expecting {expectedLength}, found {headerLength}");
        }

        [TestCase("//", "using System;\r\n// header\r\nnamespace ", 11)]
        [TestCase("//", "using System;\r\n// header I\r\n// header II\r\nnamespace ", 26)]
        public void GetHeaderLengthMultiSingleLineSkipUsings(string tag, string text, int expectedLength)
        {
            var headerLength = FileHeaderHelper.GetHeaderLength(text, tag, true);

            Assert.IsTrue(headerLength == expectedLength, $"Expecting {expectedLength}, found {headerLength}");
        }

        [TestCase("//", "//  header \r\nnamespace\r\npublic class Test\r\n", 12)]
        [TestCase("//", "//  header \r\n// more header \r\n some code\r\n", 28)]
        [TestCase("#", "# some header\r\nnamespace\r\npublic class Test\r\n", 14)]
        [TestCase("'", "' some header\r\nnamespace\r\npublic class Test\r\n", 14)]
        public void GetHeaderLengthMultiSingleLineWithCode(string tag, string text, int expectedLength)
        {
            var headerLength = FileHeaderHelper.GetHeaderLength(text, tag, false);

            Assert.IsTrue(headerLength == expectedLength, $"Expecting {expectedLength}, found {headerLength}");
        }

        [TestCase("//", "using System;\r\n\r\n//  header \r\nnamespace System.Windows;\r\npublic class Test\r\n", 13)]
        [TestCase("//", "using EnvDTE;\r\nusing System;\r\nusing SteveCadwallader.CodeMaid.Helpers;\r\n\r\n\r\n//  header \r\n// more header \r\nnamespace SteveCadwallader.CodeMaid;\r\n", 29)]
        [TestCase("//", "using System;\r\n\r\n//  header \r\n[assembly: AssemblyTitle(\"SteveCadwallader.CodeMaid.UnitTests\")]\r\nnamespace ", 13)]
        public void GetHeaderLengthMultiSingleLineWithCodeSkipUsings(string tag, string text, int expectedLength)
        {
            var headerLength = FileHeaderHelper.GetHeaderLength(text, tag, true);

            Assert.IsTrue(headerLength == expectedLength, $"Expecting {expectedLength}, found {headerLength}");
        }

        [TestCase("//", "\r\n//  header \r\n// header\r\nnamespace\r\n//not header\r\n public class Test\r\n", 23)]
        [TestCase("//", "\r\n\r\n\r\n//  header \r\n// more header \r\n some code\r\n", 31)]
        [TestCase("#", "\r\n# some header\r\nnamespace\r\npublic class Test\r\n", 15)]
        [TestCase("'", "\r\n' some header\r\nnamespace\r\npublic class Test\r\n", 15)]
        public void GetHeaderLengthMultiSingleLineWithEmptyLines(string tag, string text, int expectedLength)
        {
            var headerLength = FileHeaderHelper.GetHeaderLength(text, tag, false);

            Assert.IsTrue(headerLength == expectedLength, $"Expecting {expectedLength}, found {headerLength}");
        }

        [TestCase("//", "using System;\r\n\r\n\r\n//  header \r\n// header\r\nnamespace \r\n//not header\r\n public class Test\r\n", 23)]
        [TestCase("//", "using EnvDTE;\r\nusing System;\r\nusing SteveCadwallader.CodeMaid.Helpers;\r\n//  header \r\n// more header \r\n namespace System.Text;\r\n{\r\n", 29)]
        public void GetHeaderLengthMultiSingleLineWithEmptyLinesSkipUsings(string tag, string text, int expectedLength)
        {
            var headerLength = FileHeaderHelper.GetHeaderLength(text, tag, true);

            Assert.IsTrue(headerLength == expectedLength, $"Expecting {expectedLength}, found {headerLength}");
        }

        [TestCase("using ", "", "", 0)]
        [TestCase(" using", "using System;\r\nnamespace CodeMaid", "namespace ", 0)]
        [TestCase("using ", " using System;\r\nnamespace CodeMaid", "namespace ", 1)]
        [TestCase("using ", "using System;\r\nusing System.Collections;\r\n\r\nnamespace CodeMaid", "namespace ", 2)]
        [TestCase("using ", "using System;\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\nusing System.Collections;\r\n\r\nnamespace CodeMaid", "namespace ", 10)]
        public void GetNbLinesToSkip(string patternToFind, string text, string limit, int expectedNbLines)
        {
            var nbLines = FileHeaderHelper.GetNbLinesToSkip(patternToFind, text, new List<string>() { limit });

            Assert.IsTrue(nbLines == expectedNbLines, $"Expecting {expectedNbLines}, found {expectedNbLines}");
        }
    }
}