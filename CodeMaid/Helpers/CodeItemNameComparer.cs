using SteveCadwallader.CodeMaid.Model.CodeItems;
using System.Collections.Generic;

namespace SteveCadwallader.CodeMaid.Helpers
{
    /// <summary>
    /// A helper for comparing code items by name.
    /// </summary>
    public class CodeItemNameComparer : Comparer<BaseCodeItem>
    {
        /// <summary>
        /// Performs a comparison of two objects of the same type and returns a value indicating
        /// whether one object is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// Less than zero: <paramref name="x" /> is less than <paramref name="y" />.
        /// Zero: <paramref name="x" /> equals <paramref name="y" />.
        /// Greater than zero: <paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        public override int Compare(BaseCodeItem x, BaseCodeItem y)
        {
            int nameComparison = x.Name.CompareTo(y.Name);

            if (nameComparison == 0)
            {
                // Fall back to position comparison for matching elements.
                return x.StartOffset.CompareTo(y.StartOffset);
            }

            return nameComparison;
        }
    }
}