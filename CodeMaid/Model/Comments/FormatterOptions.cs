namespace SteveCadwallader.CodeMaid.Model.Comments
{
    /// <summary>
    /// Document wide options for the comment formatter.
    /// </summary>
    internal class FormatterOptions
    {
        public int TabSize { get; internal set; }

        /// <summary>
        /// The list of comment prefix tokens to ignore while formatting the comment.
        /// </summary>
        public string[] IgnoreTokens { get; internal set; }
    }
}