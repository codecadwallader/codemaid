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

using SteveCadwallader.CodeMaid.Model.CodeItems;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A set of extension methods for <see cref="ICodeItemParent"/>.
    /// </summary>
    public static class CodeItemParentExtensions
    {
        /// <summary>
        /// Recursively gets the children in a depth-first fashion for the specified parent without delving into nested element parents.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <returns>The recursive set of children.</returns>
        public static SetCodeItems GetChildrenRecursive(this ICodeItemParent parent)
        {
            var children = new SetCodeItems();

            foreach (var child in parent.Children)
            {
                children.Add(child);

                var childAsParent = child as ICodeItemParent;
                if (childAsParent != null && !(child is BaseCodeItemElementParent))
                {
                    children.AddRange(childAsParent.GetChildrenRecursive());
                }
            }

            return children;
        }
    }
}