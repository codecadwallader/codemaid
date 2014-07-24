using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SteveCadwallader.CodeMaid.Helpers;

namespace SteveCadwallader.CodeMaid.Model.Comments
{
    internal class CommentFormatter : IEquatable<string>
    {
        private readonly StringBuilder builder;
        private readonly string commentPrefix;
        private readonly int commentPrefixLength;
        private int currentPosition;
        private bool isFirstWord;
        private CodeCommentOptions options;
        private Regex regex;

        public CommentFormatter(ICommentLine line, string commentPrefix, CodeCommentOptions options, Regex regex)
        {
            this.builder = new StringBuilder();
            this.currentPosition = 0;
            this.options = options;
            this.regex = regex;
            this.isFirstWord = true;

            this.commentPrefix = commentPrefix + CodeCommentHelper.Spacer;
            this.commentPrefixLength = WordLength(commentPrefix);

            // Special handling for the root XML line, it should not output it's surrounding xml
            // tags, only the content.
            var xml = line as CommentLineXml;
            if (xml != null)
            {
                // On the content of the root, fix the optional alignment of param tags. This is
                // not important if all tags will be broken onto seperate lines anyway.
                if (!options.XmlBreakAllTags && options.XmlAlignParamTags)
                {
                    var paramPhrases = xml.Lines.OfType<CommentLineXml>().Where(p => string.Equals(p.TagName, "param", StringComparison.OrdinalIgnoreCase));
                    if (paramPhrases.Count() > 1)
                    {
                        var longestParam = paramPhrases.Max(p => p.OpenTag.Length);
                        foreach (var phrase in paramPhrases)
                        {
                            phrase.OpenTag = phrase.OpenTag.PadRight(longestParam);
                        }
                    }
                }

                foreach (var l in xml.Lines)
                {
                    this.NewLine();
                    this.Parse(l);
                }
            }
            else
            {
                this.NewLine();
                this.Parse(line);
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

        private void Append(char value)
        {
            Append(value.ToString());
        }

        private void Append(string value)
        {
            builder.Append(this.options.XmlKeepTagsTogether ? CodeCommentHelper.FakeToSpace(value) : value);
            currentPosition += WordLength(value);
            isFirstWord = false;
        }

        private void NewLine(int indentLevel = 0)
        {
            if (currentPosition > 0)
            {
                builder.AppendLine();
                currentPosition = 0;
            }

            builder.Append(commentPrefix);
            currentPosition += commentPrefixLength;

            if (indentLevel > 0 && options.XmlValueIndent > 0)
            {
                Append(string.Empty.PadLeft(indentLevel * options.XmlValueIndent));
            }

            isFirstWord = true;
        }

        /// <summary>
        /// Parse a code comment line into a string and write it to the buffer.
        /// </summary>
        /// <param name="line">The comment line.</param>
        /// <param name="indentLevel">The level of indenting for the content of this tag.</param>
        /// <param name="xmlTagLength">
        /// The length of the enclosing XML tags, this is needed to calculate the line length for
        /// single line XML comments.
        /// </param>
        /// <returns>True if line fitted on single line, false if wrapped.</returns>
        private bool Parse(ICommentLine line, int indentLevel = 0, int xmlTagLength = 0)
        {
            var xml = line as CommentLineXml;
            if (xml != null)
            {
                this.ParseXml(xml, indentLevel);
            }
            else
            {
                var matches = this.regex.Matches(line.Content).OfType<Match>().Select(x => new CodeCommentMatch(x)).ToList();

                // Remove empty matches from the start and end of the comment.
                CodeCommentMatch m;
                while ((m = matches.FirstOrDefault()) != null && m.IsEmpty)
                    matches.Remove(m);
                while ((m = matches.LastOrDefault()) != null && m.IsEmpty)
                    matches.Remove(m);

                // Join the comment matches into single lines where possible.
                if (matches.Count > 1)
                {
                    int i = 0;
                    do
                    {
                        m = matches[i];
                        if (m.TryAppend(matches[i + 1]))
                        {
                            matches.RemoveAt(i + 1);
                        }
                        else
                        {
                            i++;
                        }
                    } while (i < matches.Count - 1);
                }

                // Extended logic for line breaks.
                // - Break if there is more than 1 line match (eg. due to a list or child xml tags).
                // - Break if the content does not fit on a single line.
                var matchCount = matches.Count;
                var forceBreak = matchCount > 1;
                if (!forceBreak && matchCount == 1 && matches[0].Words.Any())
                {
                    // Calculate the length of the first line.
                    var firstLineLength = this.commentPrefixLength + xmlTagLength + matches[0].Length + (indentLevel * options.XmlValueIndent);

                    // Tag spacing adds a space before and after.
                    if (options.XmlSpaceTagContent)
                        firstLineLength += 2;

                    // If set to skip wrapping on the last word, it's length does not matter.
                    if (options.SkipWrapOnLastWord)
                        firstLineLength -= WordLength(matches[0].Words.Last()) + 1;

                    forceBreak = firstLineLength > options.WrapAtColumn;
                }

                if (currentPosition == 0 || !this.isFirstWord && forceBreak)
                {
                    NewLine(indentLevel);
                }

                foreach (var match in matches)
                {
                    var isList = false;
                    if (!string.IsNullOrWhiteSpace(match.ListPrefix))
                    {
                        isList = true;
                        NewLine(indentLevel);
                        Append(match.ListPrefix);
                        Append(CodeCommentHelper.Spacer);
                    }

                    if (match.Words != null)
                    {
                        var wordCount = match.Words.Count - 1;
                        var firstWord = true;

                        for (int i = 0; i <= wordCount; i++)
                        {
                            var word = match.Words[i];
                            var length = WordLength(word);
                            var wrap = false;

                            // If current position plus word length exceeds the maximum
                            // comment length, wrap to the next line. Take care not to wrap
                            // on the first word, otherwise a word that never fits a line
                            // (ie. too long) would cause endless linewrapping.
                            if (!firstWord && currentPosition + length + 1 > options.WrapAtColumn)
                                wrap = true;

                            // If this is the last word and user selected to not wrap on the
                            // last word, don't wrap.
                            if (wrap && i == wordCount && options.SkipWrapOnLastWord)
                                wrap = false;

                            if (wrap)
                            {
                                NewLine(indentLevel);
                                firstWord = true;

                                if (isList)
                                    Append(string.Empty.PadLeft(WordLength(match.ListPrefix) + 1, CodeCommentHelper.Spacer));
                            }

                            if (!firstWord)
                                Append(CodeCommentHelper.Spacer);

                            Append(word);

                            firstWord = false;
                        }
                    }
                }

                if (currentPosition == 0 || currentPosition > this.commentPrefixLength && forceBreak)
                {
                    return true;
                }
            }
            return false;
        }

        private bool ParseXml(CommentLineXml line, int indentLevel = 0)
        {
            // All XML lines start on a new line.
            if (!this.isFirstWord)
            {
                this.NewLine(indentLevel);
            }

            this.Append(line.OpenTag);

            indentLevel++;

            // Break after the tag if required:
            // - Break on all tags is set to true.
            // - Tag consists of multiple lines.
            // - Tag has literal content (eg. a <code> element).
            bool tagOnOwnLine = this.options.XmlBreakAllTags || line.Lines.Count > 1 || line.Content != null;
            if (tagOnOwnLine)
            {
                this.NewLine(indentLevel);
            }

            // If the literal content of an XML tag is set, output it without formatting.
            if (line.Content != null)
            {
                var codeLines = Regex.Split(line.Content.Trim('\r', '\n'), "\n");
                for (int i = 0; i < codeLines.Length; i++)
                {
                    Append(codeLines[i]);
                    if (i + 1 < codeLines.Length)
                        NewLine(indentLevel);
                }
            }
            else
            {
                // Loop and parse all content lines, with a little hacky solution for allowing
                // the parser to know the XML tag length.
                var xmlTagLength = tagOnOwnLine ? 0 : WordLength(line.OpenTag) + WordLength(line.Closetag);
                foreach (var l in line.Lines)
                {
                    // Parse function returns true if it had to wrap lines. If so, we need to
                    // force a newline before the closing tag.
                    tagOnOwnLine |= this.Parse(l, indentLevel, xmlTagLength);
                }
            }

            indentLevel--;

            // If opening tag was on own line, do the same for the closing tag.
            if (tagOnOwnLine && !this.isFirstWord)
            {
                this.NewLine(indentLevel);
            }

            this.Append(line.Closetag);

            return true;
        }

        /// <summary>
        /// Return the length of a string, taking into account the tab characters.
        /// </summary>
        /// <param name="word">The string to return the length of.</param>
        /// <returns>The length of the string.</returns>
        private int WordLength(string word)
        {
            return word.Length + word.Count(c => c == '\t') * (options.TabSize - 1);
        }

        /// <summary>
        /// Return the length of a character, taking into account the tab characters.
        /// </summary>
        /// <param name="word">The character to return the length of.</param>
        /// <returns>The length of the character.</returns>
        private int WordLength(char word)
        {
            return word == '\t' ? options.TabSize : 1;
        }
    }
}