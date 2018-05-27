using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;
using System;
using System.Collections.Generic;

namespace SteveCadwallader.CodeMaid.Model.Comments.Options
{
    public class FormatterOptionsXml
    {
        private readonly static FormatterOptionsXmlTag FormatterOptionsXmlTagOverrideSplitBeforeAfter = new FormatterOptionsXmlTag
        {
            Split = XmlTagNewLine.BeforeAndAfter
        };

        public FormatterOptionsXml()
        {
            Tags = new Dictionary<string, FormatterOptionsXmlTag>(StringComparer.OrdinalIgnoreCase);
            Default = new XmlTagOptions();
        }

        /// <summary>
        /// Whether `param` tags should be all made the same length.
        /// </summary>
        public bool AlignParamTags { get; set; }

        public XmlTagOptions Default { get; set; }

        /// <summary>
        /// Settings for individual tags.
        /// </summary>
        public Dictionary<string, FormatterOptionsXmlTag> Tags { get; set; }

        public IXmlTagOptions GetTagOptions(string tagName)
        {
            return !Tags.TryGetValue(tagName, out var tag) ? Default : new XmlTagOptions(tag, Default);
        }

        internal static FormatterOptionsXml FromSettings(Settings settings)
        {
            return new FormatterOptionsXml
            {
                AlignParamTags = settings.Formatting_CommentXmlAlignParamTags,
                Default = XmlTagOptions.FromSettings(settings),
                Tags = new Dictionary<string, FormatterOptionsXmlTag>
                {
                    ["summary"] = new FormatterOptionsXmlTag { Split = settings.Formatting_CommentXmlSplitSummaryTagToMultipleLines ? XmlTagNewLine.Always : XmlTagNewLine.Default },
                    ["copyright"] = new FormatterOptionsXmlTag { Split = XmlTagNewLine.Always, Indent = CodeCommentHelper.CopyrightExtraIndent },
                    ["code"] = new FormatterOptionsXmlTag { Split = XmlTagNewLine.BeforeAndAfter, Literal = true },
                    ["p"] = FormatterOptionsXmlTagOverrideSplitBeforeAfter,
                    ["para"] = FormatterOptionsXmlTagOverrideSplitBeforeAfter,
                    ["list"] = FormatterOptionsXmlTagOverrideSplitBeforeAfter,
                    ["listheader"] = FormatterOptionsXmlTagOverrideSplitBeforeAfter,
                    ["item"] = FormatterOptionsXmlTagOverrideSplitBeforeAfter,
                    ["term"] = FormatterOptionsXmlTagOverrideSplitBeforeAfter,
                    ["description"] = FormatterOptionsXmlTagOverrideSplitBeforeAfter,
                }
            };
        }
    };
}