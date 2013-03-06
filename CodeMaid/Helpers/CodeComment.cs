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
        private static Regex CommentLineRegex = new Regex(@"(?<indent>\s*(?<listprefix>-|\w+[\)\.:])?\s*)(?<words>.*?)(\r?\n|$)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex WordSplitRegex = new Regex(@"\s+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private EditPoint _cursor;
        private CachedSettingSet<string> _majorTags;
        private CachedSettingSet<string> _minorTags;
        private Regex XmlTagRegex;

        public CodeComment(string commentRegex, ref EditPoint from, ref EditPoint to, CachedSettingSet<string> majorTags, CachedSettingSet<string> minorTags)
        {
            this.StartPoint = from.CreateEditPoint();
            this.EndPoint = to.CreateEditPoint();
            this.LineCharOffset = from.LineCharOffset;

            this._majorTags = majorTags;
            this._minorTags = minorTags;

            this.Phrases = new LinkedList<CodeCommentPhrase>();

            string text = from.GetText(to);
            var commentMatch = Regex.Match(text, commentRegex, RegexOptions.IgnoreCase);

            this.CommentPrefix = commentMatch.Groups["prefix"].Value;
            
            // Scary workaround for blockcomments.
            if (CommentPrefix == "/*")
            {
                this.IsBlockComment = true;
                this.CommentPrefix = " *";
            }

            foreach (Capture c in commentMatch.Groups["line"].Captures)
            {
                var lineMatch = CommentLineRegex.Match(c.Value);

                var indent = lineMatch.Groups["indent"].Success ? lineMatch.Groups["indent"].Length : 0;
                var listPrefix = lineMatch.Groups["listprefix"].Success ? lineMatch.Groups["listprefix"].Value : null;
                var words = WordSplitRegex.Split(lineMatch.Groups["words"].Value);
                
                // Empty lines return a single empty string from split, null makes this easier to 
                // deal with.
                if (words[0].Length == 0)
                    words = null;

                if (Phrases.First == null && words != null)
                    this.IsXmlComment = this.CommentPrefix.Length == 3 && words.First().StartsWith("<");

                if (
                    Phrases.Last == null // No phrases yet
                    || !String.IsNullOrEmpty(listPrefix) // Lists always starts on own line
                    || (Phrases.Last.Value.IsList && indent <= 1) // Previous line is list but this line is not indented
                    || (words == null && Phrases.Last.Value.Words.First != null) // This is an empty line and previous one is not
                    || (words != null && Phrases.Last.Value.Words.First == null) // This is a normal line and previous one is empty
                    )
                    Phrases.AddLast(new CodeCommentPhrase(indent, listPrefix, words));
                else
                    Phrases.Last.Value.AppendWords(words);
            }
        }

        public string CommentPrefix { get; private set; }

        public EditPoint EndPoint { get; private set; }

        public bool IsBlockComment { get; private set; }

        public bool IsXmlComment { get; private set; }

        public int LineCharOffset { get; private set; }

        public LinkedList<CodeCommentPhrase> Phrases { get; private set; }

        public EditPoint StartPoint { get; private set; }

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
                // Start of Block comment
                if (IsBlockComment && phrase.Previous == null)
                    _cursor.Insert("/*");
                else
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

                if (phrase != null)
                {
                    // On a comment phrase, and there will be another phrase.
                    WriteNewCommentLine(false);
                }
                else if (IsBlockComment)
                {
                    // End the block comment
                    _cursor.Insert(" */");
                }
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
            this.XmlTagRegex = new Regex(@"(?<before>.+?)?\s*(?<fulltag><\/?(?<tagname>(" + String.Join("|", _majorTags.Value.Union(_minorTags.Value)) + @")).*>)\s*(?<after>.+)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);

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
                            // Text directly before this tag goes in current phrase.
                            if (match.Groups["before"].Success)
                                word.List.AddBefore(word, match.Groups["before"].Value);
                            if (match.Groups["after"].Success)
                                word.List.AddAfter(word, match.Groups["after"].Value);
                            if (match.Groups["fulltag"].Success)
                                word.Value = match.Groups["fulltag"].Value;

                            var tagName = match.Groups["tagname"].Value;
                            bool isCloseTag = word.Value.StartsWith("</");
                            bool isMajorTag = _majorTags.Value.Contains(tagName);

                            // Major tags and minor opening tags should be the first word.
                            if (word.Previous != null && (isMajorTag || !isCloseTag))
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

                            // Remove spacing between word and minor tags.
                            if (!isMajorTag)
                            {
                                if (isCloseTag && word.Previous != null)
                                {
                                    word.Value = word.Previous.Value + word.Value;
                                    word.List.Remove(word.Previous);
                                }
                                else if (!isCloseTag && word.Next != null)
                                {
                                    word.Value += word.Next.Value;
                                    word.List.Remove(word.Next);
                                }
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