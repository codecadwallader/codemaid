using System.Collections.Generic;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// A specialized container for holding a set of code items.
    /// </summary>
    public class SetCodeItems : List<BaseCodeItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetCodeItems" /> class.
        /// </summary>
        public SetCodeItems()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SetCodeItems" /> class initialized with the
        /// specified collection members.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public SetCodeItems(IEnumerable<BaseCodeItem> collection)
            : this()
        {
            AddRange(collection);
        }
    }
}