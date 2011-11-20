#region CodeMaid is Copyright 2007-2011 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2011 Steve Cadwallader.

using EnvDTE;
using EnvDTE80;

namespace SteveCadwallader.CodeMaid.CodeItems
{
    /// <summary>
    /// The representation of a code delegate.
    /// </summary>
    public class CodeItemDelegate : BaseCodeItemElement
    {
        /// <summary>
        /// Gets or sets the underlying VSX CodeDelegate.
        /// </summary>
        public CodeDelegate2 CodeDelegate { get; set; }

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public override KindCodeItem Kind
        {
            get { return KindCodeItem.Delegate; }
        }

        /// <summary>
        /// Gets the access level.
        /// </summary>
        public override vsCMAccess Access
        {
            get { return CodeDelegate != null ? CodeDelegate.Access : vsCMAccess.vsCMAccessDefault; }
        }

        /// <summary>
        /// Gets the doc comment.
        /// </summary>
        public override string DocComment
        {
            get { return CodeDelegate != null ? CodeDelegate.DocComment : null; }
        }

        /// <summary>
        /// Gets the type string.
        /// </summary>
        public override string TypeString
        {
            get { return CodeDelegate != null ? CodeDelegate.Type.AsString : null; }
        }

        /// <summary>
        /// Gets the namespace.
        /// </summary>
        public string Namespace
        {
            get { return CodeDelegate != null ? CodeDelegate.Namespace.Name : null; }
        }
    }
}