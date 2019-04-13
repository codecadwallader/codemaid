using SteveCadwallader.CodeMaid.Properties;
using System;

namespace SteveCadwallader.CodeMaid.Model.Comments.Options
{
    /// <summary>
    /// Document wide options for the comment formatter.
    /// </summary>
    public class FormatterOptions
    {
        /// <summary>
        /// The list of comment prefix tokens to ignore while formatting the comment.
        /// </summary>
        public string[] IgnoreTokens { get; set; }

        /// <summary>
        /// Do not wrap to a newline for only a single word.
        /// </summary>
        public bool SkipWrapOnLastWord { get; set; }

        public int TabSize { get; set; } = 4;

        public int WrapColumn { get; set; }

        public FormatterOptionsXml Xml { get; set; }

        internal static FormatterOptions FromSettings(Settings settings)
        {
            return new FormatterOptions
            {
                WrapColumn = settings.Formatting_CommentWrapColumn,
                SkipWrapOnLastWord = settings.Formatting_CommentSkipWrapOnLastWord,
                Xml = FormatterOptionsXml.FromSettings(settings)
            };
        }

        internal FormatterOptions Set(Action<FormatterOptions> action)
        {
            action?.Invoke(this);
            return this;
        }
    }
}