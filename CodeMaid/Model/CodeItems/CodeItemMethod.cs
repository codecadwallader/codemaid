#region CodeMaid is Copyright 2007-2014 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2014 Steve Cadwallader.

using EnvDTE;
using EnvDTE80;
using SteveCadwallader.CodeMaid.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// The representation of a code method.
    /// </summary>
    public class CodeItemMethod : BaseCodeItemElement, ICodeItemComplexity, ICodeItemParameters
    {
        #region Fields

        private readonly Lazy<int> _complexity;
        private readonly Lazy<bool> _isConstructor;
        private readonly Lazy<bool> _isDestructor;
        private readonly Lazy<bool> _isExplicitInterfaceImplementation;
        private readonly Lazy<vsCMOverrideKind> _overrideKind;
        private readonly Lazy<IEnumerable<CodeParameter>> _parameters;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeItemMethod" /> class.
        /// </summary>
        public CodeItemMethod()
        {
            // Make exceptions for static constructors and explicit interface implementations -
            // which report private access but really do not have a meaningful access level.
            _Access = LazyTryDefault(
                () => CodeFunction != null && !(IsStatic && IsConstructor) && !IsExplicitInterfaceImplementation ? CodeFunction.Access : vsCMAccess.vsCMAccessPublic);

            _Attributes = LazyTryDefault(
                () => CodeFunction != null ? CodeFunction.Attributes : null);

            _complexity = LazyTryDefault(
                () => CodeElementHelper.CalculateComplexity(CodeElement));

            _DocComment = LazyTryDefault(
                () => CodeFunction != null ? CodeFunction.DocComment : null);

            _isConstructor = LazyTryDefault(
                () => CodeFunction != null && CodeFunction.FunctionKind == vsCMFunction.vsCMFunctionConstructor);

            _isDestructor = LazyTryDefault(
                () => CodeFunction != null && CodeFunction.FunctionKind == vsCMFunction.vsCMFunctionDestructor);

            _isExplicitInterfaceImplementation = LazyTryDefault(
                () => CodeFunction != null && ExplicitInterfaceImplementationHelper.IsExplicitInterfaceImplementation(CodeFunction));

            _IsStatic = LazyTryDefault(
                () => CodeFunction != null && CodeFunction.IsShared);

            _overrideKind = LazyTryDefault(
                () => CodeFunction != null ? CodeFunction.OverrideKind : vsCMOverrideKind.vsCMOverrideKindNone);

            _parameters = LazyTryDefault(
                () => CodeFunction != null && CodeFunction.Parameters != null ? CodeFunction.Parameters.Cast<CodeParameter>().ToList() : Enumerable.Empty<CodeParameter>());

            _TypeString = LazyTryDefault(
                () => CodeFunction != null && CodeFunction.Type != null ? CodeFunction.Type.AsString : null);
        }

        #endregion Constructors

        #region BaseCodeItem Overrides

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public override KindCodeItem Kind
        {
            get
            {
                if (IsConstructor)
                {
                    return KindCodeItem.Constructor;
                }

                if (IsDestructor)
                {
                    return KindCodeItem.Destructor;
                }

                return KindCodeItem.Method;
            }
        }

        /// <summary>
        /// Loads all lazy initialized values immediately.
        /// </summary>
        public override void LoadLazyInitializedValues()
        {
            base.LoadLazyInitializedValues();

            var c = Complexity;
            var ic = IsConstructor;
            var id = IsDestructor;
            var ieii = IsExplicitInterfaceImplementation;
            var ok = OverrideKind;
            var p = Parameters;
        }

        #endregion BaseCodeItem Overrides

        #region Properties

        /// <summary>
        /// Gets or sets the underlying VSX CodeFunction.
        /// </summary>
        public CodeFunction2 CodeFunction { get; set; }

        /// <summary>
        /// Gets the complexity.
        /// </summary>
        public int Complexity { get { return _complexity.Value; } }

        /// <summary>
        /// Gets a flag indicating if this method is a constructor.
        /// </summary>
        public bool IsConstructor { get { return _isConstructor.Value; } }

        /// <summary>
        /// Gets a flag indicating if this method is a destructor.
        /// </summary>
        public bool IsDestructor { get { return _isDestructor.Value; } }

        /// <summary>
        /// Gets a flag indicating if this method is an explicit interface implementation.
        /// </summary>
        public bool IsExplicitInterfaceImplementation { get { return _isExplicitInterfaceImplementation.Value; } }

        /// <summary>
        /// Gets the override kind (abstract, virtual, override, new), defaulting to none.
        /// </summary>
        public vsCMOverrideKind OverrideKind { get { return _overrideKind.Value; } }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        public IEnumerable<CodeParameter> Parameters { get { return _parameters.Value; } }

        #endregion Properties
    }
}