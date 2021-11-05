using EnvDTE;
using EnvDTE80;
using System;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// The representation of a code interface.
    /// </summary>
    public class CodeItemInterface : BaseCodeItemElementParent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeItemInterface" /> class.
        /// </summary>
        public CodeItemInterface()
        {
            _Access = LazyTryDefault(
                () => CodeInterface?.Access ?? vsCMAccess.vsCMAccessPublic);

            _Attributes = LazyTryDefault(
                () => CodeInterface?.Attributes);

            _DocComment = LazyTryDefault(
                () => CodeInterface?.DocComment);

            _Namespace = LazyTryDefault(
                () => CodeInterface?.Namespace?.Name);

            _TypeString = new Lazy<string>(
                () => "interface");
        }

        #endregion Constructors

        #region BaseCodeItem Overrides

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public override KindCodeItem Kind => KindCodeItem.Interface;

        #endregion BaseCodeItem Overrides

        #region Properties

        /// <summary>
        /// Gets or sets the underlying VSX CodeInterface.
        /// </summary>
        public CodeInterface2 CodeInterface { get; set; }

        #endregion Properties
    }
}