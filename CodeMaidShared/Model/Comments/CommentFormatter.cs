using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Model.Comments.Options;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SteveCadwallader.CodeMaid.Model.Comments
{
    /// <summary>
    /// Class to handle the actual wrapping and formatting of a comment.
    /// </summary>
    internal class CommentFormatter : IEquatable<string>
    {
        private readonly StringBuilder _builder;
        private readonly CommentOptions _commentOptions;
        private readonly FormatterOptions _formatterOptions;
        private int _commentPrefixLength;
        private int _currentPosition;
        private int _indentAmount;
        private bool _isFirstWord;
        private bool _isIndented;
        private bool _isPrefixWritten;

        public CommentFormatter(ICommentLine line, FormatterOptions formatterOptions, CommentOptions commentOptions)
        {
            _formatterOptions = formatterOptions;
            _commentOptions = commentOptions;

            _builder = new StringBuilder();
            _currentPosition = 0;
            _isFirstWord = true;
            _isPrefixWritten = false;
            _isIndented = false;
            _indentAmount = 0;
            _commentPrefixLength = WordLength(commentOptions.Prefix);

            // Special handling for the root XML line, it should not output it's surrounding xml
            // tags, only it's child lines.
            if (line is CommentLineXml xml)
            {
                // On the content of the root, fix the optional alignment of param tags.
                if (_formatterOptions.Xml.AlignParamTags)
                {
                    AlignParamTags(xml);
                }

                // Process all the lines inside the root XML line.
                foreach (var l in xml.Lines)
                {
                    NewLine();
                    Format(l);
                }
            }
            else
            {
                // Normal comment line has no child-lines and can be processed normally.
                Format(line);
            }
        }

        public bool Equals(string other)
        {
            return string.Equals(ToString(), other);
        }

        public override string ToString()
        {
            return _builder.ToString().TrimEnd();
        }

        private static void AlignParamTags(CommentLineXml xml)
        {
            var paramPhrases = xml.Lines.OfType<CommentLineXml>().Where(p => string.Equals(p.TagName, "param", StringComparison.OrdinalIgnoreCase));
            if (paramPhrases.Count() > 1)
            {
                // If param tags are broken into seperate lines there is nothing to align.
                var paramSplit = paramPhrases.First().TagOptions.Split.HasFlag(XmlTagNewLine.AfterOpen);
                if (!paramSplit)
                {
                    var longestParam = paramPhrases.Max(p => p.OpenTag.Length);
                    foreach (var phrase in paramPhrases)
                    {
                        phrase.OpenTag = phrase.OpenTag.PadRight(longestParam);
                    }
                }
            }
        }

        private void Append(char value)
        {
            Append(value.ToString());
        }

        /// <summary>
        /// Append value to current line. When line is still empty, this first writes the comment
        /// prefix, indenting and initial spacer before appending the given value. Empty values are
        /// ignored, but the comment prefix will be added if required.
        /// </summary>
        /// <param name="value">The string to append to the writer.</param>
        /// <param name="noIdenting">
        /// <c>true</c> if value should not be indented, eg for literal content.
        /// </param>
        private void Append(string value, bool noIdenting = false)
        {
            if (!_isPrefixWritten)
            {
                _builder.Append(_commentOptions.Prefix);
                _currentPosition += _commentPrefixLength;
                _isPrefixWritten = true;
                _isIndented = false;
            }

            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            if (!_isIndented)
            {
                if (!noIdenting)
                {
                    // Empty prefix also means no initial spacing.
                    if (_commentPrefixLength > 0)
                    {
                        _builder.Append(CodeCommentHelper.Spacer);
                        _currentPosition += 1;
                    }

                    if (_indentAmount > 0)
                    {
                        _builder.Append(string.Empty.PadLeft(_indentAmount));
                        _currentPosition += _indentAmount;
                    }

                    _isIndented = true;
                }
            }

            _builder.Append(value);
            _currentPosition += WordLength(value);
            _isFirstWord = false;
        }

        /// <summary>
        /// Parse a comment line into individual words and write it to the buffer.
        /// </summary>
        /// <param name="line">The comment line.</param>
        /// <param name="xmlTagLength">
        /// The length of the enclosing XML tags, this is needed to calculate the line length for
        /// single line XML comments.
        /// </param>
        /// <param name="xmlSpaceParentTagContent">
        /// Set to <c>true</c> when parent is an XML tag and wants space between tags and content.
        /// </param>
        /// <returns>
        /// <c>true</c> if line fitted on single line, <c>false</c> if it wrapped on multiple lines.
        /// </returns>
        private bool Format(ICommentLine line, int xmlTagLength = 0, bool xmlSpaceParentTagContent = false)
        {
            if (line is CommentLineXml xml)
            {
                return FormatXml(xml);
            }

            if (line.Content == null)
                return true;

            var matches = _commentOptions.Regex.Matches(line.Content).OfType<Match>().Select(x => new CodeCommentMatch(x, _formatterOptions)).ToList();

            // Remove empty matches from the start and end of the comment.
            CodeCommentMatch m;
            while (((m = matches.FirstOrDefault()) != null && m.IsEmpty) || ((m = matches.LastOrDefault()) != null && m.IsEmpty))
            {
                matches.Remove(m);
            }

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
            var fittedOnLine = true;

            if (!forceBreak && matchCount == 1 && matches[0].Words.Any())
            {
                // Calculate the length of the first line.
                var firstLineLength = _commentPrefixLength + xmlTagLength + matches[0].Length + _indentAmount;

                // If set to skip wrapping on the last word, the last word's length does not matter.
                if (_formatterOptions.SkipWrapOnLastWord)
                {
                    firstLineLength -= WordLength(matches[0].Words.Last()) + 1;
                }

                forceBreak = firstLineLength > _formatterOptions.WrapColumn;
            }

            if (_currentPosition == 0 || (!_isFirstWord && forceBreak))
            {
                NewLine();
                fittedOnLine = false;
            }
            else if (!_isFirstWord && xmlSpaceParentTagContent)
            {
                // Parent is XML tag and wants space between tags and content.
                Append(CodeCommentHelper.Spacer);
            }

            // Always consider the word after the opening tag as the first word to prevent an extra
            // space before.
            _isFirstWord = true;

            foreach (var match in matches)
            {
                if (match.IsLiteral || match.IsList)
                {
                    if (!_isFirstWord)
                    {
                        NewLine();
                        fittedOnLine = false;
                    }
                }

                if (match.IsList)
                {
                    Append(match.ListPrefix);

                    // List items include their spacing and do not require additional space, thus we
                    // are logically still on the first word.
                    _isFirstWord = true;
                }

                if (!match.IsEmpty)
                {
                    var wordCount = match.Words.Count - 1;

                    for (int i = 0; i <= wordCount; i++)
                    {
                        var word = match.Words[i];
                        var length = WordLength(word);
                        var wrap = false;

                        // If current position plus word length exceeds the maximum comment length,
                        // wrap to the next line. Take care not to wrap on the first word, otherwise
                        // a word that never fits a line (ie. too long) would cause endless linewrapping.
                        if (!_isFirstWord && _currentPosition + length + 1 > _formatterOptions.WrapColumn)
                        {
                            wrap = true;
                        }

                        // If this is the last word and user selected to not wrap on the last word,
                        // don't wrap.
                        if (wrap && i == wordCount && _formatterOptions.SkipWrapOnLastWord)
                        {
                            wrap = false;
                        }

                        if (wrap)
                        {
                            NewLine();
                            fittedOnLine = false;

                            // If linewrap is on a list item, add extra spacing to align the text
                            // with the previous line.
                            if (match.IsList)
                            {
                                Append(string.Empty.PadLeft(WordLength(match.ListPrefix), CodeCommentHelper.Spacer));

                                // Unset the first-word flag, because this is just padding and not a
                                // proper word.
                                _isFirstWord = true;
                            }
                        }
                        else if (!_isFirstWord)
                        {
                            Append(CodeCommentHelper.Spacer);
                        }

                        Append(CodeCommentHelper.FakeToSpace(word));
                    }
                }
                else
                {
                    // Line without words, create a blank line. First end the current line.
                    NewLine();

                    // And then force a newline creating an empty one.
                    NewLine(true);
                    fittedOnLine = false;
                }
            }

            return fittedOnLine;
        }

        /// <returns>
        /// Returns <c>true</c> if the line requests a break afterwards (did not fit on a single
        /// line), otherwise <c>false</c>.
        /// </returns>
        private bool FormatXml(CommentLineXml xml)
        {
            var isLiteralContent = !string.IsNullOrEmpty(xml.Content);
            var split = xml.TagOptions.Split;

            if (isLiteralContent)
            {
                // Tags containing literal content with multiple with should always be on their own line.
                if (xml.Content.Contains('\n'))
                    split = XmlTagNewLine.Always;
            }
            else if ((split == XmlTagNewLine.Default || split == XmlTagNewLine.Content) && xml.Lines.Count > 1)
            {
                // Split always if there is more than one child line.
                split = XmlTagNewLine.Always;
            }

            if (split.HasFlag(XmlTagNewLine.BeforeOpen) && !_isFirstWord)
            {
                NewLine();
            }

            Append(xml.TagOptions.KeepTogether ? CodeCommentHelper.FakeToSpace(xml.OpenTag) : xml.OpenTag);

            // Self closing tags have no content, skip all further logic and just output.
            if (xml.IsSelfClosing)
            {
                if (split.HasFlag(XmlTagNewLine.AfterClose))
                {
                    if (!xml.IsLast)
                    {
                        NewLine();
                    }
                    return false;
                }

                return true;
            }

            if (split.HasFlag(XmlTagNewLine.AfterOpen))
            {
                NewLine();
            }

            // Increase the indenting.
            _indentAmount += xml.TagOptions.Indent;

            if (isLiteralContent)
            {
                // If the literal content of an XML tag is set, output that content without formatting.
                var literals = xml.Content.Trim('\r', '\n').TrimEnd('\r', '\n', '\t', ' ').Split('\n');
                for (int i = 0; i < literals.Length; i++)
                {
                    if (i > 0)
                        NewLine(true);
                    Append(literals[i].TrimEnd(), true);
                }
            }
            else
            {
                // Else output the child lines.
                var xmlTagLength = WordLength(xml.OpenTag) + WordLength(xml.CloseTag) + (xml.TagOptions.SpaceContent ? 2 : 0);

                foreach (var line in xml.Lines)
                {
                    if (!Format(line, xmlTagLength, xml.TagOptions.SpaceContent))
                        split |= XmlTagNewLine.BeforeClose | XmlTagNewLine.AfterClose;
                }
            }

            // Remove the indenting.
            _indentAmount -= xml.TagOptions.Indent;

            // If opening tag was on own line, do the same for the closing tag.
            if (split.HasFlag(XmlTagNewLine.BeforeClose))
            {
                NewLine();
            }
            else if (xml.TagOptions.SpaceContent)
            {
                Append(CodeCommentHelper.Spacer);
            }

            Append(xml.CloseTag);

            if (split.HasFlag(XmlTagNewLine.AfterClose))
            {
                //if (!xml.IsLast)
                {
                    NewLine();
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Appends a new line to the buffer, unless buffer already is on an empty new line.
        /// </summary>
        /// <param name="force">
        /// If <c>true</c>, creates a new line even if the current line is empty.
        /// </param>
        private void NewLine(bool force = false)
        {
            if (_isFirstWord && force)
            {
                Append(string.Empty);
            }

            if (!_isFirstWord || force)
            {
                _builder.AppendLine();
                _currentPosition = 0;

                _isPrefixWritten = false;
                _isFirstWord = true;
            }
        }

        /// <summary>
        /// Return the length of a string, taking into account the tab characters.
        /// </summary>
        /// <param name="word">The string to return the length of.</param>
        /// <returns>The length of the string.</returns>
        private int WordLength(string word)
        {
            return word == null ? 0 : word.Length + word.Count(c => c == '\t') * (_formatterOptions.TabSize - 1);
        }
    }
}