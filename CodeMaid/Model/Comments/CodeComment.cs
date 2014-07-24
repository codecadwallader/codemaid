#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace SteveCadwallader.CodeMaid.Model.Comments
{
    /// <summary>
    /// A <c>CodeComment</c> contains one or more <see cref="CodeCommentPhrase">phrases</see> which
    /// represent all the content of a comment.
    /// </summary>
    internal class CodeComment
    {
        private readonly TextDocument document;
        private Regex codeLineRegex;
        private Regex commentLineRegex;

        private EditPoint endPoint;
        private EditPoint startPoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeComment" /> class.
        /// </summary>
        public CodeComment(TextPoint point)
        {
            if (point == null)
                throw new ArgumentNullException("point");

            this.document = point.Parent;

            this.commentLineRegex = CodeCommentHelper.GetCommentRegex(this.document.Language, true);
            this.codeLineRegex = CodeCommentHelper.GetCodeCommentRegex(this.document.Language);

            this.Expand(point);
        }

        public TextPoint EndPoint { get { return endPoint; } }

        public bool IsValid { get; private set; }

        public TextPoint StartPoint { get { return startPoint; } }

        /// <summary>
        /// Helper function to generate the preview in the options menu.
        /// </summary>
        public static string FormatXml(string text, CodeCommentOptions options)
        {
            var xml = XElement.Parse(string.Format("<doc>{0}</doc>", text));
            var line = new CommentLineXml(xml, options);
            var regex = CodeCommentHelper.GetCommentRegex("CSharp", false);
            var formatter = new CommentFormatter(line, "///", options, regex);
            return formatter.ToString();
        }

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

            // Concatenate the comment lines without comment prefixes and see if the resulting bit
            // can be parsed as XML.
            ICommentLine line = null;
            var lineTexts = matches.Select(m => m.Groups["line"].Value).ToArray();
            var commentText = string.Join(Environment.NewLine, lineTexts);
            if (commentText.Contains('<'))
            {
                try
                {
                    var xml = XElement.Parse(string.Format("<doc>{0}</doc>", commentText));
                    line = new CommentLineXml(xml, options);
                }
                catch (System.Xml.XmlException)
                {
                    // If XML cannot be parsed, comment will be handled as a normal text comment.
                }
            }

            if (line == null)
            {
                line = new CommentLine(commentText);
            }

            var formatter = new CommentFormatter(
                line,
                commentPrefix,
                options,
                CodeCommentHelper.GetCommentRegex(this.document.Language, false));

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

            // Look up to find the start of the comment.
            startPoint = Expand(point, (p) => p.LineUp());

            // If a valid start is found, look down to find the end of the comment.
            if (startPoint != null)
                endPoint = Expand(point, (p) => p.LineDown());

            if (StartPoint != null && EndPoint != null)
            {
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
    }
}