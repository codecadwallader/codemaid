namespace SteveCadwallader.CodeMaid.Model.Comments
{
    internal class CommentLine : ICommentLine
    {
        #region Constructors

        public CommentLine(string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
            {
                this.Content = content;
            }
        }

        #endregion Constructors

        #region Properties

        public string Content { get; protected set; }

        #endregion Properties
    }
}