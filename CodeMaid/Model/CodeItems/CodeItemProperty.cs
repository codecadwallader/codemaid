#region CodeMaid is Copyright 2007-2013 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License version 3
// as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2013 Steve Cadwallader.

using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using EnvDTE80;
using SteveCadwallader.CodeMaid.Helpers;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// The representation of a code property.
    /// </summary>
    public class CodeItemProperty : BaseCodeItemElement, ICodeItemComplexity, ICodeItemParameters
    {
        #region Fields

        private readonly Lazy<int> _complexity;
        private readonly Lazy<bool> _isExplicitInterfaceImplementation;
        private readonly Lazy<bool> _isIndexer;
        private readonly Lazy<IEnumerable<CodeParameter>> _parameters;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeItemProperty"/> class.
        /// </summary>
        public CodeItemProperty()
        {
            // Make exceptions for explicit interface implementations - which report private access but really do not have a meaningful access level.
            _Access = LazyTryDefault(
                () => CodeProperty != null && !IsExplicitInterfaceImplementation ? CodeProperty.Access : vsCMAccess.vsCMAccessPublic);

            _Attributes = LazyTryDefault(
                () => CodeProperty != null ? CodeProperty.Attributes : null);

            _complexity = LazyTryDefault(
                () => CodeElementHelper.CalculateComplexity(CodeElement));

            _DocComment = LazyTryDefault(
                () => CodeProperty != null ? CodeProperty.DocComment : null);

            _isExplicitInterfaceImplementation = LazyTryDefault(
                () => CodeProperty != null && ExplicitInterfaceImplementationHelper.IsExplicitInterfaceImplementation(CodeProperty));

            _isIndexer = LazyTryDefault(
                () => CodeProperty != null && CodeProperty.Parameters != null && CodeProperty.Parameters.Count > 0);

            _IsStatic = LazyTryDefault(
                () => CodeProperty != null &&
                      ((CodeProperty.Getter != null && CodeProperty.Getter.IsShared) ||
                       (CodeProperty.Setter != null && CodeProperty.Setter.IsShared)));

            _parameters = LazyTryDefault(
                () => CodeProperty != null && CodeProperty.Parameters != null ? CodeProperty.Parameters.Cast<CodeParameter>().ToList() : Enumerable.Empty<CodeParameter>());

            _TypeString = LazyTryDefault(
                () => CodeProperty != null && CodeProperty.Type != null ? CodeProperty.Type.AsString : null);
        }

        #endregion Constructors

        #region BaseCodeItem Overrides

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public override KindCodeItem Kind
        {
            get { return IsIndexer ? KindCodeItem.Indexer : KindCodeItem.Property; }
        }

        /// <summary>
        /// Loads all lazy initialized values immediately.
        /// </summary>
        public override void LoadLazyInitializedValues()
        {
            base.LoadLazyInitializedValues();

            var c = Complexity;
            var ieii = IsExplicitInterfaceImplementation;
            var ii = IsIndexer;
            var p = Parameters;
        }

        #endregion BaseCodeItem Overrides

        #region Properties

        /// <summary>
        /// Gets or sets the underlying VSX CodeProperty.
        /// </summary>
        public CodeProperty2 CodeProperty { get; set; }

        /// <summary>
        /// Gets the complexity.
        /// </summary>
        public int Complexity { get { return _complexity.Value; } }

        /// <summary>
        /// Gets a flag indicating if this property is an explicit interface implementation.
        /// </summary>
        public bool IsExplicitInterfaceImplementation { get { return _isExplicitInterfaceImplementation.Value; } }

        /// <summary>
        /// Gets a flag indicating if this property is an indexer.
        /// </summary>
        public bool IsIndexer { get { return _isIndexer.Value; } }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        public IEnumerable<CodeParameter> Parameters { get { return _parameters.Value; } }

        #endregion Properties
    }
}