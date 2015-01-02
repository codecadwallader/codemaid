#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

using EnvDTE;
using EnvDTE80;
using System;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// The representation of a code field.
    /// </summary>
    public class CodeItemField : BaseCodeItemElement
    {
        #region Fields

        private readonly Lazy<bool> _isConstant;
        private readonly Lazy<bool> _isEnumItem;
        private readonly Lazy<bool> _isReadOnly;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeItemField" /> class.
        /// </summary>
        public CodeItemField()
        {
            _Access = LazyTryDefault(
                () => CodeVariable != null ? CodeVariable.Access : vsCMAccess.vsCMAccessPublic);

            _Attributes = LazyTryDefault(
                () => CodeVariable != null ? CodeVariable.Attributes : null);

            _DocComment = LazyTryDefault(
                () => CodeVariable != null ? CodeVariable.DocComment : null);

            _isConstant = LazyTryDefault(
                () => CodeVariable != null && CodeVariable.IsConstant && CodeVariable.ConstKind == vsCMConstKind.vsCMConstKindConst);

            _isEnumItem = LazyTryDefault(
                () => CodeVariable != null && CodeVariable.Parent != null && CodeVariable.Parent is CodeEnum);

            _isReadOnly = LazyTryDefault(
                () => CodeVariable != null && CodeVariable.IsConstant && CodeVariable.ConstKind == vsCMConstKind.vsCMConstKindReadOnly);

            _IsStatic = LazyTryDefault(
                () => CodeVariable != null && CodeVariable.IsShared);

            _TypeString = LazyTryDefault(
                () => CodeVariable != null && CodeVariable.Type != null ? CodeVariable.Type.AsString : null);
        }

        #endregion Constructors

        #region BaseCodeItem Overrides

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public override KindCodeItem Kind
        {
            get { return KindCodeItem.Field; }
        }

        /// <summary>
        /// Loads all lazy initialized values immediately.
        /// </summary>
        public override void LoadLazyInitializedValues()
        {
            base.LoadLazyInitializedValues();

            var ic = IsConstant;
            var ie = IsEnumItem;
            var isro = IsReadOnly;
        }

        #endregion BaseCodeItem Overrides

        #region Properties

        /// <summary>
        /// Gets or sets the underlying VSX CodeVariable.
        /// </summary>
        public CodeVariable2 CodeVariable { get; set; }

        /// <summary>
        /// Gets a flag indicating if this field is a constant.
        /// </summary>
        public bool IsConstant { get { return _isConstant.Value; } }

        /// <summary>
        /// Gets a flag indicating if this field is an enumeration item.
        /// </summary>
        public bool IsEnumItem { get { return _isEnumItem.Value; } }

        /// <summary>
        /// Gets a flag indicating if this field is read-only.
        /// </summary>
        public bool IsReadOnly { get { return _isReadOnly.Value; } }

        #endregion Properties
    }
}