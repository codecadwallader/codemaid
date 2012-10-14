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
    /// The representation of a code field.
    /// </summary>
    public class CodeItemField : BaseCodeItemElement
    {
        #region BaseCodeItem Overrides

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public override KindCodeItem Kind
        {
            get { return KindCodeItem.Field; }
        }

        #endregion BaseCodeItem Overrides

        #region BaseCodeItemElement Overrides

        /// <summary>
        /// Gets the access level.
        /// </summary>
        public override vsCMAccess Access
        {
            get
            {
                return TryDefault(() =>
                       {
                           // Work-around for static C++ fields - in VS2012 checking the Access results in hanging Visual Studio.
                           if (CodeVariable != null && !(IsStatic && CodeVariable.Language == CodeModelLanguageConstants.vsCMLanguageVC))
                           {
                               return CodeVariable.Access;
                           }

                           return vsCMAccess.vsCMAccessDefault;
                       });
            }
        }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        public override CodeElements Attributes
        {
            get { return CodeVariable != null ? CodeVariable.Attributes : null; }
        }

        /// <summary>
        /// Gets the doc comment.
        /// </summary>
        public override string DocComment
        {
            get { return TryDefault(() => CodeVariable != null ? CodeVariable.DocComment : null); }
        }

        /// <summary>
        /// Gets a flag indicating if this field is static.
        /// </summary>
        public override bool IsStatic
        {
            get { return TryDefault(() => CodeVariable != null && CodeVariable.IsShared); }
        }

        /// <summary>
        /// Gets the type string.
        /// </summary>
        public override string TypeString
        {
            get { return TryDefault(() => CodeVariable != null ? CodeVariable.Type.AsString : null); }
        }

        #endregion BaseCodeItemElement Overrides

        #region Properties

        /// <summary>
        /// Gets or sets the underlying VSX CodeVariable.
        /// </summary>
        public CodeVariable2 CodeVariable { get; set; }

        /// <summary>
        /// Gets a flag indicating if this field is a constant.
        /// </summary>
        public bool IsConstant
        {
            get { return TryDefault(() => CodeVariable != null && CodeVariable.IsConstant && CodeVariable.ConstKind == vsCMConstKind.vsCMConstKindConst); }
        }

        /// <summary>
        /// Gets a flag indicating if this field is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return TryDefault(() => CodeVariable != null && CodeVariable.IsConstant && CodeVariable.ConstKind == vsCMConstKind.vsCMConstKindReadOnly); }
        }

        #endregion Properties
    }
}