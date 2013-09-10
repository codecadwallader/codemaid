#region CodeMaid is Copyright 2007-2013 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2013 Steve Cadwallader.

using System;
using System.Collections.Generic;
using System.Linq;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A phrase represents an individual block of comment text that will always start on a new line
    /// and could span multiple lines.
    /// </summary>
    internal class CodeCommentPhrase
    {
        #region Constructors

        public CodeCommentPhrase()
        {
            Indent = 0;
            ListPrefix = null;
            Words = new LinkedList<string>();
        }

        public CodeCommentPhrase(int indent, string listPrefix, IEnumerable<string> words)
            : this()
        {
            Indent = indent;
            ListPrefix = listPrefix;

            AppendWords(words);
        }

        #endregion Constructors

        #region Properties

        public int Indent { get; private set; }

        public bool IsList { get { return ListPrefix != null; } }

        public bool IsXml { get; set; }

        public string ListPrefix { get; private set; }

        public LinkedList<string> Words { get; private set; }

        #endregion Properties

        #region Methods

        public CodeCommentPhrase Append(CodeCommentPhrase other)
        {
            if (other != null)
            {
                AppendWords(other.Words);
            }

            return this;
        }

        public void AppendWords(string word)
        {
            AppendWords(new[] { word });
        }

        public void AppendWords(IEnumerable<string> words)
        {
            if (words == null) return;
            foreach (var w in words.Where(w => !string.IsNullOrEmpty(w)))
            {
                Words.AddLast(w);
            }
        }

        #endregion Methods
    }
}