using EnvDTE;
using System;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// An interface for code items that support having children.
    /// </summary>
    public interface ICodeItemParent : ICodeItem
    {
        /// <summary>
        /// An event raised when the IsExpanded state has changed.
        /// </summary>
        event EventHandler IsExpandedChanged;

        /// <summary>
        /// Gets the children of this code item, may be empty.
        /// </summary>
        SetCodeItems Children { get; }

        /// <summary>
        /// Gets the insert point, may be null.
        /// </summary>
        EditPoint InsertPoint { get; }

        /// <summary>
        /// Gets or sets the flag indicating if this parent item is expanded.
        /// </summary>
        bool IsExpanded { get; set; }
    }
}