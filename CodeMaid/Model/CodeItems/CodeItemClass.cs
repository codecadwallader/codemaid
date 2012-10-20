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

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// The representation of a code class.
    /// </summary>
    public class CodeItemClass : BaseCodeItemElementParent
    {
        #region BaseCodeItem Overrides

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public override KindCodeItem Kind
        {
            get { return KindCodeItem.Class; }
        }

        #endregion BaseCodeItem Overrides

        #region BaseCodeItemElement Overrides

        /// <summary>
        /// Gets the access level.
        /// </summary>
        public override vsCMAccess Access
        {
            get { return TryDefault(() => CodeClass != null ? CodeClass.Access : vsCMAccess.vsCMAccessPublic); }
        }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        public override CodeElements Attributes
        {
            get { return CodeClass != null ? CodeClass.Attributes : null; }
        }

        /// <summary>
        /// Gets the doc comment.
        /// </summary>
        public override string DocComment
        {
            get { return TryDefault(() => CodeClass != null ? CodeClass.DocComment : null); }
        }

        /// <summary>
        /// Gets a flag indicating if this class is static.
        /// </summary>
        public override bool IsStatic
        {
            get { return TryDefault(() => CodeClass != null && CodeClass.IsShared); }
        }

        /// <summary>
        /// Gets the type string.
        /// </summary>
        public override string TypeString
        {
            get { return "class"; }
        }

        #endregion BaseCodeItemElement Overrides

        #region BaseCodeItemElementParent Overrides

        /// <summary>
        /// Gets the namespace.
        /// </summary>
        public override string Namespace
        {
            get { return TryDefault(() => CodeClass != null ? CodeClass.Namespace.Name : null); }
        }

        #endregion BaseCodeItemElementParent Overrides

        #region Properties

        /// <summary>
        /// Gets or sets the underlying VSX CodeClass.
        /// </summary>
        public CodeClass2 CodeClass { get; set; }

        #endregion Properties
    }
}