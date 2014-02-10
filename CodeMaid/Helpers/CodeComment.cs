#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

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
        private readonly TextDocument document;
        private readonly CodeMaidPackage package;
        private Regex codeLineRegex;
        private Regex commentLineRegex;

        private EditPoint endPoint;
        private EditPoint startPoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeComment" /> class.
        /// </summary>
        public CodeComment(TextPoint point, CodeMaidPackage package)
        {
            if (point == null)
                throw new ArgumentNullException("point");

            this.document = point.Parent;
            this.package = package;

            this.commentLineRegex = CodeCommentHelper.GetCommentRegex(this.document.Language, true);
            this.codeLineRegex = CodeCommentHelper.GetCodeCommentRegex(this.document.Language);

            this.Expand(point);
        }

        public TextPoint EndPoint { get { return endPoint; } }

        public bool IsValid { get; private set; }

        public TextPoint StartPoint { get { return startPoint; } }

        /// <summary>
        /// Formats the comment.
        /// </summary>
        public TextPoint Format(CodeCommentOptions options)
        {
            if (!IsValid)
                throw new InvalidOperationException("Cannot format comment, the comment is not valid.");

            var originalText = startPoint.GetText(endPoint);
            var matches = commentLineRegex.Matches(originalText).OfType<Match>().ToArray();
            var commentPrefix = matches.First(m => m.Success).Groups["prefix"].Value;

            LinkedList<ICodeCommentPhrase> phrases = null;

            // Concatenate the comment lines without comment prefixes and see if the resulting bit
            // can be parsed as XML.
            var commentText = string.Join("", matches.Select(m => m.Groups["line"].Value));
            if (commentText.Contains('<'))
            {
                try
                {
                    var commentXml = XElement.Parse(string.Format("<doc>{0}</doc>", commentText));

                    // We need a different regex for parsing the XML values, because it no longer
                    // contains the comment prefix value.
                    phrases = XmlToPhrases(
                        CodeCommentHelper.GetCommentRegex(this.document.Language, false),
                        commentXml);
                }
                catch (System.Xml.XmlException)
                {
                    // If XML cannot be parsed, comment will be handled as a normal text comment.
                }
            }

            if (phrases == null)
            {
                phrases = MatchesToPhrases(matches);
            }

            var formatter = new CommentFormatter(commentPrefix, options);

            formatter.AppendPhrases(phrases);

            if (!formatter.Equals(originalText))
            {
                var cursor = StartPoint.CreateEditPoint();
                cursor.Delete(EndPoint);
                cursor.Insert(formatter.ToString());
                endPoint = cursor.CreateEditPoint();
            }

            return EndPoint;
        }

        /// <summary>
        /// Expands a text point to the full comment.
        /// </summary>
        /// <param name="point">The original text point to expand from.</param>
        private void Expand(TextPoint point)
        {
            var i = point.CreateEditPoint();

            startPoint = Expand(point, (p) => p.LineUp());
            endPoint = Expand(point, (p) => p.LineDown());

            if (StartPoint != null && EndPoint != null)
            {
                System.Diagnostics.Debug.WriteLine("Found comment from line {0} to {1}.", StartPoint.Line, EndPoint.Line);
                startPoint.StartOfLine();
                endPoint.EndOfLine();
                IsValid = true;
            }
            else
            {
                IsValid = false;
            }
        }

        private EditPoint Expand(TextPoint point, Action<EditPoint> foundAction)
        {
            EditPoint i = point.CreateEditPoint();
            EditPoint result = null;

            do
            {
                var line = i.Line;
                var text = i.GetLine();
                if (CodeCommentHelper.LineMatchesRegex(i, this.commentLineRegex).Success)
                {
                    result = i.CreateEditPoint();
                    foundAction(i);

                    // If result and iterator line are the same, the found action (move line up or
                    // down) did nothing. This means there is no point to keep searching, it would
                    // create an infinite loop.
                    if (result.Line == i.Line)
                        break;
                }
                else
                {
                    if (i != null && result != null && CodeCommentHelper.LineMatchesRegex(i, this.codeLineRegex).Success)
                    {
                        result = null;
                    }

                    i = null;
                }
            } while (i != null);

            return result;
        }

        private static LinkedList<ICodeCommentPhrase> MatchesToPhrases(IList<Match> matches)
        {
            Match m;
            while ((m = matches.FirstOrDefault()) != null && string.IsNullOrWhiteSpace(m.Value))
                matches.Remove(m);
            while ((m = matches.LastOrDefault()) != null && string.IsNullOrWhiteSpace(m.Value))
                matches.Remove(m);

            var phrases = new LinkedList<ICodeCommentPhrase>();
            foreach (var line in matches)
            {
                if (!line.Success)
                {
                    System.Diagnostics.Debug.Fail("Cannot parse a line that does not match the comment regex.");
                }
                else
                {
                    var indent = line.Groups["indent"].Success ? line.Groups["indent"].Value.Length - 1 : 0;
                    var prefix = line.Groups["listprefix"].Success ? line.Groups["listprefix"].Value : null;
                    var words = line.Groups["words"].Success ? line.Groups["words"].Captures.OfType<Capture>().Select(c => c.Value) : null;

                    var newPhrase = new CodeCommentPhrase(words, prefix);
                    var lastPhrase = (phrases.Last ?? phrases.AddLast(new CodeCommentPhrase()));
                    var lastValue = lastPhrase.Value as CodeCommentPhrase;

                    if (lastValue != null && lastValue.CanAppend(newPhrase, indent > 0))
                        lastValue.Append(newPhrase);
                    else
                        phrases.AddLast(newPhrase);
                }
            }
            return new LinkedList<ICodeCommentPhrase>(phrases);
        }

        /// <summary>
        /// Helper function to generate the preview in the options menu.
        /// </summary>
        public static string FormatXml(string text, CodeCommentOptions options)
        {
            var xml = XElement.Parse(string.Format("<doc>{0}</doc>", text));
            var regex = CodeCommentHelper.GetCommentRegex("CSharp", false);
            var phrases = XmlToPhrases(regex, xml);

            var formatter = new CommentFormatter("///", options);
            formatter.AppendPhrases(phrases);

            return formatter.ToString();
        }

        private static LinkedList<ICodeCommentPhrase> XmlToPhrases(Regex regex, XElement xml)
        {
            var result = new LinkedList<ICodeCommentPhrase>();
            foreach (var element in xml.Elements())
            {
                var phrase = new CodeCommentPhraseXml(
                    CodeCommentHelper.CreateXmlOpenTag(element),
                    CodeCommentHelper.CreateXmlCloseTag(element));

                string innerText;
                using (var reader = element.CreateReader())
                {
                    reader.MoveToContent();
                    innerText = reader.ReadInnerXml();
                }
                var matches = regex.Matches(innerText).OfType<Match>().ToList();

                phrase.Add(MatchesToPhrases(matches));

                result.AddLast(phrase);
            }

            return result;
        }

        private class CommentFormatter : IEquatable<string>
        {
            private const string spacer = " ";

            private readonly StringBuilder builder;
            private readonly string commentPrefix;
            private readonly int commentPrefixLength;
            private int currentPosition;
            private CodeCommentOptions options;

            public CommentFormatter(string commentPrefix, CodeCommentOptions options)
            {
                this.builder = new StringBuilder();
                this.currentPosition = 0;

                this.options = options;

                this.commentPrefix = commentPrefix;
                this.commentPrefixLength = WordLength(commentPrefix);
            }

            public void AppendPhrases(IEnumerable<ICodeCommentPhrase> phrases, int extraIndent = 0)
            {
                // If XML parameter tags should be aligned, find the longest one and then pad
                // all to an equal size.
                if (options.XmlAlignParamTags)
                {
                    var paramPhrases = phrases.OfType<CodeCommentPhraseXml>().Where(p => p.OpenTag.StartsWith("<param "));
                    if (paramPhrases.Any())
                    {
                        var longestParam = paramPhrases.Max(p => p.OpenTag.Length);
                        foreach (var phrase in paramPhrases)
                        {
                            phrase.OpenTag = phrase.OpenTag.PadRight(longestParam);
                        }
                    }
                }

                foreach (var phrase in phrases)
                {
                    AppendLine();

                    var xmlPhrase = phrase as CodeCommentPhraseXml;
                    if (xmlPhrase != null)
                    {
                        AppendWord(spacer);
                        AppendWord(xmlPhrase.OpenTag);

                        var phraseCount = xmlPhrase.Phrases.Count;
                        bool singleLine = phraseCount == 0;
                        if (phraseCount == 1)
                        {
                            // If there is only one phrase inside the XML phrase, see if it fits on
                            // a single line.
                            var firstPhrase = xmlPhrase.Phrases.First.Value as CodeCommentPhrase;
                            if (firstPhrase != null)
                            {
                                var length = xmlPhrase.OpenTag.Length +
                                    xmlPhrase.CloseTag.Length +
                                    firstPhrase.Length +
                                    (options.XmlSpaceTags ? 2 : 0) +
                                    commentPrefixLength;

                                if (options.SkipWrapOnLastWord)
                                {
                                    var lastWord = firstPhrase.Words.Last;
                                    length -= lastWord != null ? WordLength(lastWord.Value) + 1 : 0;
                                }

                                if (length <= options.WrapAtColumn)
                                {
                                    // Single XML line has no extra indenting.
                                    AppendPhrase(firstPhrase, options.XmlSpaceTags, 0);
                                    singleLine = true;
                                }
                            }
                        }

                        if (!singleLine)
                        {
                            AppendPhrases(xmlPhrase.Phrases, options.XmlValueIndent);
                            AppendLine();
                            AppendWord(spacer);
                        }
                        else if (options.XmlSpaceTags)
                        {
                            AppendWord(spacer);
                        }

                        AppendWord(xmlPhrase.CloseTag);
                    }
                    else
                    {
                        AppendPhrase(phrase as CodeCommentPhrase, true, extraIndent);
                    }
                }
            }

            public bool Equals(string other)
            {
                return string.Equals(ToString(), other);
            }

            public override string ToString()
            {
                return builder.ToString();
            }

            private void AppendLine()
            {
                if (currentPosition > 0)
                {
                    builder.AppendLine();
                    currentPosition = 0;
                }

                builder.Append(commentPrefix);
                currentPosition += commentPrefixLength;
            }

            private void AppendPhrase(CodeCommentPhrase phrase, bool initialSpacer, int extraIndent)
            {
                if (phrase != null)
                {
                    AppendWord(string.Empty.PadLeft(extraIndent));

                    if (phrase.IsList)
                    {
                        AppendWord(spacer);
                        AppendWord(phrase.ListPrefix);
                    }

                    var word = phrase.Words.First;

                    var firstWord = true;
                    while (word != null)
                    {
                        var wordLength = WordLength(word.Value);

                        var wrap = false;

                        // If current position plus word length exceeds the maximum comment length,
                        // wrap to the next line. Take care not to wrap on the first word, otherwise
                        // a word that never fits a line (ie. too long) would cause endless linewrapping.
                        if (!firstWord && currentPosition + wordLength + 1 > options.WrapAtColumn)
                            wrap = true;

                        // If this is the last word and user selected to not wrap on the last word,
                        // don't wrap.
                        if (wrap && word.Next == null && options.SkipWrapOnLastWord)
                            wrap = false;

                        if (wrap)
                        {
                            AppendLine();

                            AppendWord(string.Empty.PadLeft(extraIndent));

                            if (phrase.IsList)
                                AppendWord(string.Empty.PadLeft(WordLength(phrase.ListPrefix) + 1, spacer[0]));

                            firstWord = true;
                        }

                        if (initialSpacer)
                            AppendWord(spacer);
                        else
                            initialSpacer = true;

                        AppendWord(word.Value);

                        firstWord = false;
                        word = word.Next;
                    }
                }
            }

            private void AppendWord(string word)
            {
                builder.Append(word);
                currentPosition += WordLength(word);
            }

            private int WordLength(string word)
            {
                return word.Length + word.Count(c => c == '\t') * (options.TabSize - 1);
            }
        }
    }
}