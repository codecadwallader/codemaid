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
            SkipWrapOnLastWord = Settings.Default.Cleaning_CommentSkipWrapOnLastWord;
            TabSize = CodeCommentHelper.GetTabSize(package, document);
            WrapAtColumn = Math.Max(Settings.Default.Cleaning_CommentWrapColumn, 20);
            XmlAlignParamTags = Settings.Default.Cleaning_CommentXmlAlignParamTags;
            XmlSpaceTags = Settings.Default.Cleaning_CommentXmlSpaceTags;
            XmlValueIndent = Settings.Default.Cleaning_CommentXmlValueIndent;
        }

        #endregion Constructors

        #region Properties

        public bool SkipWrapOnLastWord { get; set; }

        public int TabSize { get; set; }

        public int WrapAtColumn { get; set; }

        public bool XmlAlignParamTags { get; set; }

        public bool XmlSpaceTags { get; set; }

        public int XmlValueIndent { get; set; }

        #endregion Properties
    }
}