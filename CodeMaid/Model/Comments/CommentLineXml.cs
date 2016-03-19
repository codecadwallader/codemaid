using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace SteveCadwallader.CodeMaid.Model.Comments
{
    internal class CommentLineXml : CommentLine
    {
        #region Fields

        // Elements with these names always start and end on their own line, and are only split
        // based on content.
        internal static string[] SingleLineElementNames = { "p", "para", "list", "listheader", "item", "term", "description", "code" };

        private static Regex InterpunctionRegex = new Regex(@"^[^\w]", RegexOptions.Compiled);
        private StringBuilder _innerText;

        #endregion Fields

        #region Constructors

        public CommentLineXml(XElement xml)
            : base(null)
        {
            TagName = xml.Name.LocalName;

            // Tags that are forced to be their own line should never be self closing. This prevents
            // empty tags from getting collapsed.
            OpenTag = CodeCommentHelper.CreateXmlOpenTag(xml);
            CloseTag = CodeCommentHelper.CreateXmlCloseTag(xml);
            IsSelfClosing = CloseTag == null;

            Lines = new List<ICommentLine>();

            _innerText = new StringBuilder();
            ParseChildNodes(xml);
            CloseInnerText();
        }

        #endregion Constructors

        #region Properties

        public string CloseTag { get; private set; }

        public bool IsSelfClosing { get; private set; }

        public ICollection<ICommentLine> Lines { get; private set; }

        public string OpenTag { get; set; }

        public string TagName { get; private set; }

        #endregion Properties

        #region Methods

        private static bool StartsWithInterpunction(string value)
        {
            return InterpunctionRegex.IsMatch(value);
        }

        /// <summary>
        /// If there is text left in the buffer, parse and append it as a comment line.
        /// </summary>
        private void CloseInnerText()
        {
            if (_innerText.Length > 0)
            {
                Lines.Add(new CommentLine(_innerText.ToString()));
                _innerText.Clear();
            }
        }

        private void ParseChildNodes(XElement xml)
        {
            if (string.Equals(TagName, "code", StringComparison.OrdinalIgnoreCase))
            {
                // Content of code element should be read literally and preserve whitespace.
                using (var reader = xml.CreateReader())
                {
                    reader.MoveToContent();
                    Content = reader.ReadInnerXml();
                }
            }
            else
            {
                // Loop and parse all child nodes.
                var node = xml.FirstNode;
                while (node != null)
                {
                    // If the node is a sub-element, it needs to be handled seperately.
                    if (node.NodeType == XmlNodeType.Element)
                    {
                        var e = (XElement)node;

                        if (ShouldBeNewLine(e))
                        {
                            CloseInnerText();
                            Lines.Add(new CommentLineXml(e));
                        }
                        else
                        {
                            // If the tag is not forced to be on it's own line, append it to the
                            // current content as string.
                            _innerText.Append(CodeCommentHelper.CreateXmlOpenTag(e));

                            if (!e.IsEmpty)
                            {
                                if (Settings.Default.Formatting_CommentXmlSpaceTags)
                                {
                                    _innerText.Append(CodeCommentHelper.Spacer);
                                }

                                ParseChildNodes(e);

                                _innerText.Append(CodeCommentHelper.CreateXmlCloseTag(e));
                            }
                        }
                    }
                    else
                    {
                        // Always trim trailing
                        var value = node.ToString().TrimEnd(CodeCommentHelper.Spacer);

                        // If the parent is an element, trim the starting spaces.
                        if (node.PreviousNode == null && node.Parent.NodeType == XmlNodeType.Element && !Settings.Default.Formatting_CommentXmlSpaceTags)
                        {
                            value = value.TrimStart(CodeCommentHelper.Spacer);
                        }

                        // If the previous node was an XML element, put a space before the text
                        // unless the first character is interpunction.
                        if (node.PreviousNode != null && node.PreviousNode.NodeType == XmlNodeType.Element)
                        {
                            if (!StartsWithInterpunction(value))
                            {
                                _innerText.Append(CodeCommentHelper.Spacer);
                            }
                        }

                        _innerText.Append(value);

                        // Add spacing after (almost) each word.
                        if (node.NextNode != null || node.Parent.NodeType != XmlNodeType.Element || Settings.Default.Formatting_CommentXmlSpaceTags)
                        {
                            _innerText.Append(CodeCommentHelper.Spacer);
                        }
                    }

                    node = node.NextNode;
                }
            }
        }

        private bool ShouldBeNewLine(XElement e)
        {
            // All root level elements are always on their own line.
            if (e.Parent == null || e.Parent.Parent == null)
                return true;

            // Some special tags should also always be their own line.
            if (SingleLineElementNames.Contains(e.Name.LocalName, StringComparer.OrdinalIgnoreCase))
                return true;

            return false;
        }

        #endregion Methods
    }
}