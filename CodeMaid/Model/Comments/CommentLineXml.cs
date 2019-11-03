using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Model.Comments.Options;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace SteveCadwallader.CodeMaid.Model.Comments
{
    internal class CommentLineXml : CommentLine
    {
        private static Regex InterpunctionRegex = new Regex(@"^[^\w]", RegexOptions.Compiled);

        private readonly FormatterOptions _formatterOptions;
        private readonly StringBuilder _innerText;

        public CommentLineXml(XElement xml, FormatterOptions formatterOptions)
            : base(null)
        {
            _formatterOptions = formatterOptions;
            _innerText = new StringBuilder();

            TagName = xml.Name.LocalName;
            TagOptions = _formatterOptions.Xml.GetTagOptions(TagName);

            OpenTag = CreateXmlOpenTag(xml, TagOptions);
            CloseTag = CreateXmlCloseTag(xml, TagOptions);
            IsSelfClosing = CloseTag == null;

            Lines = new List<ICommentLine>();
            ParseChildNodes(xml);
            CloseInnerText(true);
            IsLast = xml.NextNode == null;
        }

        public string CloseTag { get; }

        public bool IsSelfClosing { get; }

        public ICollection<ICommentLine> Lines { get; }

        public string OpenTag { get; internal set; }

        public string TagName { get; }

        public IXmlTagOptions TagOptions { get; }

        /// <summary>
        /// Creates the XML close tag string for an XElement.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>
        /// The XML close tag, or <c>null</c> if the element has no value and is a self-closing tag.
        /// </returns>
        private static string CreateXmlCloseTag(XElement element, IXmlTagOptions options)
        {
            if (element.IsEmpty)
            {
                return null;
            }

            return $"</{TagCase(element.Name.LocalName, options.Case)}>";
        }

        /// <summary>
        /// Creates the XML open tag string for an XElement.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The XML open tag. In case of an element without value, the tag is self-closing.</returns>
        private static string CreateXmlOpenTag(XElement element, IXmlTagOptions options)
        {
            var builder = new StringBuilder();
            var name = element.Name.LocalName;

            builder.Append("<");

            builder.Append(TagCase(name, options.Case));

            if (element.HasAttributes)
            {
                foreach (var attr in element.Attributes())
                {
                    builder.Append(CodeCommentHelper.Spacer);
                    builder.Append(attr);
                }
            }

            if (element.IsEmpty)
            {
                if (options.SpaceSelfClosing)
                {
                    builder.Append(CodeCommentHelper.Spacer);
                }

                builder.Append("/");
            }

            builder.Append(">");

            var result = builder.ToString();

            return options.KeepTogether ? CodeCommentHelper.SpaceToFake(result) : result;
        }

        private static bool StartsWithInterpunction(string value)
        {
            return InterpunctionRegex.IsMatch(value);
        }

        private static string TagCase(string tag, XmlTagCase tagCase)
        {
            return tagCase == XmlTagCase.LowerCase ? tag.ToLowerInvariant() :
                tagCase == XmlTagCase.UpperCase ? tag.ToUpperInvariant() :
                tag;
        }

        /// <summary>
        /// If there is text left in the buffer, parse and append it as a comment line.
        /// </summary>
        private void CloseInnerText(bool isLast)
        {
            if (_innerText.Length > 0)
            {
                Lines.Add(new CommentLine(_innerText.ToString()) { IsLast = isLast });
                _innerText.Clear();
            }
        }

        private bool NeedsXmlHandling(XElement e)
        {
            // All root level elements are always on their own line.
            if (e.Parent == null || e.Parent.Parent == null)
                return true;

            // Some special tags should also always be their own line.
            if (_formatterOptions.Xml.Tags.ContainsKey(e.Name.LocalName))
                return true;

            return false;
        }

        private void ParseChildNodes(XElement xml)
        {
            if (TagOptions.Literal)
            {
                // Read content literally and preserve all formatting.
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
                        var element = (XElement)node;

                        if (NeedsXmlHandling(element))
                        {
                            CloseInnerText(false);
                            Lines.Add(new CommentLineXml(element, _formatterOptions));
                        }
                        else
                        {
                            // If the tag is not forced to be on it's own line, append it to the
                            // current content as string.
                            _innerText.Append(CreateXmlOpenTag(element, TagOptions));

                            if (!element.IsEmpty)
                            {
                                if (TagOptions.SpaceContent)
                                {
                                    _innerText.Append(CodeCommentHelper.Spacer);
                                }

                                ParseChildNodes(element);

                                _innerText.Append(CreateXmlCloseTag(element, TagOptions));
                            }
                        }
                    }
                    else
                    {
                        // Always trim trailing
                        var value = node.ToString().TrimEnd(CodeCommentHelper.Spacer);

                        // If the parent is an element, trim the starting spaces.
                        if (node.PreviousNode == null && node.Parent.NodeType == XmlNodeType.Element && !TagOptions.SpaceContent)
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
                        if (node.NextNode != null || node.Parent.NodeType != XmlNodeType.Element || TagOptions.SpaceContent)
                        {
                            _innerText.Append(CodeCommentHelper.Spacer);
                        }
                    }

                    node = node.NextNode;
                }
            }
        }
    }
}