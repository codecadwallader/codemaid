#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using System;
using EnvDTE;
using SteveCadwallader.CodeMaid.Helpers;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.Model.Comments
{
    /// <summary>
    /// A data structure class used to store the options passed to the comment formatter.
    /// </summary>
    internal class CodeCommentOptions
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeCommentOptions" /> class.
        /// </summary>
        public CodeCommentOptions(Settings settings)
        {
            SkipWrapOnLastWord = settings.Formatting_CommentSkipWrapOnLastWord;
            WrapAtColumn = Math.Max(settings.Formatting_CommentWrapColumn, 20);
            FormatDuringCleanup = settings.Formatting_CommentRunDuringCleanup;

            XmlAlignParamTags = settings.Formatting_CommentXmlAlignParamTags;
            XmlSpaceTagContent = settings.Formatting_CommentXmlSpaceTags;
            XmlValueIndent = settings.Formatting_CommentXmlValueIndent;
            XmlSplitSummaryTag = settings.Formatting_CommentXmlSplitSummaryTagToMultipleLines;
            XmlSplitAllTags = settings.Formatting_CommentXmlSplitAllTags;
            XmlSpaceSingleTags = settings.Formatting_CommentXmlSpaceSingleTags;
            XmlTagsToLowerCase = settings.Reorganizing_RegionsInsertNewRegions;
            XmlKeepTagsTogether = settings.Formatting_CommentXmlKeepTagsTogether;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeCommentOptions" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <param name="document">The text document.</param>
        public CodeCommentOptions(Settings settings, CodeMaidPackage package, TextDocument document)
            : this(settings, CodeCommentHelper.GetTabSize(package, document))
        {
        }

        public CodeCommentOptions(Settings settings, int tabSize)
            : this(settings)
        {
            TabSize = tabSize;
        }

        #endregion Constructors

        #region Methods

        internal void Save(Settings settings)
        {
            settings.Formatting_CommentSkipWrapOnLastWord = SkipWrapOnLastWord;
            settings.Formatting_CommentWrapColumn = WrapAtColumn;
            settings.Formatting_CommentRunDuringCleanup = FormatDuringCleanup;

            settings.Formatting_CommentXmlAlignParamTags = XmlAlignParamTags;
            settings.Formatting_CommentXmlSpaceTags = XmlSpaceTagContent;
            settings.Formatting_CommentXmlValueIndent = XmlValueIndent;
            settings.Formatting_CommentXmlSplitSummaryTagToMultipleLines = XmlSplitSummaryTag;
            settings.Formatting_CommentXmlSplitAllTags = XmlSplitAllTags;
            settings.Formatting_CommentXmlSpaceSingleTags = XmlSpaceSingleTags;
            settings.Reorganizing_RegionsInsertNewRegions = XmlTagsToLowerCase;
            settings.Formatting_CommentXmlKeepTagsTogether = XmlKeepTagsTogether;
        }

        #endregion Methods

        #region Properties

        public bool FormatDuringCleanup { get; set; }

        public bool SkipWrapOnLastWord { get; set; }

        public int TabSize { get; set; }

        public int WrapAtColumn { get; set; }

        public bool XmlAlignParamTags { get; set; }

        public bool XmlKeepTagsTogether { get; set; }

        public bool XmlSpaceSingleTags { get; set; }

        public bool XmlSpaceTagContent { get; set; }

        public bool XmlSplitAllTags { get; set; }

        public bool XmlSplitSummaryTag { get; set; }

        public bool XmlTagsToLowerCase { get; set; }

        public int XmlValueIndent { get; set; }

        #endregion Properties
    }
}