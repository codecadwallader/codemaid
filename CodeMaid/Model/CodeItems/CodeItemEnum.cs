using EnvDTE;
using System;

namespace SteveCadwallader.CodeMaid.Model.CodeItems
{
    /// <summary>
    /// The representation of a code enumeration.
    /// </summary>
    public class CodeItemEnum : BaseCodeItemElementParent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeItemEnum" /> class.
        /// </summary>
        public CodeItemEnum()
        {
            _Access = LazyTryDefault(
                () => CodeEnum?.Access ?? vsCMAccess.vsCMAccessPublic);

            _Attributes = LazyTryDefault(
                () => CodeEnum?.Attributes);

            _DocComment = LazyTryDefault(
                () => CodeEnum?.DocComment);

            _Namespace = LazyTryDefault(
                () => CodeEnum?.Namespace?.Name);

            _TypeString = new Lazy<string>(
                () => "enum");
        }

        #endregion Constructors

        #region BaseCodeItem Overrides

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public override KindCodeItem Kind => KindCodeItem.Enum;

        #endregion BaseCodeItem Overrides

        #region Properties

        /// <summary>
        /// Gets or sets the underlying VSX CodeEnum.
        /// </summary>
        public CodeEnum CodeEnum { get; set; }

        #endregion Properties
    }
}