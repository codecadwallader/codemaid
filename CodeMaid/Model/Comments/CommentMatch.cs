using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SteveCadwallader.CodeMaid.Model.Comments
{
    internal class CodeCommentMatch
    {
        #region Constructors

        public CodeCommentMatch(Match match)
        {
            if (!match.Success)
            {
                System.Diagnostics.Debug.Fail("Cannot parse a line that does not match the comment regex.");
                return;
            }

            Indent = match.Groups["indent"].Success ? match.Groups["indent"].Value.Length : 0;
            ListPrefix = match.Groups["listprefix"].Success ? match.Groups["listprefix"].Value : null;
            Words = match.Groups["words"].Success ? match.Groups["words"].Captures.OfType<Capture>().Select(c => c.Value).ToList() : null;

            IsEmpty = string.IsNullOrWhiteSpace(match.Value) || Words == null || Words.Count < 1;
            IsList = !string.IsNullOrWhiteSpace(ListPrefix);

            // In the case of a list prefix but no content (e.g. hyphen line) convert to regular content.
            if (IsEmpty && IsList)
            {
                Words = new List<string>(new[] { ListPrefix });
                ListPrefix = null;
                IsEmpty = false;
                IsList = false;
            }
        }

        #endregion Constructors

        #region Properties

        public int Indent { get; private set; }

        public bool IsEmpty { get; private set; }

        public bool IsList { get; private set; }

        public int Length
        {
            get
            {
                return Indent - 1 +
                    Words.Sum(w => w.Length + 1) +
                    (IsList ? ListPrefix.Length + 1 : 0);
            }
        }

        public string ListPrefix { get; private set; }

        public IList<string> Words { get; private set; }

        #endregion Properties

        #region Methods

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

            if (IsEmpty || other.IsEmpty)
                return false;

            if (other.IsList)
                return false;

            if (IsList && other.Indent < 1)
                return false;

            foreach (var word in other.Words)
                Words.Add(word);

            IsEmpty = Words.Count < 1;

            return true;
        }

        #endregion Methods
    }
}