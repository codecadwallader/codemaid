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

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// The representation of a code using statemen.
    /// </summary>
    public class CodeItemUsingStatement : BaseCodeItemElement
    {
        #region BaseCodeItem Overrides

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public override KindCodeItem Kind
        {
            get { return KindCodeItem.Using; }
        }

        /// <summary>
        /// Refreshes the cached fields on this item.
        /// </summary>
        /// <remarks>
        /// Similar to BaseCodeItemElement's implementation, except ignores the Name property which is not available for using statements.
        /// </remarks>
        public override void Refresh()
        {
            StartLine = CodeElement.StartPoint.Line;
            StartOffset = CodeElement.StartPoint.AbsoluteCharOffset;
            EndLine = CodeElement.EndPoint.Line;
            EndOffset = CodeElement.EndPoint.AbsoluteCharOffset;
        }

        #endregion BaseCodeItem Overrides

        #region BaseCodeItemElement Overrides

        /// <summary>
        /// Gets the type string.
        /// </summary>
        public override string TypeString
        {
            get { return "using"; }
        }

        #endregion BaseCodeItemElement Overrides
    }
}