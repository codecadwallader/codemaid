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

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EnvDTE;
using EnvDTE80;
using SteveCadwallader.CodeMaid.Helpers;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// The representation of a code method.
    /// </summary>
    public class CodeItemMethod : BaseCodeItemElement, ICodeItemComplexity, ICodeItemParameters
    {
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
        /// Refreshes the cached fields on this item.
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();

            Task.Factory.StartNew(() =>
            {
                IsConstructor = TryDefault(() => CodeFunction != null && CodeFunction.FunctionKind == vsCMFunction.vsCMFunctionConstructor);
                IsDestructor = TryDefault(() => CodeFunction != null && CodeFunction.FunctionKind == vsCMFunction.vsCMFunctionDestructor);
                IsExplicitInterfaceImplementation = TryDefault(() => CodeFunction != null && ExplicitInterfaceImplementationHelper.IsExplicitInterfaceImplementation(CodeFunction));
                IsStatic = TryDefault(() => CodeFunction != null && CodeFunction.IsShared);

                // Make exceptions for static constructors and explicit interface implementations - which report private access but really do not have a meaningful access level.
                Access = TryDefault(() => CodeFunction != null && !(IsStatic && IsConstructor) && !IsExplicitInterfaceImplementation ? CodeFunction.Access : vsCMAccess.vsCMAccessPublic);
                Attributes = TryDefault(() => CodeFunction != null ? CodeFunction.Attributes : null);
                Complexity = CodeElementHelper.CalculateComplexity(CodeElement);
                DocComment = TryDefault(() => CodeFunction != null ? CodeFunction.DocComment : null);
                OverrideKind = TryDefault(() => CodeFunction != null ? CodeFunction.OverrideKind : vsCMOverrideKind.vsCMOverrideKindNone);
                Parameters = TryDefault(() => CodeFunction != null && CodeFunction.Parameters != null ? CodeFunction.Parameters.Cast<CodeParameter>().ToList() : Enumerable.Empty<CodeParameter>());
                TypeString = TryDefault(() => CodeFunction != null && CodeFunction.Type != null ? CodeFunction.Type.AsString : null);
            }, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Wait();
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
        public int Complexity { get; private set; }

        /// <summary>
        /// Gets a flag indicating if this method is a constructor.
        /// </summary>
        public bool IsConstructor { get; private set; }

        /// <summary>
        /// Gets a flag indicating if this method is a destructor.
        /// </summary>
        public bool IsDestructor { get; private set; }

        /// <summary>
        /// Gets a flag indicating if this method is an explicit interface implementation.
        /// </summary>
        public bool IsExplicitInterfaceImplementation { get; private set; }

        /// <summary>
        /// Gets the override kind (abstract, virtual, override, new), defaulting to none.
        /// </summary>
        public vsCMOverrideKind OverrideKind { get; private set; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        public IEnumerable<CodeParameter> Parameters { get; private set; }

        #endregion Properties
    }
}