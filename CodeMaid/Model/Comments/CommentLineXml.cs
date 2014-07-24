using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using SteveCadwallader.CodeMaid.Helpers;

namespace SteveCadwallader.CodeMaid.Model.Comments
{
    internal class CommentLineXml : CommentLine
    {
        private static string[] NewLineElementNames = { "p", "para", "code" };
        private StringBuilder innerText;

        public CommentLineXml(XElement xml, CodeCommentOptions options)
            : base(null)
        {
            this.TagName = xml.Name.LocalName;

            // Tags that are forced to be their own line should never be self closing. This prevents
            // empty tags from getting collapsed.
            this.OpenTag = CodeCommentHelper.CreateXmlOpenTag(xml, options, false);
            this.Closetag = CodeCommentHelper.CreateXmlCloseTag(xml, options, false);

            this.Lines = new List<ICommentLine>();

            this.innerText = new StringBuilder();
            this.ParseChildNodes(xml, options);
            this.CloseInnerText();
        }

        public string Closetag { get; set; }

        public ICollection<ICommentLine> Lines { get; private set; }

        public string OpenTag { get; set; }

        public string TagName { get; private set; }

        /// <summary>
        /// If there is text left in the buffer, parse and append it as a comment line.
        /// </summary>
        private void CloseInnerText()
        {
            if (this.innerText.Length > 0)
            {
                this.Lines.Add(new CommentLine(innerText.ToString()));
                this.innerText.Clear();
            }
        }

        private void ParseChildNodes(XElement xml, CodeCommentOptions options)
        {
            if (string.Equals(this.TagName, "code", StringComparison.OrdinalIgnoreCase))
            {
                // Content of code element should be read literally and preserve whitespace.
                using (var reader = xml.CreateReader())
                {
                    reader.MoveToContent();
                    this.Content = reader.ReadInnerXml();
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

                        // All root level elements and certain special sub elements always need to
                        // be on their own line.
                        if (e.Parent == null || e.Parent.Parent == null || NewLineElementNames.Contains(e.Name.LocalName, StringComparer.OrdinalIgnoreCase))
                        {
                            this.CloseInnerText();
                            this.Lines.Add(new CommentLineXml(e, options));
                        }
                        else
                        {
                            // If the tag is not forced to be on it's own line, append it to the
                            // current content as string.
                            this.innerText.Append(CodeCommentHelper.CreateXmlOpenTag(e, options));

                            if (!e.IsEmpty)
                            {
                                if (options.XmlSpaceTagContent)
                                {
                                    this.innerText.Append(CodeCommentHelper.Spacer);
                                }

                                this.ParseChildNodes(e, options);

                                this.innerText.Append(CodeCommentHelper.CreateXmlCloseTag(e, options));
                            }

                            this.innerText.Append(CodeCommentHelper.Spacer);
                        }
                    }
                    else
                    {
                        // Always trim trailing 
                        var value = node.ToString().TrimEnd(CodeCommentHelper.Spacer);

                        // If the parent is an element, trim the starting spaces.
                        if (node.PreviousNode == null && node.Parent.NodeType == XmlNodeType.Element && !options.XmlSpaceTagContent)
                        {
                            value = value.TrimStart(CodeCommentHelper.Spacer);
                        }

                        this.innerText.Append(value);

                        // Add spacing after (almost) each word.
                        if (node.NextNode != null || node.Parent.NodeType != XmlNodeType.Element || options.XmlSpaceTagContent)
                        {
                            this.innerText.Append(CodeCommentHelper.Spacer);
                        }
                    }

                    node = node.NextNode;
                }
            }
        }
    }
}