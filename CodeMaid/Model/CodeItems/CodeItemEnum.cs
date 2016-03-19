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
                () => CodeEnum != null ? CodeEnum.Access : vsCMAccess.vsCMAccessPublic);

            _Attributes = LazyTryDefault(
                () => CodeEnum != null ? CodeEnum.Attributes : null);

            _DocComment = LazyTryDefault(
                () => CodeEnum != null ? CodeEnum.DocComment : null);

            _Namespace = LazyTryDefault(
                () => CodeEnum != null && CodeEnum.Namespace != null ? CodeEnum.Namespace.Name : null);

            _TypeString = new Lazy<string>(
                () => "enum");
        }

        #endregion Constructors

        #region BaseCodeItem Overrides

        /// <summary>
        /// Gets the kind.
        /// </summary>
        public override KindCodeItem Kind
        {
            get { return KindCodeItem.Enum; }
        }

        #endregion BaseCodeItem Overrides

        #region Properties

        /// <summary>
        /// Gets or sets the underlying VSX CodeEnum.
        /// </summary>
        public CodeEnum CodeEnum { get; set; }

        #endregion Properties
    }
}