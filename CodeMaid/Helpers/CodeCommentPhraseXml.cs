using System.Collections.Generic;

namespace SteveCadwallader.CodeMaid.Helpers
{
    internal class CodeCommentPhraseXml : ICodeCommentPhrase
    {
        public CodeCommentPhraseXml(string openTag, string closeTag)
        {
            this.OpenTag = openTag;
            this.CloseTag = closeTag;

            this.Phrases = new LinkedList<ICodeCommentPhrase>();
        }

        public string CloseTag { get; private set; }

        public string OpenTag { get; private set; }

        public LinkedList<ICodeCommentPhrase> Phrases { get; private set; }

        internal void Add(LinkedList<ICodeCommentPhrase> phrases)
        {
            if (phrases != null)
            {
                foreach (var phrase in phrases)
                {
                    this.Phrases.AddLast(phrase);
                }
            }
        }
    }
}