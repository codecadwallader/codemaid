namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// An interface for code items that support complexity calculations.
    /// </summary>
    public interface ICodeItemComplexity : ICodeItem
    {
        /// <summary>
        /// Gets the complexity.
        /// </summary>
        int Complexity { get; }
    }
}