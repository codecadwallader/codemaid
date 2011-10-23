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
using SteveCadwallader.CodeMaid.Helpers;

namespace SteveCadwallader.CodeMaid.CodeItems
{
    /// <summary>
    /// The representation of a code method.
    /// </summary>
    public class CodeItemMethod : BaseCodeItemElement
    {
        private int? _complexity;

        /// <summary>
        /// Gets or sets the underlying VSX CodeFunction.
        /// </summary>
        public CodeFunction2 CodeFunction { get; set; }

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
        /// Gets the access level.
        /// </summary>
        public override vsCMAccess Access
        {
            get { return CodeFunction != null ? CodeFunction.Access : vsCMAccess.vsCMAccessDefault; }
        }

        /// <summary>
        /// Gets a flag indicating if this method is static.
        /// </summary>
        public override bool IsStatic
        {
            get { return CodeFunction != null && CodeFunction.IsShared; }
        }

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
            get { return CodeFunction != null && CodeFunction.FunctionKind == vsCMFunction.vsCMFunctionConstructor; }
        }

        /// <summary>
        /// Gets a flag indicating if this method is a destructor.
        /// </summary>
        public bool IsDestructor
        {
            get { return CodeFunction != null && CodeFunction.FunctionKind == vsCMFunction.vsCMFunctionDestructor; }
        }

        /// <summary>
        /// Gets a flag indicating if this method is overloaded.
        /// </summary>
        public bool IsOverloaded
        {
            get { return CodeFunction != null && CodeFunction.IsOverloaded; }
        }
    }
}