using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using SteveCadwallader.CodeMaid.Helpers;
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
        public void GetHeaderLengthLanguage(CodeLanguage language, string docStart)
        {
            var headerLength = FileHeaderHelper.GetHeaderLength(language, docStart);

            Assert.IsTrue(headerLength > 0);
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
        public void GetHeaderLengthLanguageWithWrongTags(CodeLanguage language, string docStart)
        {
            var headerLength = FileHeaderHelper.GetHeaderLength(language, docStart);

            Assert.IsTrue(headerLength == 0);
        }

        [TestCase("/* */", "/*", "*/")]
        [TestCase("/* \r\n   Copyright © 2021 \r\n */", "/*", "*/")]
        [TestCase("<!-- -->", "<!--", "-->")]
        [TestCase("<!-- =========== \r\n \r\n  Copyright © 2021 \r\n \r\n =========== -->", "<!--", "-->")]
        [TestCase("<# #>", "<#", "#>")]
        [TestCase("<# //////////////// \r\n   Copyright © 2021 \r\n //////////////// #>", "<#", "#>")]
        [TestCase("(* *)", "(*", "*)")]
        [TestCase("(* ~~~~ \r\n ~~ \r\n  Copyright © 2021 \r\n ~~ \r\n ~~~~ *)", "(*", "*)")]
        public void GetHeaderLengthMultiLine(string docStart, string tagStart, string beaconEnd)
        {
            var headerLength = FileHeaderHelper.GetHeaderLength(docStart, tagStart, beaconEnd);
            var length = docStart.Length - Regex.Matches(docStart, Environment.NewLine).Count;

            Assert.IsTrue(headerLength == length);
        }

        [TestCase("\r\n/* */", "/*", "*/")]
        [TestCase("\r\n\r\n\r\n/*    Copyright © 2021  */", "/*", "*/")]
        [TestCase("\r\n\r\n<!-- -->", "<!--", "-->")]
        public void GetHeaderLengthMultiLineWithEmptyLines(string docStart, string tagStart, string tagEnd)
        {
            var headerLength = FileHeaderHelper.GetHeaderLength(docStart, tagStart, tagEnd);
            var value = docStart.Length - Regex.Matches(docStart, Environment.NewLine).Count;

            Assert.IsTrue(headerLength == value);
        }

        [TestCase("", "/*", "*/")]
        [TestCase("some text", "/*", "*/")]
        [TestCase("/*     \r\n \r\n      ", "/*", "*/")]
        [TestCase("/* header */", "/*", "*>")]
        [TestCase("*/           ", "/*", "*/")]
        public void GetHeaderLengthMultiLineZero(string docStart, string tagStart, string tagEnd)
        {
            var headerLength = FileHeaderHelper.GetHeaderLength(docStart, tagStart, tagEnd);

            Assert.IsTrue(headerLength == 0);
        }

        [TestCase("//", "// header fdsfndksjnfe\r\n")]
        [TestCase("#", "# header eropf opekfropze jaqozerijfg uihgsdfidnffdsfg\r\n")]
        [TestCase("'", "'header  sdvq qsudg bqudgybuydzeau _(àç_ç*ù$^$*ùàyguozadgzao\r\n")]
        [TestCase("//", "// ====================\r\n// Copyright\r\n// ====================\r\n")]
        [TestCase("#", "# ==============\r\n#   Copyright !\r\n#    ~~\r\n# ==============\r\n")]
        [TestCase("'", "' ==============\r\n' some text\r\n'   Copyright !\r\n' ==============\r\n")]
        public void GetHeaderLengthMultiSingleLine(string tag, string docStart)
        {
            var headerLength = FileHeaderHelper.GetHeaderLength(docStart, tag);
            var length = docStart.Length - Regex.Matches(docStart, Environment.NewLine).Count;

            Assert.IsTrue(headerLength == length);
        }

        [TestCase("//", "//  header \r\nnamespace\r\npublic class Test\r\n", 12)]
        [TestCase("//", "//  header \r\n// more header \r\n some code\r\n", 28)]
        [TestCase("#", "# some header\r\nnamespace\r\npublic class Test\r\n", 14)]
        [TestCase("'", "' some header\r\nnamespace\r\npublic class Test\r\n", 14)]
        public void GetHeaderLengthMultiSingleLineWithCode(string tag, string docStart, int expectedLength)
        {
            var headerLength = FileHeaderHelper.GetHeaderLength(docStart, tag);

            Assert.IsTrue(headerLength == expectedLength);
        }

        [TestCase("//", "\r\n//  header \r\n// header\r\nnamespace\r\n//not header\r\n public class Test\r\n", 23)]
        [TestCase("//", "\r\n\r\n\r\n//  header \r\n// more header \r\n some code\r\n", 31)]
        [TestCase("#", "\r\n# some header\r\nnamespace\r\npublic class Test\r\n", 15)]
        [TestCase("'", "\r\n' some header\r\nnamespace\r\npublic class Test\r\n", 15)]
        public void GetHeaderLengthMultiSingleLineWithEmptyLines(string tag, string docStart, int expectedLength)
        {
            var headerLength = FileHeaderHelper.GetHeaderLength(docStart, tag);

            Assert.IsTrue(headerLength == expectedLength);
        }
    }
}