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
        public CodeCommentOptions()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeCommentOptions" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <param name="document">The text document.</param>
        public CodeCommentOptions(CodeMaidPackage package, TextDocument document)
        {
            TabSize = CodeCommentHelper.GetTabSize(package, document);

            SkipWrapOnLastWord = Settings.Default.Formatting_CommentSkipWrapOnLastWord;
            WrapAtColumn = Math.Max(Settings.Default.Formatting_CommentWrapColumn, 20);

            XmlAlignParamTags = Settings.Default.Formatting_CommentXmlAlignParamTags;
            XmlSpaceTagContent = Settings.Default.Formatting_CommentXmlSpaceTags;
            XmlValueIndent = Settings.Default.Formatting_CommentXmlValueIndent;
        }

        #endregion Constructors

        #region Properties

        public bool SkipWrapOnLastWord { get; set; }

        public int TabSize { get; set; }

        public int WrapAtColumn { get; set; }

        public bool XmlAlignParamTags { get; set; }

        public bool XmlBreakAllTags { get; set; }

        public bool XmlKeepTagsTogether { get; set; }

        public bool XmlSpaceSingleTags { get; set; }

        public bool XmlSpaceTagContent { get; set; }

        public bool XmlTagsToLowerCase { get; set; }

        public int XmlValueIndent { get; set; }

        #endregion Properties
    }
}