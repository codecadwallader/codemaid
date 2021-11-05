namespace SteveCadwallader.CodeMaid.Model.Comments.Options
{
    public interface IXmlTagOptions
    {
        XmlTagCase Case { get; }

        int Indent { get; }

        bool KeepTogether { get; }

        bool Literal { get; }

        bool SpaceContent { get; }

        bool SpaceSelfClosing { get; }

        XmlTagNewLine Split { get; }
    }
}