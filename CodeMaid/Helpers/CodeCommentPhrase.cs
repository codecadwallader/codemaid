#region CodeMaid is Copyright 2007-2012 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2012 Steve Cadwallader.

using System;
using System.Collections.Generic;
using System.Linq;
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
                // Trickery to keep multiple-word XML tags (eg. with attributes) together in a single word.
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