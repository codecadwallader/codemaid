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

using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using EnvDTE80;
using SteveCadwallader.CodeMaid.Helpers;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// The representation of a code method.
    /// </summary>
    public class CodeItemMethod : BaseCodeItemElement, ICodeItemParameters
    {
        #region Fields

        private int? _complexity;

        #endregion Fields

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

        #endregion BaseCodeItem Overrides

        #region BaseCodeItemElement Overrides

        /// <summary>
        /// Gets the access level.
        /// </summary>
        public override vsCMAccess Access
        {
            // Make exceptions for static constructors and explicit interface implementations - which report private access but really do not have a meaningful access level.
            get { return TryDefault(() => CodeFunction != null && !(IsStatic && IsConstructor) && !IsExplicitInterfaceImplementation ? CodeFunction.Access : vsCMAccess.vsCMAccessPublic); }
        }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        public override CodeElements Attributes
        {
            get { return CodeFunction != null ? CodeFunction.Attributes : null; }
        }

        /// <summary>
        /// Gets the doc comment.
        /// </summary>
        public override string DocComment
        {
            get { return TryDefault(() => CodeFunction != null ? CodeFunction.DocComment : null); }
        }

        /// <summary>
        /// Gets a flag indicating if this method is static.
        /// </summary>
        public override bool IsStatic
        {
            get { return TryDefault(() => CodeFunction != null && CodeFunction.IsShared); }
        }

        /// <summary>
        /// Gets the return type.
        /// </summary>
        public override string TypeString
        {
            get { return TryDefault(() => CodeFunction != null ? CodeFunction.Type.AsString : null); }
        }

        #endregion BaseCodeItemElement Overrides

        #region Properties

        /// <summary>
        /// Gets or sets the underlying VSX CodeFunction.
        /// </summary>
        public CodeFunction2 CodeFunction { get; set; }

        /// <summary>
        /// Gets the complexity.
        /// </summary>
        public int Complexity
        {
            get
            {
                if (_complexity == null)
                {
                    _complexity = CodeModelHelper.CalculateComplexity(CodeElement);
                }

                return _complexity.Value;
            }
        }

        /// <summary>
        /// Gets a flag indicating if this method is a constructor.
        /// </summary>
        public bool IsConstructor
        {
            get { return TryDefault(() => CodeFunction != null && CodeFunction.FunctionKind == vsCMFunction.vsCMFunctionConstructor); }
        }

        /// <summary>
        /// Gets a flag indicating if this method is a destructor.
        /// </summary>
        public bool IsDestructor
        {
            get { return TryDefault(() => CodeFunction != null && CodeFunction.FunctionKind == vsCMFunction.vsCMFunctionDestructor); }
        }

        /// <summary>
        /// Gets a flag indicating if this method is an explicit interface implementation.
        /// </summary>
        public bool IsExplicitInterfaceImplementation
        {
            get { return TryDefault(() => CodeFunction != null && ExplicitInterfaceImplementationHelper.IsExplicitInterfaceImplementation(CodeFunction)); }
        }

        /// <summary>
        /// Gets a flag indicating if this method is overloaded.
        /// </summary>
        public bool IsOverloaded
        {
            get { return TryDefault(() => CodeFunction != null && CodeFunction.IsOverloaded); }
        }

        /// <summary>
        /// Gets the override kind (abstract, virtual, override, new), defaulting to none.
        /// </summary>
        public vsCMOverrideKind OverrideKind
        {
            get { return TryDefault(() => CodeFunction != null ? CodeFunction.OverrideKind : vsCMOverrideKind.vsCMOverrideKindNone); }
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        public IEnumerable<CodeParameter> Parameters
        {
            get { return TryDefault(() => CodeFunction != null ? CodeFunction.Parameters.Cast<CodeParameter>().ToList() : Enumerable.Empty<CodeParameter>()); }
        }

        #endregion Properties
    }
}