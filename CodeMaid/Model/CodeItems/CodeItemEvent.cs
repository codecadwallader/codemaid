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
using SteveCadwallader.CodeMaid.Helpers;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// The representation of a code event.
    /// </summary>
    public class CodeItemEvent : BaseCodeItemElement
    {
        #region BaseCodeItem Overrides

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public override KindCodeItem Kind
        {
            get { return KindCodeItem.Event; }
        }

        #endregion BaseCodeItem Overrides

        #region BaseCodeItemElement Overrides

        /// <summary>
        /// Gets the access level.
        /// </summary>
        public override vsCMAccess Access
        {
            // Make exceptions for explicit interface implementations - which report private access but really do not have a meaningful access level.
            get { return TryDefault(() => CodeEvent != null && !IsExplicitInterfaceImplementation ? CodeEvent.Access : vsCMAccess.vsCMAccessDefault); }
        }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        public override CodeElements Attributes
        {
            get { return CodeEvent != null ? CodeEvent.Attributes : null; }
        }

        /// <summary>
        /// Gets the doc comment.
        /// </summary>
        public override string DocComment
        {
            get { return TryDefault(() => CodeEvent != null ? CodeEvent.DocComment : null); }
        }

        /// <summary>
        /// Gets a flag indicating if this event is static.
        /// </summary>
        public override bool IsStatic
        {
            get { return TryDefault(() => CodeEvent != null && CodeEvent.IsShared); }
        }

        /// <summary>
        /// Gets the type string.
        /// </summary>
        public override string TypeString
        {
            get { return TryDefault(() => CodeEvent != null ? CodeEvent.Type.AsString : null); }
        }

        #endregion BaseCodeItemElement Overrides

        #region Properties

        /// <summary>
        /// Gets or sets the VSX CodeEvent.
        /// </summary>
        public CodeEvent CodeEvent { get; set; }

        /// <summary>
        /// Gets a flag indicating if this property is an explicit interface implementation.
        /// </summary>
        public bool IsExplicitInterfaceImplementation
        {
            get { return TryDefault(() => CodeEvent != null && ExplicitInterfaceImplementationHelper.IsExplicitInterfaceImplementation(CodeEvent)); }
        }

        #endregion Properties
    }
}