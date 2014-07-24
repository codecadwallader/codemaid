#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SteveCadwallader.CodeMaid.Model.Comments
{
    internal class CodeCommentMatch
    {
        public CodeCommentMatch(Match match)
        {
            if (!match.Success)
            {
                System.Diagnostics.Debug.Fail("Cannot parse a line that does not match the comment regex.");
                return;
            }

            this.Indent = match.Groups["indent"].Success ? match.Groups["indent"].Value.Length : 0;
            this.ListPrefix = match.Groups["listprefix"].Success ? match.Groups["listprefix"].Value : null;
            this.Words = match.Groups["words"].Success ? match.Groups["words"].Captures.OfType<Capture>().Select(c => c.Value).ToList() : null;

            this.IsEmpty = string.IsNullOrWhiteSpace(match.Value) || this.Words.Count < 1;
            this.IsList = !string.IsNullOrWhiteSpace(this.ListPrefix);
        }

        public int Indent { get; private set; }

        public bool IsEmpty { get; private set; }

        public bool IsList { get; private set; }

        public int Length
        {
            get
            {
                return this.Indent - 1 +
                    this.Words.Sum(w => w.Length + 1) +
                    (IsList ? ListPrefix.Length + 1 : 0);
            }
        }

        public string ListPrefix { get; private set; }

        public IList<string> Words { get; private set; }

        /// <summary>
        /// Attempt to combine another match with this match. If possible, all words from the other
        /// match are appended to this match and the other match can be removed.
        /// </summary>
        /// <param name="other">The match to append.</param>
        /// <returns><c>true</c> if the matches were combined, otherwise <c>false</c>.</returns>
        public bool TryAppend(CodeCommentMatch other)
        {
            if (other == null)
                return false;

            if (this.IsEmpty || other.IsEmpty)
                return false;

            if (other.IsList)
                return false;

            if (this.IsList && other.Indent < 1)
                return false;

            foreach (var word in other.Words)
                this.Words.Add(word);

            this.IsEmpty = this.Words.Count < 1;

            return true;
        }
    }
}