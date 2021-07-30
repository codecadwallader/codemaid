using EnvDTE;
using System.Collections.Generic;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// An interface for code items that support having parameters.
    /// </summary>
    public interface ICodeItemParameters : ICodeItem
    {
        /// <summary>
        /// Gets the parameters.
        /// </summary>
        IEnumerable<CodeParameter> Parameters { get; }
    }
}