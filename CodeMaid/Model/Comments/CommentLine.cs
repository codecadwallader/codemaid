namespace SteveCadwallader.CodeMaid.Model.Comments
{
    internal class CommentLine : ICommentLine
    {
        public CommentLine(string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                this.Content = content;
            }
        }

        public string Content { get; protected set; }

        public bool IsLast { get; internal set; }
    }
}