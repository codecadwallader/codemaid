﻿#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
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
        #region Fields

        private StringBuilder _builder;
        private string _commentPrefix;
        private int _commentPrefixLength;
        private int _currentPosition;
        private bool _isFirstWord;
        private Regex _regex;
        private int _tabSize;

        #endregion Fields

        #region Constructors

        public CommentFormatter(ICommentLine line, string commentPrefix, int tabSize, Regex regex)
        {
            _builder = new StringBuilder();
            _currentPosition = 0;
            _regex = regex;
            _tabSize = tabSize;
            _isFirstWord = true;

            // Handle optionally empty prefix.
            if (!string.IsNullOrWhiteSpace(commentPrefix))
            {
                _commentPrefix = commentPrefix + CodeCommentHelper.Spacer;
                _commentPrefixLength = WordLength(_commentPrefix);
            }
            else
            {
                _commentPrefix = string.Empty;
                _commentPrefixLength = 0;
            }

            // Special handling for the root XML line, it should not output it's surrounding xml
            // tags, only it's child lines.
            var xml = line as CommentLineXml;
            if (xml != null)
            {
                // On the content of the root, fix the optional alignment of param tags. This is
                // not important if all tags will be broken onto seperate lines anyway.
                if (!Settings.Default.Formatting_CommentXmlSplitAllTags && Settings.Default.Formatting_CommentXmlAlignParamTags)
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

                // Process all the lines inside the root XML line.
                foreach (var l in xml.Lines)
                {
                    NewLine();
                    Parse(l);
                }
            }
            else
            {
                // Normal comment line has no child-lines and can be processed normally.
                NewLine();
                Parse(line);
            }
        }

        #endregion Constructors

        #region Methods

        public bool Equals(string other)
        {
            return string.Equals(ToString(), other);
        }

        public override string ToString()
        {
            return _builder.ToString();
        }

        private void Append(char value)
        {
            Append(value.ToString());
        }

        private void Append(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            _builder.Append(Settings.Default.Formatting_CommentXmlKeepTagsTogether ? CodeCommentHelper.FakeToSpace(value) : value);
            _currentPosition += WordLength(value);
            _isFirstWord = false;
        }

        private void NewLine(int indentLevel = 0, bool force = false)
        {
            if (_currentPosition > 0 || force)
            {
                _builder.AppendLine();
                _currentPosition = 0;
            }

            _builder.Append(_commentPrefix);
            _currentPosition += _commentPrefixLength;

            if (indentLevel > 0 && Settings.Default.Formatting_CommentXmlValueIndent > 0)
            {
                Append(string.Empty.PadLeft(indentLevel * Settings.Default.Formatting_CommentXmlValueIndent));
            }

            _isFirstWord = true;
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
        /// <returns><c>true</c> if line fitted on single line, <c>false</c> if it wrapped on multiple lines.</returns>
        private bool Parse(ICommentLine line, int indentLevel = 0, int xmlTagLength = 0)
        {
            var xml = line as CommentLineXml;
            if (xml != null)
            {
                ParseXml(xml, indentLevel);

                // XML lines always introduce a line wrap.
                return true;
            }
            else
            {
                var matches = _regex.Matches(line.Content).OfType<Match>().Select(x => new CodeCommentMatch(x)).ToList();

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
                if (!forceBreak && matchCount == 1 && matches[0].Words.Any())
                {
                    // Calculate the length of the first line.
                    var firstLineLength = _commentPrefixLength + xmlTagLength + matches[0].Length + (indentLevel * Settings.Default.Formatting_CommentXmlValueIndent);

                    // Tag spacing adds a space before and after.
                    if (Settings.Default.Formatting_CommentXmlSpaceTags)
                    {
                        firstLineLength += 2;
                    }

                    // If set to skip wrapping on the last word, the last word's length does not matter.
                    if (Settings.Default.Formatting_CommentSkipWrapOnLastWord)
                    {
                        firstLineLength -= WordLength(matches[0].Words.Last()) + 1;
                    }

                    forceBreak = firstLineLength > Settings.Default.Formatting_CommentWrapColumn;
                }

                if (_currentPosition == 0 || !_isFirstWord && forceBreak)
                {
                    NewLine(indentLevel);
                }
                else if (!_isFirstWord && Settings.Default.Formatting_CommentXmlSpaceTags)
                {
                    Append(CodeCommentHelper.Spacer);
                }

                // Always consider the word after the opening tag as the first word to prevent an
                // extra space before.
                _isFirstWord = true;

                foreach (var match in matches)
                {
                    if (match.IsList)
                    {
                        if (!_isFirstWord)
                        {
                            NewLine(indentLevel);
                        }

                        Append(match.ListPrefix);
                        // List items include their spacing and do not require additional space,
                        // thus we are logically still on the first word.
                        _isFirstWord = true;
                    }

                    if (match.Words != null)
                    {
                        var wordCount = match.Words.Count - 1;

                        for (int i = 0; i <= wordCount; i++)
                        {
                            var word = match.Words[i];
                            var length = WordLength(word);
                            var wrap = false;

                            // If current position plus word length exceeds the maximum
                            // comment length, wrap to the next line. Take care not to wrap
                            // on the first word, otherwise a word that never fits a line
                            // (ie. too long) would cause endless linewrapping.
                            if (!_isFirstWord && _currentPosition + length + 1 > Settings.Default.Formatting_CommentWrapColumn)
                            {
                                wrap = true;
                            }

                            // If this is the last word and user selected to not wrap on the
                            // last word, don't wrap.
                            if (wrap && i == wordCount && Settings.Default.Formatting_CommentSkipWrapOnLastWord)
                            {
                                wrap = false;
                            }

                            if (wrap)
                            {
                                NewLine(indentLevel);

                                // If linewrap is on a list item, add spacing to align the text.
                                if (match.IsList)
                                {
                                    Append(string.Empty.PadLeft(WordLength(match.ListPrefix), CodeCommentHelper.Spacer));
                                    // This is just padding and not a word.
                                    _isFirstWord = true;
                                }
                            }

                            if (!_isFirstWord)
                            {
                                Append(CodeCommentHelper.Spacer);
                            }

                            Append(word);
                        }
                    }
                    else
                    {
                        // Line without words, create a blank line.
                        if (!_isFirstWord)
                        {
                            NewLine(0);
                        }

                        NewLine(indentLevel, true);
                    }
                }

                if (!forceBreak && Settings.Default.Formatting_CommentXmlSpaceTags)
                {
                    Append(CodeCommentHelper.Spacer);
                }

                if (_currentPosition == 0 || _currentPosition > _commentPrefixLength && forceBreak)
                {
                    // This comment fitted on a single line.
                    return true;
                }
            }

            // This comment did not fit on a single line.
            return false;
        }

        private void ParseXml(CommentLineXml line, int indentLevel = 0)
        {
            // All XML lines start on a new line.
            if (!_isFirstWord)
            {
                NewLine(indentLevel);
            }

            Append(line.OpenTag);

            // If this is the StyleCop SA1633 header <copyright> tag, the content should ALWAYS be
            // indented. So if no indenting is set, fake it. This is done by adding the indenting to
            // the comment prefix, otherwise it would indent recursively.
            var isCopyrightTag = indentLevel == 0 && string.Equals(line.TagName, "copyright", StringComparison.OrdinalIgnoreCase);
            if (isCopyrightTag && Settings.Default.Formatting_CommentXmlValueIndent < 1)
            {
                _commentPrefixLength += CodeCommentHelper.CopyrightExtraIndent;
                _commentPrefix += string.Empty.PadLeft(CodeCommentHelper.CopyrightExtraIndent);
            }

            // Increase the indent level.
            indentLevel++;

            bool isLiteralContent = line.Content != null;
            bool tagOnOwnLine = isLiteralContent || TagsOnOwnLine(line, indentLevel);

            if (tagOnOwnLine)
            {
                // Literals are output without indenting.
                NewLine(isLiteralContent ? 0 : indentLevel);
            }

            // If the literal content of an XML tag is set, output that content without formatting.
            if (isLiteralContent)
            {
                var literals = line.Content.Trim('\r', '\n').TrimEnd('\r', '\n', '\t', ' ').Split('\n');
                for (int i = 0; i < literals.Length; i++)
                {
                    Append(literals[i].TrimEnd());

                    // Append newline for all except the last line. Literals are output without indenting.
                    if (i + 1 < literals.Length)
                        NewLine(0, true);
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
                    tagOnOwnLine |= Parse(l, indentLevel, xmlTagLength);
                }
            }

            indentLevel--;

            // Undo the indenting hack done for copyright tags.
            if (isCopyrightTag && Settings.Default.Formatting_CommentXmlValueIndent < 1)
            {
                _commentPrefixLength -= CodeCommentHelper.CopyrightExtraIndent;
                _commentPrefix = _commentPrefix.Substring(0, _commentPrefixLength);
            }

            // If opening tag was on own line, do the same for the closing tag.
            if (tagOnOwnLine && !_isFirstWord)
            {
                NewLine(indentLevel);
            }

            Append(line.Closetag);
        }

        /// <summary>
        /// Check if the open and close tags for an XML line should be on their own lines or not.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="indentlevel"></param>
        /// <returns><c>true</c> if the tags should be split, otherwise <c>false</c>.</returns>
        private bool TagsOnOwnLine(CommentLineXml line, int indentlevel)
        {
            // Check for splitting all root level tags.
            if (Settings.Default.Formatting_CommentXmlSplitAllTags && indentlevel <= 1)
                return true;

            // Split if there is more than one child line.
            if (line.Lines.Count > 1)
                return true;

            // Split if there is literal content (eg. a code tag).
            if (line.Content != null)
                return true;

            // Split if this is a summary tag and option to split is set.
            if (Settings.Default.Formatting_CommentXmlSplitSummaryTagToMultipleLines && string.Equals(line.TagName, "summary", StringComparison.OrdinalIgnoreCase))
                return true;

            // Always split on StyleCop SA1633 copyright tag.
            if (Settings.Default.Formatting_CommentXmlSplitSummaryTagToMultipleLines && string.Equals(line.TagName, "copyright", StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        /// <summary>
        /// Return the length of a string, taking into account the tab characters.
        /// </summary>
        /// <param name="word">The string to return the length of.</param>
        /// <returns>The length of the string.</returns>
        private int WordLength(string word)
        {
            return word == null ? 0 : word.Length + word.Count(c => c == '\t') * (_tabSize - 1);
        }

        #endregion Methods
    }
}