using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A phrase represents an individual block of comment text that will always start on a new
    /// line and could span multiple lines.
    /// </summary>
    internal class CodeCommentPhrase
    {
        private static Regex ListRegex = new Regex(@"^(?<indent>\s*(?<prefix>-|\w+\))?\s+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex SplitRegex = new Regex(@"\s+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public CodeCommentPhrase()
        {
            this.Words = new LinkedList<string>();
            this.Indent = 0;
        }

        public CodeCommentPhrase(string value)
            : this()
        {
            var match = ListRegex.Match(value);
            if (match.Success)
            {
                if (match.Groups["prefix"].Success)
                {
                    ListPrefix = String.Format("{0}", match.Groups["prefix"].Value);
                    IsList = true;
                }
                if (match.Groups["indent"].Success)
                {
                    Indent = match.Groups["indent"].Value.Length;
                    value = value.Substring(Indent);
                }
            }

            AppendString(value);
        }

        public CodeCommentPhrase(int indent, bool isList, string listPrefix, IEnumerable<string> words)
            : this()
        {
            this.Indent = indent;
            this.IsList = IsList;
            this.ListPrefix = listPrefix;
            AppendWords(words);
        }

        public int Indent { get; private set; }

        public bool IsList { get; private set; }

        public string ListPrefix { get; private set; }

        public LinkedList<string> Words { get; private set; }

        public CodeCommentPhrase Append(CodeCommentPhrase other)
        {
            if (other != null)
                AppendWords(other.Words);
            return this;
        }

        public CodeCommentPhrase AppendString(string s)
        {
            // s.Split(new[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries)
            if (s != null)
                AppendWords(SplitRegex.Split(s));
            return this;
        }

        private void AppendWords(IEnumerable<string> words)
        {
            int depth = 0;
            LinkedListNode<string> node = null;

            foreach (var w in words.Where(w => !String.IsNullOrEmpty(w)))
            {
                // Trickery to keep multiple-word XML tags (eg. with attributes) together.
                int open = w.Count(c => c == '<');
                int close = w.Count(c => c == '>');

                if (node != null)
                    node.Value = String.Format("{0} {1}", node.Value, w);
                else
                    node = Words.AddLast(w);

                depth += (open - close);

                if (depth <= 0)
                {
                    node = null;
                    depth = 0;
                }
            }
        }
    }
}