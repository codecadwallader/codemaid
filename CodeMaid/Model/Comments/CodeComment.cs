using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using System;
using System.Linq;
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
        #region Fields

        private readonly TextDocument _document;
        private readonly Regex _commentLineRegex;
        private readonly int _tabSize;

        private EditPoint _endPoint;
        private EditPoint _startPoint;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeComment" /> class.
        /// </summary>
        public CodeComment(TextPoint point, int tabSize)
        {
            if (point == null)
            {
                throw new ArgumentNullException(nameof(point));
            }

            _document = point.Parent;
            _commentLineRegex = CodeCommentHelper.GetCommentRegex(_document.GetCodeLanguage());
            _tabSize = tabSize;

            Expand(point);
        }

        #endregion Constructors

        #region Properties

        public TextPoint EndPoint => _endPoint;

        public bool IsValid { get; private set; }

        public TextPoint StartPoint => _startPoint;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Helper function to generate the preview in the options menu.
        /// </summary>
        public static string FormatXml(string text)
        {
            var xml = XElement.Parse($"<doc>{text}</doc>");
            var line = new CommentLineXml(xml);
            var regex = CodeCommentHelper.GetCommentRegex(CodeLanguage.CSharp, false);
            var formatter = new CommentFormatter(line, "///", 4, regex);

            return formatter.ToString();
        }

        /// <summary>
        /// Formats the comment.
        /// </summary>
        public TextPoint Format()
        {
            if (!IsValid)
            {
                throw new InvalidOperationException("Cannot format comment, the comment is not valid.");
            }

            var originalText = _startPoint.GetText(_endPoint);
            var matches = _commentLineRegex.Matches(originalText).OfType<Match>().ToArray();
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
                    var xml = XElement.Parse($"<doc>{commentText}</doc>");
                    line = new CommentLineXml(xml);
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
                _tabSize,
                CodeCommentHelper.GetCommentRegex(_document.GetCodeLanguage(), false));

            if (!formatter.Equals(originalText))
            {
                var cursor = StartPoint.CreateEditPoint();
                cursor.Delete(EndPoint);
                cursor.Insert(formatter.ToString());
                _endPoint = cursor.CreateEditPoint();
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
            _startPoint = Expand(point, p => p.LineUp());

            // If a valid start is found, look down to find the end of the comment.
            if (_startPoint != null)
            {
                _endPoint = Expand(point, p => p.LineDown());
            }

            // If both start and endpoint are valid, the comment is valid.
            if (_startPoint != null && _endPoint != null)
            {
                _startPoint.StartOfLine();
                _endPoint.EndOfLine();
                IsValid = true;
            }
            else
            {
                IsValid = false;
            }
        }

        /// <summary>
        /// Expand a textpoint to the full comment, in the direction specified by the <paramref name="foundAction"/>.
        /// </summary>
        /// <param name="point">The initial starting point for the expansion.</param>
        /// <param name="foundAction">An action which advances the search either up or down.</param>
        /// <returns>
        /// The endpoint of the comment, or <c>null</c> if the expansion did not find a valid comment.
        /// </returns>
        private EditPoint Expand(TextPoint point, Action<EditPoint> foundAction)
        {
            EditPoint current = point.CreateEditPoint();
            EditPoint result = null;
            string prefix = null;

            do
            {
                var line = current.Line;
                var text = current.GetLine();

                var match = _commentLineRegex.Match(text);
                if (match.Success)
                {
                    // Cancel the expansion if the prefix does not match. This takes priority over
                    // the initial spacer check to allow formatting comments adjacent to Stylecop
                    // SA1626 style commented code.
                    var currentPrefix = match.Groups["prefix"].Value.TrimStart();
                    if (prefix != null && !string.Equals(prefix, currentPrefix))
                    {
                        break;
                    }
                    else
                    {
                        prefix = currentPrefix;
                    }

                    // The initial spacer is required, otherwise we assume this is commented out
                    // code and do not format.
                    if (match.Groups["initialspacer"].Success)
                    {
                        result = current.CreateEditPoint();
                        foundAction(current);

                        // If result and iterator line are the same, the found action (move line up or
                        // down) did nothing. This means we're at the start or end of the file, and
                        // there is no point to keep searching, it would create an infinite loop.
                        if (result.Line == current.Line)
                        {
                            break;
                        }
                    }
                    else
                    {
                        // Did not succesfully match the intial spacer, we have to assume this is
                        // code and cancel all formatting.
                        result = null;
                        current = null;
                    }
                }
                else
                {
                    current = null;
                }
            } while (current != null);

            return result;
        }

        #endregion Methods
    }
}