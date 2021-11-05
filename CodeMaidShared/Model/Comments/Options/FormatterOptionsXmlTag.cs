namespace SteveCadwallader.CodeMaid.Model.Comments.Options
{
    public class FormatterOptionsXmlTag
    {
        /// <summary>
        /// If not <see cref="XmlTagCase.Default"/>, overrides the default tag case setting.
        /// </summary>
        public XmlTagCase Case { get; set; }

        /// <summary>
        /// If not <c>null</c>, overrides the default tag indentation.
        /// </summary>
        public int? Indent { get; set; }

        public bool? KeepTogether { get; set; }

        /// <summary>
        /// Whether the content should be kept literally and not formatted.
        /// </summary>
        public bool? Literal { get; set; }

        public bool? SpaceContent { get; set; }

        public bool? SpaceSelfClosing { get; set; }

        /// <summary>
        /// If not <see cref="XmlTagNewLine.Default"/>, overrides the default tag split setting.
        /// </summary>
        public XmlTagNewLine Split { get; set; }
    }
}