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
using System.Xml.Linq;
using EnvDTE;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A <c>CodeComment</c> contains one or more <see cref="CodeCommentPhrase">phrases</see> which
    /// represent all the content of a comment.
    /// </summary>
    internal class CodeComment
    {
        #region Fields

        private static readonly Regex CommentLineRegex = new Regex(@"(?<indent>[\t ]*(?<listprefix>[-=\*\+]+|\w+[\):]|\d+\.)?[\t ]*)(?<words>.*?)(\r?\n|$)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex WordSplitRegex = new Regex(@"[\t ]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private readonly IndentSettings _indentSettings;
        private readonly CachedSettingSet<string> _majorTags;
        private readonly CachedSettingSet<string> _minorTags;
        private readonly string _commentRegex;

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
            Phrases = new LinkedList<CodeCommentPhrase>();

            StartPoint = from.CreateEditPoint();
            EndPoint = to.CreateEditPoint();
            LineCharOffset = from.LineCharOffset;

            _commentRegex = commentRegex;
            _majorTags = majorTags;
            _minorTags = minorTags;
            _indentSettings = indentSettings;
        }

        private void ParseTextComment(IEnumerable<string> lines)
        {
            foreach (var line in lines)
                ParseTextLine(line);
        }

        private void ParseTextLine(string line)
        {
            var lineMatch = CommentLineRegex.Match(line.Trim());

            var indent = lineMatch.Groups["indent"].Success ? lineMatch.Groups["indent"].Length : 0;
            var listPrefix = lineMatch.Groups["listprefix"].Success ? lineMatch.Groups["listprefix"].Value : null;
            var words = WordSplitRegex.Split(lineMatch.Groups["words"].Value);

            if (words[0].Length == 0 && listPrefix == null)
            {
                Phrases.AddLast(new CodeCommentPhrase());
                Phrases.AddLast(new CodeCommentPhrase());
            }
            else if (
                Phrases.Last == null // No phrases yet
                || !String.IsNullOrEmpty(listPrefix) // Lists always starts on own line
                || (Phrases.Last.Value.IsList && indent <= 1) // Previous line is list but this line is not indented
                )
            {
                Phrases.AddLast(new CodeCommentPhrase(indent, listPrefix, words));
            }
            else
            {
                Phrases.Last.Value.AppendWords(words);
            }
        }

        private bool ParseXmlComment(IEnumerable<string> lines, int maxWidth)
        {
            XElement xml;
            try
            {
                // Need to put a root element around the comment to make it valid XML before
                // parsing.
                xml = XElement.Parse(string.Format("<doc>{0}</doc>", string.Join(Environment.NewLine, lines)));
            }
            catch (System.Xml.XmlException)
            {
                // If there is a parsing error within the XML, return false so the calling function
                // knows it cannot be treated as XML comment.
                return false;
            }

            foreach (var element in xml.Elements())
            {
                // Root level elements always start on their own line.
                var phrase = StartNewPhrase();
                phrase.IsXml = true;

                var openTag = CodeCommentHelper.CreateXmlOpenTag(element);
                phrase.AppendWords(openTag);

                if (!element.IsEmpty)
                {
                    // Major tags are alone on their line.
                    var onNewLine = _majorTags.Value.Contains(element.Name.LocalName, StringComparer.OrdinalIgnoreCase);

                    var closeTag = CodeCommentHelper.CreateXmlCloseTag(element);

                    // Get the inner text of this node, including tags.
                    string innerText;
                    using (var reader = element.CreateReader())
                    {
                        reader.MoveToContent();
                        innerText = reader.ReadInnerXml();
                    }
                   
                    // With the fancy XML parsing, we can also check if the content will fit on a
                    // single line, and if not, split it like it is a major tag.
                    if (this.LineCharOffset + openTag.Length + closeTag.Length + innerText.Length > maxWidth)
                        onNewLine = true;

                    if (onNewLine)
                        phrase = StartNewPhrase();

                    ParseTextComment(innerText.Split(new[] { Environment.NewLine }, StringSplitOptions.None));

                    if (onNewLine)
                        phrase = StartNewPhrase();

                    phrase.AppendWords(closeTag);
                }
            }

            return true;
        }

        private CodeCommentPhrase StartNewPhrase()
        {
            return Phrases.AddLast(new CodeCommentPhrase()).Value;
        }

        #endregion Constructors

        #region Properties


        public EditPoint EndPoint { get; private set; }

        public bool IsBlockComment { get; private set; }

        public bool IsXmlComment { get; private set; }

        public int LineCharOffset { get; private set; }

        /// <summary>
        /// Gets the collection of comment phrases for this comment. Each phrase starts on it's own
        /// line, and may span one or more lines.
        /// </summary>
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

        /// <summary>
        /// Gets the text and builds a comment out of it. This function does not change the actual
        /// document.
        /// </summary>
        /// <param name="maxWidth">Max width of the comment.</param>
        /// <returns>A <see cref="CommentBuilder"/> instance with the formatted content of this
        /// comment.</returns>
        private CommentBuilder BuildComment(int maxWidth)
        {
            if (maxWidth < LineCharOffset + 20)
            {
                maxWidth = LineCharOffset + 20;
            }

            // Get the complete comment text (including the comment prefixes) and parse it into phrases.
            string text = StartPoint.GetText(EndPoint);

            var commentMatch = Regex.Match(text, _commentRegex, RegexOptions.IgnoreCase);
            var originalLines = commentMatch.Groups["line"].Captures.OfType<Capture>().Select(c => c.Value);
            var commentPrefix = commentMatch.Groups["prefix"].Value;

            if (commentPrefix.Length != 3 || !ParseXmlComment(originalLines, maxWidth))
            {
                ParseTextComment(originalLines);
            }

            // Output buffer. The magic minus 1 accounts for the difference between cursor position
            // and actual character count.
            var builder = new CommentBuilder(LineCharOffset - 1, commentPrefix, _indentSettings);

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
                    builder.Insert(commentPrefix);
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
                    // keep in mind some words can by themself already be too long to fit on a line.
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

                    // Output spacing (with exception for the first and last word on an XML phrase),
                    // and the actual word.
                    if (!(phrase.Value.IsXml && word.Previous != null && (phrase.Value.Words.First == word.Previous || word.Next == null)))
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

        #endregion Methods

        /// <summary>
        /// CommentBuilder mimics the functions of <c>EditPoint</c> used to create a comment, but it
        /// works interally rather than editting the document directly.
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
        }
    }
}