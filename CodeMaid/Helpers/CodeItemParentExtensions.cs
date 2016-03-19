using SteveCadwallader.CodeMaid.Model.CodeItems;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A set of extension methods for <see cref="ICodeItemParent" />.
    /// </summary>
    public static class CodeItemParentExtensions
    {
        /// <summary>
        /// Recursively gets the children in a depth-first fashion for the specified parent without
        /// delving into nested element parents.
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