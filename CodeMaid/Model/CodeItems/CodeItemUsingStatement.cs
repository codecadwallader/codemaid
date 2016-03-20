using System;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// The representation of a code using statemen.
    /// </summary>
    public class CodeItemUsingStatement : BaseCodeItemElement
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeItemUsingStatement" /> class.
        /// </summary>
        public CodeItemUsingStatement()
        {
            _TypeString = new Lazy<string>(
                () => "using");
        }

        #endregion Constructors

        #region BaseCodeItem Overrides

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public override KindCodeItem Kind => KindCodeItem.Using;

        /// <summary>
        /// Refreshes the cached position and name fields on this item.
        /// </summary>
        /// <remarks>
        /// Similar to BaseCodeItemElement's implementation, except ignores the Name property which
        /// is not available for using statements.
        /// </remarks>
        public override void RefreshCachedPositionAndName()
        {
            StartLine = CodeElement.StartPoint.Line;
            StartOffset = CodeElement.StartPoint.AbsoluteCharOffset;
            EndLine = CodeElement.EndPoint.Line;
            EndOffset = CodeElement.EndPoint.AbsoluteCharOffset;
        }

        #endregion BaseCodeItem Overrides
    }
}