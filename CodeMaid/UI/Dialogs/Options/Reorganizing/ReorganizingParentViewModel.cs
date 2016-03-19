using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.UI.Dialogs.Options.Reorganizing
{
    /// <summary>
    /// The view model for reorganizing options - a parent to more specific view models.
    /// </summary>
    public class ReorganizingParentViewModel : OptionsPageViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReorganizingParentViewModel" /> class.
        /// </summary>
        /// <param name="package">The hosting package.</param>
        /// <param name="activeSettings">The active settings.</param>
        public ReorganizingParentViewModel(CodeMaidPackage package, Settings activeSettings)
            : base(package, activeSettings)
        {
        }

        #endregion Constructors

        #region Overrides of OptionsPageViewModel

        /// <summary>
        /// Gets the header.
        /// </summary>
        public override string Header => "Reorganizing";

        #endregion Overrides of OptionsPageViewModel
    }
}