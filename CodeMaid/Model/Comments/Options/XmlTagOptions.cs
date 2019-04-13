using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.Model.Comments.Options
{
    public class XmlTagOptions : IXmlTagOptions
    {
        public XmlTagOptions()
        {
        }

        public XmlTagOptions(FormatterOptionsXmlTag tag, IXmlTagOptions fallback)
        {
            Case = tag.Case != XmlTagCase.Default ? tag.Case : fallback.Case != XmlTagCase.Default ? fallback.Case : XmlTagCase.Keep;
            Indent = tag.Indent ?? fallback.Indent;
            KeepTogether = tag.KeepTogether ?? fallback.KeepTogether;
            Literal = tag.Literal ?? false;
            SpaceContent = tag.SpaceContent ?? fallback.SpaceContent;
            SpaceSelfClosing = tag.SpaceSelfClosing ?? fallback.SpaceSelfClosing;
            Split = tag.Split != XmlTagNewLine.Default ? tag.Split : fallback.Split != XmlTagNewLine.Default ? fallback.Split : XmlTagNewLine.Content;
        }

        public XmlTagCase Case { get; set; }

        public int Indent { get; set; }

        public bool KeepTogether { get; set; }

        public bool Literal { get; set; }

        public bool SpaceContent { get; set; }

        public bool SpaceSelfClosing { get; set; }

        public XmlTagNewLine Split { get; set; }

        internal static XmlTagOptions FromSettings(Settings settings)
        {
            return new XmlTagOptions
            {
                Case = settings.Formatting_CommentXmlTagsToLowerCase ? XmlTagCase.LowerCase : XmlTagCase.Keep,
                Indent = settings.Formatting_CommentXmlValueIndent,
                KeepTogether = settings.Formatting_CommentXmlKeepTagsTogether,
                Literal = false,
                SpaceContent = settings.Formatting_CommentXmlSpaceTags,
                SpaceSelfClosing = settings.Formatting_CommentXmlSpaceSingleTags,
                Split = settings.Formatting_CommentXmlSplitAllTags ? XmlTagNewLine.Always : XmlTagNewLine.Content
            };
        }
    }
}