#region CodeMaid is Copyright 2007-2012 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2012 Steve Cadwallader.

using EnvDTE;
using EnvDTE80;

namespace SteveCadwallader.CodeMaid.CodeItems
{
    /// <summary>
    /// The representation of a code event.
    /// </summary>
    public class CodeItemEvent : BaseCodeItemElement
    {
        /// <summary>
        /// Gets or sets the VSX CodeEvent.
        /// </summary>
        public CodeEvent CodeEvent { get; set; }

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public override KindCodeItem Kind
        {
            get { return KindCodeItem.Event; }
        }

        /// <summary>
        /// Gets the access level.
        /// </summary>
        public override vsCMAccess Access
        {
            get { return CodeEvent != null ? CodeEvent.Access : vsCMAccess.vsCMAccessDefault; }
        }

        /// <summary>
        /// Gets a flag indicating if this event is static.
        /// </summary>
        public override bool IsStatic
        {
            get { return CodeEvent != null && CodeEvent.IsShared; }
        }

        /// <summary>
        /// Gets the doc comment.
        /// </summary>
        public override string DocComment
        {
            get { return CodeEvent != null ? CodeEvent.DocComment : null; }
        }

        /// <summary>
        /// Gets the type string.
        /// </summary>
        public override string TypeString
        {
            get { return CodeEvent != null ? CodeEvent.Type.AsString : null; }
        }
    }
}