using System.Text.RegularExpressions;

namespace SteveCadwallader.CodeMaid.Model.Comments.Options
{
    internal interface ICommentOptions
    {
        string Prefix { get; }

        Regex Regex { get; }
    }
}