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

using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A <c>CodeComment</c> contains one or more <see cref="CodeCommentPhrase">Phrases</see>
    /// which represent all the content of a comment.
    /// </summary>
    internal class CodeComment
    {
        private static string[] MajorXmlTags = { "summary", "remarks", "example" };
        private static string[] MinorXmlTags = { "param", "exception", "returns", "value" };
        private static Regex WordSplitRegex = new Regex(@"\s+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex XmlTagRegex = new Regex(@"^<(?<closetag>\/)?(?<tag>(" + String.Join("|", MajorXmlTags.Union(MinorXmlTags)) + ")).*>$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private EditPoint _cursor;
        private Regex CommentLineRegex;

        public CodeComment(string commentPrefix, ref EditPoint from, ref EditPoint to)
        {
            this.CommentLineRegex = new Regex(String.Format(@"(?<commentprefix>{0}) (?<indent>\s*)(?<listprefix>-|\w+\))?\s*(?<words>.*?)(\r?\n|$)", commentPrefix), RegexOptions.Compiled | RegexOptions.IgnoreCase);
            this.StartPoint = from.CreateEditPoint();
            this.EndPoint = to.CreateEditPoint();
            this.LineCharOffset = from.LineCharOffset;

            this.Phrases = new LinkedList<CodeCommentPhrase>();

            string text = from.GetText(to);

            foreach (Match match in CommentLineRegex.Matches(text))
            {
                var indent = match.Groups["indent"].Success ? match.Groups["indent"].Length : 0;
                var listPrefix = match.Groups["listprefix"].Success ? match.Groups["listprefix"].Value : null;
                var words = WordSplitRegex.Split(match.Groups["words"].Value);

                if (Phrases.First == null)
                {
                    this.CommentPrefix = match.Groups["commentprefix"].Value;
                    this.IsXmlComment = this.CommentPrefix.Length == 3 && words[0].StartsWith("<");
                }

                AddPhrase(new CodeCommentPhrase(indent, listPrefix, words));
            }
        }

        public string CommentPrefix { get; private set; }

        public EditPoint EndPoint { get; private set; }

        public bool IsXmlComment { get; private set; }

        public int LineCharOffset { get; private set; }

        public LinkedList<CodeCommentPhrase> Phrases { get; private set; }

        public EditPoint StartPoint { get; private set; }

        /// <summary>
        /// Add add a phrase to this comment.
        /// </summary>
        /// <param name="value">The phrase to add.</param>
        public void AddPhrase(CodeCommentPhrase value)
        {
            if (Phrases.Last == null || value.IsList || (Phrases.Last.Value.IsList && Phrases.Last.Value.Indent != value.Indent))
                Phrases.AddLast(value);
            else
                Phrases.Last.Value.Append(value);
        }

        /// <summary>
        /// Write the new comment text to the document.
        /// </summary>
        /// <param name="maxWidth"> The right margin to adhere to. </param>
        /// <returns>The endpoint of the comment.</returns>
        public EditPoint Output(int maxWidth)
        {
            if (maxWidth < LineCharOffset + 20)
                maxWidth = LineCharOffset + 20;

            _cursor = StartPoint.CreateEditPoint();

            _cursor.Delete(EndPoint);

            if (IsXmlComment)
                ReformatXmlPhrases();

            // Loop through each phrase.
            var phrase = Phrases.First;

            while (phrase != null)
            {
                _cursor.Insert(CommentPrefix);

                // Phrase is a list, so output the list prefix before the first word.
                if (phrase.Value.IsList)
                {
                    _cursor.Insert(" ");
                    _cursor.Insert(phrase.Value.ListPrefix);
                }

                // Loop through each word.
                var word = phrase.Value.Words.First;
                while (word != null)
                {
                    // Create newline if next word no longer fits on this line, but keep in
                    // mind some words can by themself already be too long to fit on a line.
                    if (_cursor.LineCharOffset + word.Value.Length > maxWidth && word.Value.Length < maxWidth)
                    {
                        WriteNewCommentLine(true);

                        // If the current phrase is a list, add extra spacing to create pretty
                        // alignment of list items.
                        if (phrase.Value.IsList)
                            _cursor.Insert("".PadLeft(phrase.Value.ListPrefix.Length + 1, ' '));
                    }

                    // This is were we write the actual word.
                    _cursor.Insert(" ");
                    _cursor.Insert(word.Value);

                    word = word.Next;
                }

                phrase = phrase.Next;

                // If on a comment phrase, and there will be another phrase, add a newline.
                if (phrase != null)
                    WriteNewCommentLine(false);
            }

            EndPoint = _cursor.CreateEditPoint();

            return _cursor;
        }

        /// <summary>
        /// Get all words including and after current word.
        /// </summary>
        private IEnumerable<string> GetWordsAfter(LinkedListNode<string> word)
        {
            while ((word = word.Next) != null)
                yield return word.Value;
        }

        /// <summary>
        /// Check for XML tags in phrases and make sure they follow the rules for newlines.
        /// </summary>
        private void ReformatXmlPhrases()
        {
            var phrase = Phrases.First;
            while (phrase != null)
            {
                if (!phrase.Value.IsList)
                {
                    var word = phrase.Value.Words.First;
                    while (word != null)
                    {
                        var match = XmlTagRegex.Match(word.Value);
                        if (match.Success)
                        {
                            var tagName = match.Groups["tag"].Value;
                            bool isCloseTag = match.Groups["closetag"].Success;
                            bool isMajorTag = MajorXmlTags.Contains(tagName);

                            // Major tags and minor opening tags should be the first word.
                            if (word.Previous != null && (isMajorTag || (!isCloseTag && MinorXmlTags.Contains(tagName))))
                            {
                                // Previous word will be the last word of this phrase.
                                word = word.Previous;

                                // Create a new phrase with the rest of the words.
                                Phrases.AddAfter(phrase, new CodeCommentPhrase(
                                    phrase.Value.Indent,
                                    phrase.Value.ListPrefix,
                                    GetWordsAfter(word)));

                                // Remove the rest.
                                while (word.Next != null)
                                    word.List.Remove(word.Next);
                            }

                            // Major tags should be the last word.
                            if (word.Next != null && isMajorTag)
                            {
                                Phrases.AddAfter(phrase, new CodeCommentPhrase(
                                    phrase.Value.Indent,
                                    phrase.Value.ListPrefix,
                                    GetWordsAfter(word)));

                                while (word.Next != null)
                                    word.List.Remove(word.Next);
                            }
                        }

                        word = word.Next;
                    }
                }

                phrase = phrase.Next;
            }
        }

        /// <summary>
        /// Write a newline at the <c>StartPoint</c> and add indenting.
        /// </summary>
        /// <param name="resumeComment"> If set to <c>true</c> it will also write <c>CommentPrefix</c>. </param>
        private void WriteNewCommentLine(bool resumeComment = false)
        {
            _cursor.Insert(Environment.NewLine);
            _cursor.PadToColumn(LineCharOffset);
            if (resumeComment)
                _cursor.Insert(CommentPrefix);
        }
    }
}