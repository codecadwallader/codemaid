using EnvDTE;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// An interface for code items.
    /// </summary>
    public interface ICodeItem
    {
        #region Properties

        /// <summary>
        /// Gets the kind.
        /// </summary>
        KindCodeItem Kind { get; }

        /// <summary>
        /// Gets or sets the name, may be empty.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the start line.
        /// </summary>
        int StartLine { get; set; }

        /// <summary>
        /// Gets or sets the start offset.
        /// </summary>
        int StartOffset { get; set; }

        /// <summary>
        /// Gets or sets the start point, may be null.
        /// </summary>
        EditPoint StartPoint { get; set; }

        /// <summary>
        /// Gets or sets the end line.
        /// </summary>
        int EndLine { get; set; }

        /// <summary>
        /// Gets or sets the end offset.
        /// </summary>
        int EndOffset { get; set; }

        /// <summary>
        /// Gets or sets the end point, may be null.
        /// </summary>
        EditPoint EndPoint { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Loads all lazy initialized values immediately.
        /// </summary>
        void LoadLazyInitializedValues();

        /// <summary>
        /// Refreshes the cached position and name fields on this item.
        /// </summary>
        void RefreshCachedPositionAndName();

        #endregion Methods
    }
}