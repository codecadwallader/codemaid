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
using System.Text;
using System.Text.RegularExpressions;
using EnvDTE;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A <c>CodeComment</c> contains one or more <see cref="CodeCommentPhrase">phrases</see>
    /// which represent all the content of a comment.
    /// </summary>
    internal class CodeComment
    {
        #region Fields

        private static readonly Regex CommentLineRegex = new Regex(@"(?<indent>\s*(?<listprefix>-|\w+[\):]|\d+\.)?\s*)(?<words>.*?)(\r?\n|$)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex WordSplitRegex = new Regex(@"\s+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private readonly CachedSettingSet<string> _majorTags;
        private readonly CachedSettingSet<string> _minorTags;
        private readonly IndentSettings _indentSettings;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeComment"/> class.
        /// </summary>
        /// <param name="commentRegex">The comment regular expression.</param>
        /// <param name="from">The start point.</param>
        /// <param name="to">The end point.</param>
        /// <param name="majorTags">The major tags.</param>
        /// <param name="minorTags">The minor tags.</param>
        /// <param name="indentSettings">The indent settings.</param>
        public CodeComment(string commentRegex, ref EditPoint from, ref EditPoint to, CachedSettingSet<string> majorTags, CachedSettingSet<string> minorTags, IndentSettings indentSettings)
        {
            StartPoint = from.CreateEditPoint();
            EndPoint = to.CreateEditPoint();
            LineCharOffset = from.LineCharOffset;

            _majorTags = majorTags;
            _minorTags = minorTags;
            _indentSettings = indentSettings;

            Phrases = new LinkedList<CodeCommentPhrase>();

            string text = from.GetText(to);
            var commentMatch = Regex.Match(text, commentRegex, RegexOptions.IgnoreCase);

            CommentPrefix = commentMatch.Groups["prefix"].Value;

            // Workaround for blockcomments.
            if (CommentPrefix == "/*")
            {
                IsBlockComment = true;
                CommentPrefix = " *";
            }

            foreach (Capture c in commentMatch.Groups["line"].Captures)
            {
                var lineMatch = CommentLineRegex.Match(c.Value);

                var indent = lineMatch.Groups["indent"].Success ? lineMatch.Groups["indent"].Length : 0;
                var listPrefix = lineMatch.Groups["listprefix"].Success ? lineMatch.Groups["listprefix"].Value : null;
                var words = WordSplitRegex.Split(lineMatch.Groups["words"].Value);

                // Empty lines return a single empty string from split, null makes this easier to deal with.
                if (words[0].Length == 0)
                {
                    words = null;
                }

                if (Phrases.First == null && words != null)
                {
                    IsXmlComment = CommentPrefix.Length == 3 && words.First().StartsWith("<");
                }

                if (
                    Phrases.Last == null // No phrases yet
                    || !String.IsNullOrEmpty(listPrefix) // Lists always starts on own line
                    || (Phrases.Last.Value.IsList && indent <= 1) // Previous line is list but this line is not indented
                    || (words == null && Phrases.Last.Value.Words.First != null) // This is an empty line and previous one is not
                    || (words != null && Phrases.Last.Value.Words.First == null) // This is a normal line and previous one is empty
                    )
                {
                    Phrases.AddLast(new CodeCommentPhrase(indent, listPrefix, words));
                }
                else
                {
                    Phrases.Last.Value.AppendWords(words);
                }
            }
        }

        #endregion Constructors

        #region Properties

        public string CommentPrefix { get; private set; }

        public EditPoint EndPoint { get; private set; }

        public bool IsBlockComment { get; private set; }

        public bool IsXmlComment { get; private set; }

        public int LineCharOffset { get; private set; }

        public LinkedList<CodeCommentPhrase> Phrases { get; private set; }

        public EditPoint StartPoint { get; private set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Conditionally writes the new comment text to the document.
        /// </summary>
        /// <param name="maxWidth">The maximum width.</param>
        /// <returns>The endpoint of the comment.</returns>
        public EditPoint Output(int maxWidth)
        {
            var commentBuilder = BuildComment(maxWidth);

            if (!commentBuilder.Equals(StartPoint.GetText(EndPoint)))
            {
                var cursor = StartPoint.CreateEditPoint();
                cursor.Delete(EndPoint);
                cursor.Insert(commentBuilder.ToString());
                EndPoint = cursor.CreateEditPoint();
            }

            return EndPoint;
        }

        private CommentBuilder BuildComment(int maxWidth)
        {
            if (maxWidth < LineCharOffset + 20)
            {
                maxWidth = LineCharOffset + 20;
            }

            if (IsXmlComment)
            {
                ReformatXmlPhrases();
            }

            // Output buffer. The magic minus 1 accounts for the difference between cursor position
            // and actual character count.
            var builder = new CommentBuilder(LineCharOffset - 1, CommentPrefix, _indentSettings);

            // Loop through each phrase.
            var phrase = Phrases.First;

            while (phrase != null)
            {
                // Start of Block comment
                if (IsBlockComment && phrase.Previous == null)
                {
                    builder.Insert("/*");
                }
                else
                {
                    builder.Insert(CommentPrefix);
                }

                // Phrase is a list, so output the list prefix before the first word.
                if (phrase.Value.IsList)
                {
                    builder.Insert(" ");
                    builder.Insert(phrase.Value.ListPrefix);
                }

                // Loop through each word.
                var word = phrase.Value.Words.First;
                while (word != null)
                {
                    // Create a newline if the next space and word no longer fit on this line, but
                    // keep in mind some words can by themself already be too long to fit on a
                    // line.
                    if (builder.LineCharOffset + word.Value.Length + 1 > maxWidth && word.Value.Length < maxWidth)
                    {
                        builder.WriteNewCommentLine(true);

                        // If the current phrase is a list, add extra spacing to create pretty
                        // alignment of list items.
                        if (phrase.Value.IsList)
                        {
                            builder.Insert("".PadLeft(phrase.Value.ListPrefix.Length + 1, ' '));
                        }
                    }

                    // This is were we write the actual word.
                    builder.Insert(" ");
                    builder.Insert(word.Value);

                    word = word.Next;
                }

                phrase = phrase.Next;

                if (phrase != null)
                {
                    // On a comment phrase, and there will not be another phrase, write a newline
                    // but do not resume comment.
                    builder.WriteNewCommentLine();
                }
                else if (IsBlockComment)
                {
                    // End the block comment
                    builder.Insert(" */");
                }
            }

            return builder;
        }

        /// <summary>
        /// Get all words including and after current word.
        /// </summary>
        private IEnumerable<string> GetWordsAfter(LinkedListNode<string> word)
        {
            while ((word = word.Next) != null)
            {
                yield return word.Value;
            }
        }

        /// <summary>
        /// Check for XML tags in phrases and make sure they follow the rules for newlines.
        /// </summary>
        private void ReformatXmlPhrases()
        {
            var xmlTagRegex = new Regex(@"(?<before>[^\<]+?)?\s*(?<fulltag><\/?(?<tagname>(" + String.Join("|", _majorTags.Value.Union(_minorTags.Value)) + @")).*?>)\s*(?<after>.+)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            var phrase = Phrases.First;
            while (phrase != null)
            {
                if (!phrase.Value.IsList)
                {
                    var word = phrase.Value.Words.First;
                    while (word != null)
                    {
                        var match = xmlTagRegex.Match(word.Value);

                        if (match.Success)
                        {
                            // Text directly before this tag goes in current phrase.
                            if (match.Groups["before"].Success)
                            {
                                word.List.AddBefore(word, match.Groups["before"].Value);
                            }
                            if (match.Groups["after"].Success)
                            {
                                word.List.AddAfter(word, match.Groups["after"].Value);
                            }
                            if (match.Groups["fulltag"].Success)
                            {
                                word.Value = match.Groups["fulltag"].Value;
                            }

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
                                {
                                    word.List.Remove(word.Next);
                                }
                            }

                            // Major tags should be the last word.
                            if (word.Next != null && isMajorTag)
                            {
                                Phrases.AddAfter(phrase, new CodeCommentPhrase(
                                    phrase.Value.Indent,
                                    phrase.Value.ListPrefix,
                                    GetWordsAfter(word)));

                                while (word.Next != null)
                                {
                                    word.List.Remove(word.Next);
                                }
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

        #endregion Methods

        /// <summary>
        /// CommentBuilder mimics the functions of <c>EditPoint</c> used to create a comment, but
        /// it works interally rather than editting the document directly.
        /// </summary>
        private class CommentBuilder : IEquatable<string>
        {
            private readonly StringBuilder _builder;
            private readonly int _commentOffset;
            private readonly string _commentPrefix;
            private readonly IndentSettings _indentSettings;

            public CommentBuilder(int commentOffset, string commentPrefix, IndentSettings indentSettings)
            {
                _commentOffset = commentOffset;
                _commentPrefix = commentPrefix;
                _indentSettings = indentSettings ?? new IndentSettings();

                _builder = new StringBuilder();

                LineCharOffset = _commentOffset;
            }

            public int LineCharOffset { get; private set; }

            public bool Equals(string other)
            {
                return String.Equals(_builder.ToString(), other);
            }

            public void Insert(string text)
            {
                _builder.Append(text);
                LineCharOffset += text.Length;
            }

            private void PadToColumn(int column)
            {
                // If using tabs, insert as many tabs as possible without exceeding padding width.
                if (_indentSettings.InsertTabs)
                {
                    int tabCount = (column - LineCharOffset) / _indentSettings.TabSize;
                    Insert("".PadLeft(tabCount, '\t'));
                    // Fixup character offset because tab is one character but takes up more room.
                    LineCharOffset += tabCount * (_indentSettings.TabSize - 1);
                }

                // Fill remaining space with spaces.
                Insert("".PadLeft(column - LineCharOffset, ' '));
            }

            public override string ToString()
            {
                return _builder.ToString();
            }

            public void WriteNewCommentLine(bool resumeComment = false)
            {
                _builder.AppendLine();
                LineCharOffset = 0;

                PadToColumn(_commentOffset);

                if (resumeComment)
                {
                    Insert(_commentPrefix);
                }
            }
        }
    }
}