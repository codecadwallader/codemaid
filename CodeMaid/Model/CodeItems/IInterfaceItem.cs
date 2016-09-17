namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// Represents an item that can implement an interface member.
    /// </summary>
    public interface IInterfaceItem
    {
        /// <summary>
        /// Gets a flag indicating if this is an explicit interface implementation.
        /// </summary>
        bool IsExplicitInterfaceImplementation { get; }
    }
}
