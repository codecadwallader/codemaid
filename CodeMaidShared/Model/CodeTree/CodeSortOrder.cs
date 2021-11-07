namespace SteveCadwallader.CodeMaid.Model.CodeTree
{
    /// <summary>
    /// An enumeration of code sort orders.
    /// </summary>
    public enum CodeSortOrder
    {
        /// <summary>
        /// The default sort following the file line order.
        /// </summary>
        File,

        /// <summary>
        /// A sort following the C# standards that groups items by type and accessibility.
        /// </summary>
        Type,

        /// <summary>
        /// A sort following alphabetical order.
        /// </summary>
        Alpha
    }
}