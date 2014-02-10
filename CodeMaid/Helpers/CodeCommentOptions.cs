using System;
using EnvDTE;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// Helper class to store the options passed to the comment formatter.
    /// </summary>
    internal class CodeCommentOptions
    {
        public int TabSize { get; set; }

        public int WrapAtColumn { get; set; }

        public int XmlValueIndent { get; set; }

        public bool XmlSpaceTags { get; set; }

        public bool XmlAlignParamTags { get; set; }

        public bool SkipWrapOnLastWord { get; set; }

        public CodeCommentOptions()
        {
        }

        public CodeCommentOptions(CodeMaidPackage package, TextDocument document)
        {
            SkipWrapOnLastWord = Settings.Default.Cleaning_CommentSkipWrapOnLastWord;
            TabSize = CodeCommentHelper.GetTabSize(package, document);
            WrapAtColumn = Math.Max(Settings.Default.Cleaning_CommentWrapColumn, 20);

            XmlValueIndent = Settings.Default.Cleaning_CommentXmlValueIndent;
            XmlSpaceTags = Settings.Default.Cleaning_CommentXmlSpaceTags;
            XmlAlignParamTags = Settings.Default.Cleaning_CommentXmlAlignParamTags;
        }
    }
}