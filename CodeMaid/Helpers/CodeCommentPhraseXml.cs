#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using System.Collections.Generic;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A code comment phrase containing XML content.
    /// </summary>
    internal class CodeCommentPhraseXml : ICodeCommentPhrase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeCommentPhraseXml" /> class.
        /// </summary>
        /// <param name="openTag">The opening tag.</param>
        /// <param name="closeTag">The closing tag.</param>
        public CodeCommentPhraseXml(string openTag, string closeTag)
        {
            OpenTag = openTag;
            CloseTag = closeTag;

            Phrases = new LinkedList<ICodeCommentPhrase>();
        }

        #endregion Constructors

        #region Properties

        public string OpenTag { get; set; }

        public string CloseTag { get; set; }

        public LinkedList<ICodeCommentPhrase> Phrases { get; private set; }

        #endregion Properties

        #region Methods

        internal void Add(LinkedList<ICodeCommentPhrase> phrases)
        {
            if (phrases != null)
            {
                foreach (var phrase in phrases)
                {
                    Phrases.AddLast(phrase);
                }
            }
        }

        #endregion Methods
    }
}