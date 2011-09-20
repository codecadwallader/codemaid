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

namespace SteveCadwallader.CodeMaid.CodeItems
{
    /// <summary>
    /// The representation of a code field.
    /// </summary>
    public class CodeItemField : BaseCodeItemElement
    {
        /// <summary>
        /// Gets or sets the underlying VSX CodeVariable.
        /// </summary>
        public CodeVariable CodeVariable { get; set; }

        /// <summary>
        /// Gets the access level.
        /// </summary>
        public override vsCMAccess Access
        {
            get { return CodeVariable != null ? CodeVariable.Access : vsCMAccess.vsCMAccessDefault; }
        }

        /// <summary>
        /// Gets a flag indicating if this field is a constant.
        /// </summary>
        public bool IsConstant
        {
            get { return CodeVariable != null && CodeVariable.IsConstant; }
        }
    }
}