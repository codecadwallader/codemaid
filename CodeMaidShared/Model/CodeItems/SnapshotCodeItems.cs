using EnvDTE;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// A snapshot of a document and its associated code items at a point in time.
    /// </summary>
    internal class SnapshotCodeItems
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapshotCodeItems" /> class.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="codeItems">The code items.</param>
        internal SnapshotCodeItems(Document document, SetCodeItems codeItems)
        {
            Document = document;
            CodeItems = codeItems;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the document.
        /// </summary>
        internal Document Document { get; private set; }

        /// <summary>
        /// Gets the code items.
        /// </summary>
        internal SetCodeItems CodeItems { get; private set; }

        #endregion Properties
    }
}