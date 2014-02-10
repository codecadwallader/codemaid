#region CodeMaid is Copyright 2007-2013 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2013 Steve Cadwallader.

using System.Collections.Generic;
using System.Linq;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A phrase represents an individual block of comment text that will always start on a new line
    /// and could span multiple lines.
    /// </summary>
    internal class CodeCommentPhrase : ICodeCommentPhrase
    {
        #region Constructors

        public CodeCommentPhrase()
        {
            ListPrefix = null;
            Words = new LinkedList<string>();
            OnOwnLine = false;
        }

        /// <param name="words">
        /// The collection of words to build this phrase from. Use <c>null</c> to force this to be
        /// an empty line.
        /// </param>
        /// <param name="listPrefix">
        /// If this phrase is a list, this is the prefix string. Use <c>null</c> if the phrase is
        /// not a list.
        /// </param>
        public CodeCommentPhrase(IEnumerable<string> words, string listPrefix)
            : this()
        {
            ListPrefix = listPrefix;
            
            // Must check if no words but also no list, because the list prefix also is used to make
            // horizontal lines. Otherwise it would create an extra empty line.
            if (words == null && !IsList)
                OnOwnLine = true;
            else
                AppendWords(words);
        }

        #endregion Constructors

        #region Properties

        public bool IsEmpty { get { return Words.First == null; } }

        public bool IsList { get { return ListPrefix != null; } }

        public bool OnOwnLine { get; private set; }

        public string ListPrefix { get; private set; }

        public LinkedList<string> Words { get; private set; }

        /// <summary>
        /// Gets the total length in characters of this phrase.
        /// </summary>
        /// <remarks>
        /// This value is calculated on the fly, use with care.
        /// </remarks>
        public int Length
        {
            get
            {
                return Words.Sum(w => w.Length + 1) - 1;
            }
        }

        #endregion Properties

        #region Methods

        public CodeCommentPhrase Append(CodeCommentPhrase other)
        {
            if (other == null)
                return this;

            if (other.IsList)
            {
                if (!this.IsEmpty || this.IsList)
                    System.Diagnostics.Debug.Fail("Appending a phrase that is a list to an existing phrase is most likely wrong.");
                else
                    this.ListPrefix = other.ListPrefix;
            }

            AppendWords(other.Words);

            return this;
        }

        public void AppendWords(string word)
        {
            if (!string.IsNullOrWhiteSpace(word))
                Words.AddLast(word);
        }

        public void AppendWords(IEnumerable<string> words)
        {
            if (words == null) 
                return;
            
            foreach (var w in words)
            {
                AppendWords(w);
            }
        }

        internal bool CanAppend(CodeCommentPhrase other, bool indented)
        {
            if (other == null)
                return true;

            if (other.IsList && !this.IsEmpty && !this.OnOwnLine)
                return false;

            if (this.OnOwnLine || other.OnOwnLine)
                return false;

            if (this.IsList && !indented)
                return false;

            return true;
        }

        #endregion Methods
    }
}