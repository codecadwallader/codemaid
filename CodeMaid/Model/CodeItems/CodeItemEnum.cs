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

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// The representation of a code enumeration.
    /// </summary>
    public class CodeItemEnum : BaseCodeItemElementParent
    {
        #region BaseCodeItem Overrides

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public override KindCodeItem Kind
        {
            get { return KindCodeItem.Enum; }
        }

        #endregion BaseCodeItem Overrides

        #region BaseCodeItemElement Overrides

        /// <summary>
        /// Gets the access level.
        /// </summary>
        public override vsCMAccess Access
        {
            get { return TryDefault(() => CodeEnum != null ? CodeEnum.Access : vsCMAccess.vsCMAccessDefault); }
        }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        public override CodeElements Attributes
        {
            get { return CodeEnum != null ? CodeEnum.Attributes : null; }
        }

        /// <summary>
        /// Gets the doc comment.
        /// </summary>
        public override string DocComment
        {
            get { return TryDefault(() => CodeEnum != null ? CodeEnum.DocComment : null); }
        }

        /// <summary>
        /// Gets the type string.
        /// </summary>
        public override string TypeString
        {
            get { return "enum"; }
        }

        #endregion BaseCodeItemElement Overrides

        #region BaseCodeItemElementParent Overrides

        /// <summary>
        /// Gets the namespace.
        /// </summary>
        public override string Namespace
        {
            get { return TryDefault(() => CodeEnum != null ? CodeEnum.Namespace.Name : null); }
        }

        #endregion BaseCodeItemElementParent Overrides

        #region Properties

        /// <summary>
        /// Gets or sets the underlying VSX CodeEnum.
        /// </summary>
        public CodeEnum CodeEnum { get; set; }

        #endregion Properties
    }
}