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
using SteveCadwallader.CodeMaid.Helpers;

namespace SteveCadwallader.CodeMaid.CodeItems
{
    /// <summary>
    /// The representation of a code property.
    /// </summary>
    public class CodeItemProperty : BaseCodeItemElement
    {
        private int? _complexity;

        /// <summary>
        /// Gets or sets the underlying VSX CodeProperty.
        /// </summary>
        public CodeProperty CodeProperty { get; set; }

        /// <summary>
        /// Gets the access level.
        /// </summary>
        public override vsCMAccess Access
        {
            get { return CodeProperty != null ? CodeProperty.Access : vsCMAccess.vsCMAccessDefault; }
        }

        /// <summary>
        /// Gets a flag indicating if this property is static.
        /// </summary>
        public override bool IsStatic
        {
            get
            {
                return CodeProperty != null &&
                       ((CodeProperty.Getter != null && CodeProperty.Getter.IsShared) ||
                        (CodeProperty.Setter != null && CodeProperty.Setter.IsShared));
            }
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
    }
}